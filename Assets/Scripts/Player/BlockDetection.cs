using System;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class BlockDetection : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if(col.TryGetComponent(out PlayerFist hand))
        {
            hand.isBlocking = true;
            Player.instance.UpdateBlockState();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent(out PlayerFist hand))
        {
            hand.isBlocking = false;
            Player.instance.UpdateBlockState();
        }
    }

}
