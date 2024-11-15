using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu_UI : PauseMenu_UI
{
    [SerializeField] EventReference startGame;
    [SerializeField] EventReference exitGame;

    // disables these 3 methods in base class by overriding them with no actual code.
    internal override void Awake()
    {
        
    }
    public override void Pause(InputAction.CallbackContext context)
    {
        
    }
    public override void Unpause()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(start());
        IEnumerator start()
        {
            AudioManager.instance.PlayOneShot(startGame);
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
    public void ExitGame()
    {
        StartCoroutine(Exit());
        IEnumerator Exit()
        {
            AudioManager.instance.PlayOneShot(exitGame);
            yield return new WaitForSeconds(3);
            Application.Quit();
        }
    }


}
