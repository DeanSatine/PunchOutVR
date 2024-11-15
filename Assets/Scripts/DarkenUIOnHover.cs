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
        if (image == null) return;
        image.color = darkenedColour;
        AudioManager.instance.PlayOneShot(AudioManager.instance.UI_OnHover);
    }

    // Called when the mouse exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        if (image == null) return;
        image.color = Color.white;
    }
}