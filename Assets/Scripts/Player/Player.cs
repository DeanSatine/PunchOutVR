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
    #endregion

    #region Variables and Properties


    #region Player Stats
    public readonly int maxHealth = 100;
    public int currentHealth;
    public int baseDamage = 10;

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
        currentHealth = maxHealth;

        GetComponentInChildren<Healthbar>().Initialize(maxHealth);

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

    }

    private void InitializeControllers()
    {
        // assign the controllers based on XR nodes
        LeftControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        RightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }
    #endregion
}
