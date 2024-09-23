using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_09 : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = col.gameObject.GetComponent<EnemyAI>();
            //enemy.hp -= 5f;
        }
        if (col.gameObject.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
        }
    }
}
