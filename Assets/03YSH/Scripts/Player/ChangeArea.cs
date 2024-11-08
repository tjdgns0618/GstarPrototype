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
            if (other.gameObject.layer != 13 || other.gameObject.layer != 7)
                other.GetComponent<EnemyAI>().PlayKnockback(other.transform.position - PlayerCharacter.Instance.transform.position, 0.4f, 0.6f);
        }
    }
}
