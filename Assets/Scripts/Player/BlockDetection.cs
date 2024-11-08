using System;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class BlockDetection : MonoBehaviour
{

    // Only interacts with the "PlayerHands" layer.
    private void OnTriggerEnter(Collider col)
    {
        if(col.TryGetComponent(out PlayerFist hand))
        {
            hand.isBlocking = true;
            Player.instance.UpdateBlockState();
        }
    }
    // Only interacts with the "PlayerHands" layer.
    private void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent(out PlayerFist hand))
        {
            hand.isBlocking = false;
            Player.instance.UpdateBlockState();
        }
    }

}
