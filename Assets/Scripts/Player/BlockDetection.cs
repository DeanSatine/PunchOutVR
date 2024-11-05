using System;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class BlockDetection : MonoBehaviour
{

    #region Variables and Properties
    // actions are assigned () =>{}; as this assigns an empty method to them. without this, invoking an action before subscribing another method to it will crash the game.
    public Action OnBlockEnter = () => { };
    public Action OnBlockExit= () => { };
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
    byte handsBlocking = 0; // tracks how many hands are in the block trigger attached this gameobject.

    #endregion

    #region Methods

    private void Start()
    {
        // rn i have both these actions doing the same thing. later we can do different effects depending though.
        OnBlockEnter += () =>
        {
            DEBUG_UI.instance.SetBlockingStateText(IsBlocking); 
        };

        OnBlockExit += () =>
        {
            DEBUG_UI.instance.SetBlockingStateText(IsBlocking);
        };
        handsBlocking = 0; // for some reason it tends to automatically set this to 1 when i start. i really don't know why but this is a simple solution.
    }


    // Only interacts with the "PlayerHands" layer.
    private void OnTriggerEnter(Collider col)
    {
        handsBlocking++; // if hand enters trigger, increment counter
        CheckIsBlocking();
    }
    // Only interacts with the "PlayerHands" layer.
    private void OnTriggerExit(Collider col)
    {
        handsBlocking--; // if hand enters trigger, decrement counter
        CheckIsBlocking(); // assign blocking value.
    }


    void CheckIsBlocking()
    {
        IsBlocking = handsBlocking >= 2;
    }
    #endregion
}
