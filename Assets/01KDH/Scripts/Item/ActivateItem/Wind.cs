using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.forward*Time.deltaTime*8f);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (eAI != null)
            {
                eAI.Damage(0.5f);
            }
        }
    }
}
