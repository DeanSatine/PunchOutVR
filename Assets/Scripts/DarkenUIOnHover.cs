using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DarkenUIOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]Image image;
    static Color darkenedColour = new Color(0.8f, 0.8f, 0.8f, 1);
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = darkenedColour;
    }

    // Called when the mouse exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}