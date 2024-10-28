using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    GameManager gm;

    void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider col)   // particle system collision¿∏∑Œ πŸ≤„¡‡æﬂ«‘
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = col.GetComponent<IDamageAble<float>>();
            // EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (damageAble != null)
            {
                damageAble.Damage(1f);
            }
        }
    }
}
