using UnityEngine;

public class Ready_Up_Manager : MonoBehaviour
{

    bool rightHandReady;
    bool leftHandReady;

    bool isReady => rightHandReady && leftHandReady;

    public void UpdateHand(bool isRightHand, bool value)
    {
        if (isRightHand) rightHandReady = value;
        else leftHandReady = value;
        if (isReady)
        {
            MatchManager.instance.playerReadied = true;
            OnReady();
        }
    }  


    void OnReady()
    {
        
       gameObject.SetActive(false);
    }

}
