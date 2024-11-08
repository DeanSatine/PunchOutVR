using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy_Values : ScriptableObject
{
    [Header("Health")]
    public float MaxHealth;

    [Header("Blocking")]
    public int Block_NunberSavedAttacks;
    public float Block_AverageSpeedUntilBlock;
    public float Block_TimeUntilStopBlocking;

    [Header("Movement")]
    public float Movement_Speed;
    public int Movement_MinArkDegrees;
    public int Movement_MaxArkDegrees;
    public float Movement_ToFarAwayFromPlayer;
    public float Movement_PreferedDistanceFromPlayer;
    public int Movement_MovementBeforeAttackingRandomizer;

    [Header("Combat")]
    public float Combat_Damage;
    public float Combat_Damage_SpeedModifier;
    public float[] Combat_JabTimes;
}
