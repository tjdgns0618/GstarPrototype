using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_08 : MonoBehaviour        // �ֺ� �� ��������
{
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = col.gameObject.GetComponent<EnemyAI>();
            //enemy.hp -= 2f;
        }
    }
}
