using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BodyPart : MonoBehaviour
{
    [SerializeField] Enemy_Controller enemyController;
    [SerializeField] float bodyPartDamageMultiplier = 1;
    public float BodyPartDamageMultiplier => bodyPartDamageMultiplier;



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

    public void TakeDamage(float damage)
    {
        int finalDamage = Mathf.FloorToInt(damage * bodyPartDamageMultiplier);
        enemyController.TakeDamage( finalDamage );
       // Debug.Log("You struck " + enemyController.name + " in the " + gameObject.name + " for a base damage of " + damage.ToString() + " and a total damage of " + finalDamage.ToString() + ".");

}
