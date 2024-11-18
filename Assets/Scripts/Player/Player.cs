using System;
using UnityEngine;
using FMODUnity;
using UnityEngine.XR;

using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using FMOD.Studio;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Player : MonoBehaviour
{
    #region References
    [Header("References")]
    public static Player instance;
    //references to left and right controller.
    InputDevice LeftControllerDevice;
    InputDevice RightControllerDevice;

    [SerializeField] PlayerFist rightHand;
    [SerializeField] PlayerFist leftHand;
    [SerializeField] EventReference punchEvent;
    EventInstance crowdCheer;
    [SerializeField] EventReference crowdCheerEvent;
    EventInstance heartbeat;
    [SerializeField] EventReference heartbeatEvent;
    Healthbar healthBar;
    #endregion

    #region Variables and Properties


    #region Player Stats
    public bool isRightHanded;
    public readonly int maxHealth = 100;
    public int currentHealth;
    public int baseDamage = 10;

    const float blockingDamageMultiplier = 1;//0.1f;
    [SerializeField] float punchSoundVelocityThreshhold;
    [SerializeField] float punchSoundCooldown;
    WaitForSeconds punchCooldown;

    Coroutine leftHandPunchSound;
    Coroutine rightHandPunchSound;
    Coroutine IFrameCoroutine;

    #endregion


    // current velocity of left and right controller.
    Vector3 LeftHandVelocity;
    Vector3 RightHandVelocity;
    // public tuple containing velocity of both hands. 
    public (Vector3 left, Vector3 right) HandVelocities => (RightHandVelocity, LeftHandVelocity);

    // actions are assigned () =>{}; as this assigns an empty method to them. without this, invoking an action before subscribing another method to it will crash the game.
    public Action OnBlockEnter = () => { };
    public Action OnBlockExit = () => { };
    bool isBlocking;

    public bool IsBlocking
    {
        get { return isBlocking; }
        set
        {
            isBlocking = value;
            if (value) OnBlockEnter(); // when blocking state changes, invoke the respective actions.
            else OnBlockExit();
        }
    }

    #endregion

    #region Methods
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }
    public void UpdateBlockState() => IsBlocking = rightHand.isBlocking && leftHand.isBlocking;
    void Start()
    {
        InitializeControllers();
        InitializePlayer();
        currentHealth = maxHealth;
        AudioManager.instance.UpdateHealthFmodParam(currentHealth);
        punchCooldown = new WaitForSeconds(punchSoundCooldown);
        healthBar = GetComponentInChildren<Healthbar>(true);
        healthBar.Initialize(maxHealth, false);
        // rn i have both these actions doing the same thing. later we can do different effects depending though.
        OnBlockEnter += () =>
        {
            DEBUG_UI.instance.SetBlockingStateText(IsBlocking);
        };

        OnBlockExit += () =>
        {
            DEBUG_UI.instance.SetBlockingStateText(IsBlocking);
        };

        //play crowd cheer and heartbeat fmod event
        crowdCheer = RuntimeManager.CreateInstance(crowdCheerEvent);
        crowdCheer.start();

        heartbeat = RuntimeManager.CreateInstance(heartbeatEvent);
        heartbeat.start();
    }
    void Update()
    {
        if (!LeftControllerDevice.isValid || !RightControllerDevice.isValid) // checks every frame for controllers if either left or right controller is not valid.
        {
            InitializeControllers();
        }


        // poll controllers for current velocity.
        LeftControllerDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out LeftHandVelocity);
        RightControllerDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out RightHandVelocity);


        if (LeftHandVelocity.magnitude > punchSoundVelocityThreshhold) {

            if (leftHandPunchSound == null)
            {
                leftHandPunchSound = StartCoroutine(PlayPunch());
            }
        }
        if(RightHandVelocity.magnitude > punchSoundVelocityThreshhold) {

            if (rightHandPunchSound == null)
            {
                rightHandPunchSound = StartCoroutine(PlayPunch(true));
            }
        }



    }

    IEnumerator PlayPunch(bool isRightHand = false)
    {
        AudioManager.instance.PlayOneShot(punchEvent);
        yield return punchCooldown;
        if (isRightHand) rightHandPunchSound = null;
        else leftHandPunchSound = null;

    }


    public void ChangeHandedness(bool isRightHanded)
    {
        this.isRightHanded = isRightHanded;
        Transform newHand = isRightHanded ? leftHand.gloveCanvas.transform : rightHand.gloveCanvas.transform; // UI will be on the non dominant hand.
        healthBar.transform.SetParent(newHand, false);
    }

    public void TakeDamage(int damage)
    {
        if (IFrameCoroutine != null) return;
        int preDamageHealth = currentHealth;
        currentHealth -= Mathf.FloorToInt(damage * (IsBlocking ? blockingDamageMultiplier : 1)); // if blocking, deal 25% reduced damage
        healthBar.UpdateHealthBar(preDamageHealth, currentHealth);

        if(currentHealth <= 0)
        {
            MatchManager.instance.Start_PlayerLoseState();
        }
        AudioManager.instance.UpdateHealthFmodParam(currentHealth);
        IFrameCoroutine = StartCoroutine(IFrames(0.35f));
        SendVibration(1, 1);
        
    }
    IEnumerator IFrames(float duration)
    {
        yield return new WaitForSeconds(duration);
        IFrameCoroutine = null;
    }

    private void InitializeControllers()
    {
        // assign the controllers based on XR nodes
        LeftControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        RightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }
    #endregion
    /// <summary>
    /// chose hand for vibration,
    /// </summary>

    public void SendVibration(bool isRightHand,float amplitude, float duration)
    {

        HapticImpulsePlayer hapticHandler = isRightHand ? rightHand.GetComponentInParent<HapticImpulsePlayer>() : leftHand.GetComponentInParent<HapticImpulsePlayer>();
        hapticHandler.SendHapticImpulse(amplitude, duration);
       
    }
    /// <summary>
    /// Sends vibration to both hands.
    /// </summary>

    public void SendVibration(float amplitude,float duration){
        rightHand.GetComponentInParent<HapticImpulsePlayer>().SendHapticImpulse(amplitude, duration);
        leftHand.GetComponentInParent<HapticImpulsePlayer>().SendHapticImpulse(amplitude, duration);
    }
    private void InitializePlayer()
    {
        Settings.OnHandednessChange += (value) => ChangeHandedness(value);
    }

    private void OnDestroy()
    {
        Settings.OnHandednessChange -= (value) => ChangeHandedness(value);

        heartbeat.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        heartbeat.release();
        crowdCheer.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        crowdCheer.release();
    }
}
