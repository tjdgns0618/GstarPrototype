using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneul : MonoBehaviour        // 주변 적 지속피해
{
    GameManager gm;

    void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider col)   // particle system collision으로 바꿔줘야함
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (eAI != null)
            {
                eAI.Damage(gm._damage * ItemDataBase.instance.Variable2(36));
            }
        }
    }
}
