using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] Enemy_Values enemy;
    [SerializeField] int health;
    public int Health => health;
    [SerializeField] Transform PlayerPosition;
    [SerializeField] Transform DirectionHelper;
    [SerializeField] Healthbar healthbar;

    [SerializeField] MatchManager manager;
    public bool isMatchStarted;


    [Header("Blocking")]
    [SerializeField] bool isBlocking = false;
    [SerializeField] float BlockingDamageMultiplyer = 0.05f;
    [SerializeField] float TimeSinceLastPlayerStrike = 0;
    [SerializeField] List<float> TimeBetweenPlayerStrikes;

    [Header("Movement")]
    [SerializeField] Transform MovementPointer;
    [SerializeField] Vector3 StageCenter;
    [SerializeField] Vector2 StageBounds;
    Vector2 XBounds = Vector2.zero;
    Vector2 ZBounds = Vector2.zero;
    [SerializeField] List<Vector3> MovementQueue;
    [SerializeField] Transform ArkMaker_Origin;
    [SerializeField] Transform ArkMaker_Distance;
    [SerializeField] bool MovingTowardsPlayer;

    [Header("Combat")]
    [SerializeField] FistDamageArea[] Fists = new FistDamageArea[2];
    [SerializeField] LayerMask PlayerDamageMask;
    int AmountOfJabsBeingDone;
    [SerializeField]List<float> Combat_JabTimes;
    [SerializeField]float Combat_Timer;

    [SerializeField] Animator EnemyAnimator;
    [SerializeField] string Animator_ParemeterName_IsMatchStarted;
    [SerializeField] string Animator_ParemeterName_IsJabbing;
    [SerializeField] string Animator_ParemeterName_IsBlocking;
    [SerializeField] string Animator_ParemeterName_IsDead;
    [SerializeField] string Animator_ParemeterName_HorizontalVector;
    [SerializeField] string Animator_ParemeterName_VerticalVector;

    bool isEnemyDead = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        health = enemy.MaxHealth;
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.Initialize(enemy.MaxHealth, true);

        XBounds = new Vector2(StageCenter.x - StageBounds.x, StageCenter.x + StageBounds.x);
        ZBounds = new Vector2(StageCenter.z - StageBounds.y, StageCenter.z + StageBounds.y);
    }

    // Update is called once per frame
    void Update()
    {


        if (!isEnemyDead && isMatchStarted)
        {
            EnemyAnimator.SetBool(Animator_ParemeterName_IsBlocking, isBlocking);
            EnemyLookAtPlayer_Update();
            Block_Update();

            if (!isBlocking)
            {

                if (MovingTowardsPlayer)
                {
                    EnemyAnimator.SetBool(Animator_ParemeterName_IsJabbing, false);
                    MoveTowardsPlayer_Update();
                }
                else if (MovementQueue.Count > 0)
                {
                    EnemyAnimator.SetBool(Animator_ParemeterName_IsJabbing, false);
                    Movement_Update();
                }
                else
                {
                    Combat_Update();
                }
            }

            /*if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                MakeMovementArk(true);
            }*/
        }

    }

    public void TakeDamage(int Damage)
    {
        int preDamageHealth = health;
        int finalDamage = Mathf.FloorToInt(Damage * (isBlocking ? BlockingDamageMultiplyer : 1) );
        health -= finalDamage;

        healthbar.UpdateHealthBar(preDamageHealth, health);

        if (TimeBetweenPlayerStrikes.Count == enemy.Block_NunberSavedAttacks)
        {
            TimeBetweenPlayerStrikes.Remove(TimeBetweenPlayerStrikes[0]);
            TimeBetweenPlayerStrikes.Add(TimeSinceLastPlayerStrike);
            TimeSinceLastPlayerStrike = 0;
        }
        else
        {
            TimeBetweenPlayerStrikes.Add(TimeSinceLastPlayerStrike);
            TimeSinceLastPlayerStrike = 0;
        }

        if (Health <= 0)
        {
            isEnemyDead = true;
            manager.Start_PlayerWinState();
            EnemyAnimator.SetBool(Animator_ParemeterName_IsDead, true);
        }
    }

    #region Blocking
    void Block_Update()
    {
        TimeSinceLastPlayerStrike += Time.deltaTime;

        if (!isBlocking)
        {
            if (TimeBetweenPlayerStrikes.Count == enemy.Block_NunberSavedAttacks)
            {
                float TotalTimeBetweenStrikes = 0;

                foreach (float TimeBetween in TimeBetweenPlayerStrikes)
                {
                    TotalTimeBetweenStrikes += TimeBetween;
                }

                float AverageTimeBetweenPlayerStrikes = TotalTimeBetweenStrikes / TimeBetweenPlayerStrikes.Count;

                if (AverageTimeBetweenPlayerStrikes <= enemy.Block_AverageSpeedUntilBlock)
                {
                    isBlocking = true;
                }
            }
        }
        else
        {
            if (TimeSinceLastPlayerStrike >= enemy.Block_TimeUntilStopBlocking)
            {
                isBlocking = false;
                TimeBetweenPlayerStrikes.Clear();
            }
        }
    }
    #endregion

    #region Movement


    void Movement_Update()
    {
        MovementPointer.LookAt(MovementQueue[0]);
        transform.position += Time.deltaTime * enemy.Movement_Speed * MovementPointer.forward;

        if (Vector3.Distance(MovementQueue[0], transform.position) <= 0.05f)
        {
            MovementQueue.Remove(MovementQueue[0]);
        }
    }

    void MakeMovementArk(bool isClockwise)
    {
        if (isClockwise)
        {
            EnemyAnimator.SetFloat(Animator_ParemeterName_HorizontalVector, 1);
        }
        else
        {
            EnemyAnimator.SetFloat(Animator_ParemeterName_HorizontalVector, -1);
        }

        int MovementDegrees = Random.Range(enemy.Movement_MinArkDegrees, enemy.Movement_MaxArkDegrees + 1);

        ArkMaker_Origin.position = PlayerPosition.position;
        ArkMaker_Origin.position = new Vector3(ArkMaker_Origin.position.x,transform.position.y, ArkMaker_Origin.position.z);
        ArkMaker_Origin.LookAt(transform.position);
        ArkMaker_Distance.localPosition = new Vector3(0,0,Vector3.Distance(transform.position, ArkMaker_Origin.position));

        for (int i = 0; i < MovementDegrees; i++)
        {
            if (isClockwise)
            {
                ArkMaker_Origin.Rotate(0, 1, 0);
            }
            else
            {
                ArkMaker_Origin.Rotate(0, -1, 0);
            }

            if (CheckIfPointIsWithinTheRing(ArkMaker_Distance.position))
            {
                MovementQueue.Add(ArkMaker_Distance.position);
            }
        }
    }
    
    void MoveTowardsPlayer_Update()
    {
        EnemyAnimator.SetFloat(Animator_ParemeterName_HorizontalVector, 0);
        EnemyAnimator.SetFloat(Animator_ParemeterName_VerticalVector, 1);

        ArkMaker_Origin.position = PlayerPosition.position;
        ArkMaker_Origin.position = new Vector3(ArkMaker_Origin.position.x, transform.position.y, ArkMaker_Origin.position.z);
        MovementPointer.LookAt(ArkMaker_Origin);

        transform.position += Time.deltaTime * enemy.Movement_Speed * MovementPointer.forward;

        if (Vector3.Distance(transform.position, PlayerPosition.position) <= enemy.Movement_PreferedDistanceFromPlayer)
        {
            MovingTowardsPlayer = false;
            EnemyAnimator.SetFloat(Animator_ParemeterName_VerticalVector, 0);
        }
    }

    bool CheckIfPointIsWithinTheRing(Vector3 MovementPoint)
    {
        bool output = false;

        if (MovementPoint.x >= XBounds.x && MovementPoint.x <= XBounds.y)
        {
            output = true;
        }

        if (output)
        {
            if (MovementPoint.z >= ZBounds.x && MovementPoint.z <= ZBounds.y)
            {
                output = true;
            }
        }

        return output;
    }

    void EnemyLookAtPlayer_Update()
    {
        DirectionHelper.position = PlayerPosition.position;
        DirectionHelper.position = new Vector3(DirectionHelper.position.x, transform.position.y, DirectionHelper.position.z);

        transform.LookAt(DirectionHelper.position);
    }

    #endregion

    #region Combat

    void Combat_Update()
    {
        if (AmountOfJabsBeingDone == 0)
        {
            EnemyAnimator.SetBool(Animator_ParemeterName_IsJabbing, false);

            if (Vector3.Distance(transform.position, PlayerPosition.position) >= enemy.Movement_ToFarAwayFromPlayer)
            {
                MovingTowardsPlayer = true;
                MovementQueue.Clear();
            }

            if (MovementQueue.Count == 0)
            {
                int RandomWeight = Random.Range(1, enemy.Movement_MovementBeforeAttackingRandomizer + 1);

                if (RandomWeight == 1)
                {
                    Combat_StartJabCycle();
                }
                else
                {
                    int isClockwise = Random.Range(1, 3);

                    if (isClockwise == 1)
                    {
                        MakeMovementArk(true);
                    }
                    else
                    {
                        MakeMovementArk(false);
                    }
                }
            }
        }
        else
        {
            EnemyAnimator.SetBool(Animator_ParemeterName_IsJabbing, true);

            Combat_Timer -= Time.deltaTime;

            if (Combat_Timer <= 0)
            {
                Combat_JabTimes.Remove(Combat_JabTimes[0]);

                if (Combat_JabTimes.Count == 0)
                {
                    AmountOfJabsBeingDone = 0;
                }
                else
                {
                    Combat_Timer = Combat_JabTimes[0];
                    foreach (FistDamageArea FistArea in Fists)
                    {
                        FistArea.HasHitPlayerThisRound = false;
                    }
                }
            }

            CheckFistAreas();
        }
    }

    void Combat_StartJabCycle()
    {
        AmountOfJabsBeingDone = Random.Range(1, enemy.Combat_JabTimes.Length + 1);

        for (int i = 0; i < AmountOfJabsBeingDone; i++)
        {
            Combat_JabTimes.Add(enemy.Combat_JabTimes[i]);
        }

        Combat_Timer = Combat_JabTimes[0];

        foreach (FistDamageArea FistArea in Fists)
        {
            FistArea.HasHitPlayerThisRound = false;
        }
    }

    void CheckFistAreas()
    {
        foreach (FistDamageArea FistArea in Fists)
        {
            if (!FistArea.HasHitPlayerThisRound)
            {
                Collider[] LidarsInRange = Physics.OverlapSphere(FistArea.FistBone.position, FistArea.Radius, PlayerDamageMask);
                foreach (Collider Lidar in LidarsInRange)
                {
                    float Velocity = Vector3.Distance(FistArea.PreviousPosition, FistArea.FistBone.position);
                    PrototypeDealDamageToPlayer(enemy.Combat_Damage * FistArea.DamageMultiplyer * (Velocity * enemy.Combat_Damage_SpeedModifier));
                    FistArea.HasHitPlayerThisRound = true;
                }
                FistArea.PreviousPosition = FistArea.FistBone.position;
            }
        }
    }

    void PrototypeDealDamageToPlayer(float Damage)
    {
        Debug.Log(gameObject.name + " did " +  Damage.ToString() + " to the player.");
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 Point1 = StageCenter + (Vector3.right * StageBounds.x) + (Vector3.forward * StageBounds.y);
        Vector3 Point2 = StageCenter + (-Vector3.right * StageBounds.x) + (Vector3.forward * StageBounds.y);
        Vector3 Point3 = StageCenter + (-Vector3.right * StageBounds.x) + (-Vector3.forward * StageBounds.y);
        Vector3 Point4 = StageCenter + (Vector3.right * StageBounds.x) + (-Vector3.forward * StageBounds.y);

        Gizmos.DrawLine(Point1, Point2);
        Gizmos.DrawLine(Point2, Point3);
        Gizmos.DrawLine(Point3, Point4);
        Gizmos.DrawLine(Point4, Point1);

        foreach (FistDamageArea fistArea in Fists)
        {
            Gizmos.DrawWireSphere(fistArea.FistBone.position, fistArea.Radius);
        }

        if (MovementQueue.Count > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < MovementQueue.Count; i++)
            {
                if (i == 0)
                {
                    Gizmos.DrawLine(transform.position, MovementQueue[i]);
                }
                else
                {
                    Gizmos.DrawLine(MovementQueue[i - 1], MovementQueue[i]);
                }
            }
        }
    }
}

[System.Serializable]
public class FistDamageArea
{
    public Transform FistBone;
    public float Radius;
    public float DamageMultiplyer;
    [HideInInspector]public bool HasHitPlayerThisRound;
    [HideInInspector]public Vector3 PreviousPosition;
}