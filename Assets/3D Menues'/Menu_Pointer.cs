using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_Pointer : MonoBehaviour
{
    [SerializeField] LayerMask UIMask;
    [SerializeField] bool isInputClicked;
    [SerializeField] LineRenderer MenuRayHelper;
    [SerializeField] bool ShowDebugLine=true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward,out hit, 100, UIMask))
        {
            MenuRayHelper.gameObject.SetActive(true);
            MenuRayHelper.SetPosition(0, transform.position);
            MenuRayHelper.SetPosition(1, hit.point);

            ThreeDMenu_Button menuButton = hit.transform.GetComponent<ThreeDMenu_Button>();
            if (menuButton != null )
            {
                menuButton.isHoveringOver = true;

                if (isInputClicked)
                {
                    menuButton.OnClick.Invoke();
                }
            }
        }
        else
        {
            MenuRayHelper.gameObject.SetActive(false);
        }

        isInputClicked = false;
    }

    private void OnDrawGizmos()
    {
        if (ShowDebugLine)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 100));
        }
    }

    public void TryPressButton()
    {
        isInputClicked = true;
    }
}
