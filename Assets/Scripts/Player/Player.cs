using System;
using UnityEngine;

using UnityEngine.XR;

using UnityEngine.XR.Interaction.Toolkit;

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
    Healthbar healthBar;
    #endregion

    #region Variables and Properties


    #region Player Stats
    public bool isRightHanded;
    public readonly int maxHealth = 100;
    public int currentHealth;
    public int baseDamage = 10;

    const float blockingDamageMultiplier = 1;//0.1f;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeHandedness(!isRightHanded);
        }


    }

    public void ChangeHandedness(bool isRightHanded)
    {
        this.isRightHanded = isRightHanded;
        Transform newHand = isRightHanded ? leftHand.gloveCanvas.transform : rightHand.gloveCanvas.transform; // UI will be on the non dominant hand.
        healthBar.transform.SetParent(newHand, false);
    }

    public void TakeDamage(int damage)
    {

        int preDamageHealth = currentHealth;
        currentHealth -= Mathf.FloorToInt(damage * (IsBlocking ? blockingDamageMultiplier : 1)); // if blocking, deal 25% reduced damage
        healthBar.UpdateHealthBar(preDamageHealth, currentHealth);
        
    }

    private void InitializeControllers()
    {
        // assign the controllers based on XR nodes
        LeftControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        RightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }
    #endregion

    private void InitializePlayer()
    {
        Settings.OnHandednessChange += (value) => ChangeHandedness(value);
    }

    private void OnDestroy()
    {
        Settings.OnHandednessChange -= (value) => ChangeHandedness(value);
    }
}
