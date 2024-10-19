using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeArea : MonoBehaviour
{
    string enemyTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        if (damageAble != null && other.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyAI>().PlayKnockback(other.transform.forward * -1f, 0.4f, 5f);
        }
    }
}
