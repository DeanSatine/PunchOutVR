using System;
using UnityEngine;

using UnityEngine.XR;

using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    #region References
    public static Player instance;
    //references to left and right controller.
    InputDevice LeftControllerDevice;
    InputDevice RightControllerDevice;
    #endregion

    #region Variables and Properties
    // current velocity of left and right controller.
    Vector3 LeftHandVelocity;
    Vector3 RightHandVelocity;
    // public tuple containing velocity of both hands. 
    public (Vector3 left, Vector3 right) HandVelocities => (RightHandVelocity, LeftHandVelocity);

    #endregion

    #region Methods
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }
    void Start()
    {
        InitializeControllers();
    }

    void Update()
    {
        if (!LeftControllerDevice.isValid || !RightControllerDevice.isValid) // checks every frame for controllers if either left or right controller is not valid.
        {
            InitializeControllers();
            Debug.Log("Controllers Re-initialized.");
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
