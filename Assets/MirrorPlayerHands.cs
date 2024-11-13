using UnityEngine;

public class MirrorPlayerHands : MonoBehaviour
{
    [Header("Original Hands")]
    [SerializeField] Transform leftHandOriginal;   // Left VR controller
    [SerializeField] Transform rightHandOriginal;  // Right VR controller

    [Header("Cutscene Hands")]
    [SerializeField] Transform leftHandMirrored;   // Left hand in cutscene
    [SerializeField] Transform rightHandMirrored;  // Right hand in cutscene

    [SerializeField] Transform cutsceneCameraTransform;     // Cutscene camera

    void LateUpdate()
    {
        if (cutsceneCameraTransform == null)
            return;

        // Mirror the left hand
        if (leftHandOriginal != null && leftHandMirrored != null)
        {
            MirrorHand(leftHandOriginal, leftHandMirrored);
        }

        // Mirror the right hand
        if (rightHandOriginal != null && rightHandMirrored != null)
        {
            MirrorHand(rightHandOriginal, rightHandMirrored);
        }
    }

    private void MirrorHand(Transform originalHand, Transform cutsceneHand)
    {
        // Calculate position and rotation offsets relative to the controller
        Vector3 offsetPosition = originalHand.position - cutsceneCameraTransform.position;
        Quaternion offsetRotation = Quaternion.Inverse(cutsceneCameraTransform.rotation) * originalHand.rotation;

        // Apply offset to the cutscene hand in the cutscene camera's space
        cutsceneHand.position = cutsceneCameraTransform.position + offsetPosition;
        cutsceneHand.rotation = cutsceneCameraTransform.rotation * offsetRotation;
    }
}
