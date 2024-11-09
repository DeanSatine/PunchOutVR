using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThreeDMenu_Button : MonoBehaviour
{
    public UnityEvent OnClick;
    [SerializeField] Color NeutralColor = Color.white;
    [SerializeField] Color HoverColor = Color.white;
    [HideInInspector] public bool isHoveringOver;

    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoveringOver)
        {
            ren.material.color = HoverColor;
        }
        else
        {
            ren.material.color = NeutralColor;
        }

        isHoveringOver = false;
    }
}
