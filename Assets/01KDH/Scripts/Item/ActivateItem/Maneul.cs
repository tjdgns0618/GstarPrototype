using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneul : MonoBehaviour        // �ֺ� �� ��������
{
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = col.gameObject.GetComponent<EnemyAI>();
            enemy.Damage(2f);
        }
    }
}
