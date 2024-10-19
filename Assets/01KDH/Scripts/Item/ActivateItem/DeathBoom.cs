using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (eAI != null)
            {
                eAI.Damage(10f);
            }
        }
    }
}
