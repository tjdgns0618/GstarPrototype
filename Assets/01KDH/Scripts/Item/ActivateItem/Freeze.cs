using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (eAI != null)
            {
                eAI.Slow(ItemDataBase.instance.Variable2(47) + (ItemDataBase.instance.Variable3(47) * (GameManager.instance.FindItemCount(47) - 1)));
            }
        }
    }
}
