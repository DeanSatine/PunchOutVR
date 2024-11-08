using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BodyPart : MonoBehaviour
{
    [SerializeField] Enemy_Controller enemyController;
    [SerializeField] float BodyPartDamageMultiplyer = 1;

    [Header("Prototyping")]
    [SerializeField] bool isInPrototypingPhase = false;
    [SerializeField] float Prototype_Damage;
    [SerializeField] KeyCode Prototype_Key;
    private void Update()
    {
        if (isInPrototypingPhase)
        {
            if (Input.GetKeyDown(Prototype_Key))
            {
                TakeDamage(Prototype_Damage);
            }
        }
    }

    public void TakeDamage(float BaseDamage)
    {
        enemyController.TakeDamage(BaseDamage * BodyPartDamageMultiplyer);
        Debug.Log("You struck " + enemyController.name + " in the " + gameObject.name + " for a base damage of " + BaseDamage.ToString() + " and a total damage of " + (BaseDamage * BodyPartDamageMultiplyer).ToString() + ".");
    }
}
