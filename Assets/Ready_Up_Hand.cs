using UnityEngine;

public class Ready_Up_Hand : MonoBehaviour
{
    [SerializeField] bool isRightHand;
    Ready_Up_Manager manager;
    private void Start()
    {
        manager = GetComponentInParent<Ready_Up_Manager>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerFist hand)){
            manager.UpdateHand(hand.IsRightHand, true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerFist hand))
        {
            manager.UpdateHand(hand.IsRightHand, false);
        }
    }
}
