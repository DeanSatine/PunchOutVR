using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DEBUG_UI : MonoBehaviour
{
    #region References
    public static DEBUG_UI instance;
    [SerializeField] TextMeshProUGUI blockingState;
    [SerializeField] TextMeshProUGUI rightHandVelocity;
    [SerializeField] TextMeshProUGUI leftHandVelocity;
    #endregion

    #region Methods
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }

    public void SetBlockingStateText(bool state)
    {
        blockingState.text = state ? "Blocking" : "Not Blocking";
        blockingState.color = state ? Color.green : Color.red;
    }
    private void Update()
    {
        SetHandVelocityText(Player.instance.HandVelocities);
    }
    public void SetHandVelocityText((Vector3 right, Vector3 left) hand)
    {
        rightHandVelocity.text = $"Velocity (R): {hand.right}\n" +
            $"Speed (R): {hand.right.magnitude}";
        leftHandVelocity.text = $"Velocity (l): {hand.left}\n" +
            $"Speed (L): {hand.left.magnitude}";
    }
    #endregion
}
