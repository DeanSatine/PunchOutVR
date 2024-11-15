using UnityEngine;

public class EnemyFist : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("dmg");
        }
    }
}
