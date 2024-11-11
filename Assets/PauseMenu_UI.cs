using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PauseMenu_UI : MonoBehaviour
{
    public static PauseMenu_UI instance;
    public bool paused = false;
    [SerializeField] Canvas pauseCanvas;

    public InputActionAsset inputActionAsset;
    private InputAction pauseAction;


    [Header("SubMenus")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    const float menuDistance = 1.5f;

    private void Awake()
    {
        if(instance != null) { Destroy(instance.gameObject); }
        instance = this;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseCanvas.enabled = paused = false;

        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void Pause(InputAction.CallbackContext context) // param only so that i can call subscribe it to action.
    {
        if (paused) // if already paused, unpause.
        {
            Unpause();
            return;
        }
        Time.timeScale = 0;
        pauseCanvas.enabled = paused = true;
        Transform cameraTransform = Camera.main.transform;
        Vector3 forwards = cameraTransform.forward;
        forwards.y = 0; // exclude y value from position offset.
        Vector3 newPosition = cameraTransform.position + (forwards * menuDistance);
        newPosition.y = 1.1f; // manually set height
        transform.position = newPosition;
        transform.LookAt(cameraTransform.position); // face player

    }


    private void OnEnable()
    {
        var xrControls = inputActionAsset.FindActionMap("CustomInputs");
        pauseAction = xrControls.FindAction("Pause");
        pauseAction.performed += Pause;
        pauseAction?.Enable();
    }

    private void OnDisable()
    {
        pauseAction?.Disable();
    }
}
