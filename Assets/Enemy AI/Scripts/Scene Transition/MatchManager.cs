using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;

    [Header("UI / Fading & Timings")]
    [SerializeField] RawImage VSScreen;
    [SerializeField] float VSScreenTimeOnScreen;
    [SerializeField] float VSScreenFadeTime;

    [SerializeField] Image FadeInOut;
    [SerializeField] float FadeInTime;
    [SerializeField] float FadeOutTime_PlayerWin;
    [SerializeField] float FadeOutTime_EnemyWin;
    float Fade_Timer;

    [Header("Script Refrences")]
    [SerializeField] Enemy_Controller enemy_Controller;

    [Header("Scene Transitions")]
    public float EnemyDeathTime;
    [SerializeField] int ThisScene_Index;
    [SerializeField] int NextScene_Index;
    [SerializeField] bool prototypePlayerLose;

    float EnemyDeathTimer;

    bool isMatchStart = true;
    int OnMatchStartPart = 1;
    bool isEnemyDead = false;
    bool isPlayerDead;
    public bool playerReadied = false;

    private void Awake()
    {
        if (instance != null) { Destroy(instance.gameObject); }
        instance = this;
    }
    void Start()
    {
        Fade_Timer = FadeInTime;
        FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, Fade_Timer / FadeInTime);
        StartCoroutine(VSScreenLoop());

    }


    IEnumerator VSScreenLoop()
    {
        yield return new WaitUntil(() => isMatchStart);

        while (OnMatchStartPart == 1){
                Fade_Timer -= Time.deltaTime;
                if (Fade_Timer <= 0)
                {
                    Fade_Timer = VSScreenTimeOnScreen;
                    OnMatchStartPart++;
                    FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, 0);
                }
                else
                {
                    FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, Fade_Timer / FadeInTime);
                }
            yield return null;
        }

        while(OnMatchStartPart == 2){
            Fade_Timer -= Time.deltaTime;
            if (Fade_Timer <= 0)
            {
                Fade_Timer = VSScreenFadeTime;
                OnMatchStartPart++;
            }
            yield return null;
        }


        yield return new WaitUntil(() => playerReadied);

        while(Fade_Timer >=0)
        {
            Fade_Timer -= Time.deltaTime;

            if (Fade_Timer <= 0)
            {
                enemy_Controller.isMatchStarted = true;
                isMatchStart = false;
                VSScreen.color = new Color(VSScreen.color.r, VSScreen.color.g, VSScreen.color.b, 0);
            }
            else
            {
                VSScreen.color = new Color(VSScreen.color.r, VSScreen.color.g, VSScreen.color.b, Fade_Timer / VSScreenFadeTime);
            }
            yield return null;
        }


    }
    
    void Update()
    {
        /*if (isMatchStart)
        {
            if (OnMatchStartPart == 1)
            {
                Fade_Timer -= Time.deltaTime;
                if (Fade_Timer <= 0)
                {
                    Fade_Timer = VSScreenTimeOnScreen;
                    OnMatchStartPart++;
                    FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, 0);
                }
                else
                {
                    FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, Fade_Timer / FadeInTime);
                }
            }
            else if (OnMatchStartPart == 2)
            {
                Fade_Timer -= Time.deltaTime;
                if (Fade_Timer <= 0)
                {
                    Fade_Timer = VSScreenFadeTime;
                    OnMatchStartPart++;
                }
            }
            else
            {
                Fade_Timer -= Time.deltaTime;

                if (Fade_Timer <= 0)
                {
                    enemy_Controller.isMatchStarted = true;
                    isMatchStart = false;
                    VSScreen.color = new Color(VSScreen.color.r, VSScreen.color.g, VSScreen.color.b, 0);
                }
                else
                {
                    VSScreen.color = new Color(VSScreen.color.r, VSScreen.color.g, VSScreen.color.b, Fade_Timer / VSScreenFadeTime);
                }
            }
        }*/

        if (EnemyDeathTimer > 0)
        {
            EnemyDeathTimer -= Time.deltaTime;

            if (EnemyDeathTimer <= 0)
            {
                isEnemyDead = true;
                Fade_Timer = 0;
            }
        }

        if (isEnemyDead)
        {
            Fade_Timer += Time.deltaTime;
            FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, Fade_Timer / FadeOutTime_PlayerWin);

            if (Fade_Timer >= FadeOutTime_PlayerWin)
            {
                SceneManager.LoadScene(NextScene_Index);
            }
        }

        if (isPlayerDead)
        {
            Fade_Timer += Time.deltaTime;
            FadeInOut.color = new Color(FadeInOut.color.r, FadeInOut.color.g, FadeInOut.color.b, Fade_Timer / FadeOutTime_EnemyWin);

            if (Fade_Timer >= FadeOutTime_EnemyWin)
            {
                SceneManager.LoadScene(ThisScene_Index);
            }
        }

    }

    public void Start_PlayerWinState()
    {
        EnemyDeathTimer = EnemyDeathTime;
    }

    public void Start_PlayerLoseState()
    {
        isPlayerDead= true;
    }
}
