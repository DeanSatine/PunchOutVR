using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BodyPart : MonoBehaviour
{
    [SerializeField] Enemy_Controller enemyController;
    [SerializeField] float BodyPartDamageMultiplyer = 1;

    [Header("Prototyping")]
    [SerializeField] bool ProtottypeStrike = false;
    [SerializeField] float Prototype_Damage;
    private void Update()
    {
        if (ProtottypeStrike)
        {
                TakeDamage(Prototype_Damage);
            ProtottypeStrike = false;
        }
    }

    public void TakeDamage(float BaseDamage)
    {
        enemyController.TakeDamage(BaseDamage * BodyPartDamageMultiplyer);
        Debug.Log("You struck " + enemyController.name + " in the " + gameObject.name + " for a base damage of " + BaseDamage.ToString() + " and a total damage of " + (BaseDamage * BodyPartDamageMultiplyer).ToString() + ".");
    }
}
