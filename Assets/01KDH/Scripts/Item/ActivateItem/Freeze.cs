using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    float timer = 0;
    float duration = 0.5f;
    public GameObject target;


    private void OnEnable()
    {
        timer = 0;
    }

    private void OnDisable()
    {
        timer = 0;
        target = null;
    }

    private void Update()
    {
        if (target != null)
            transform.position = target.transform.position;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") && timer != 0f)
        {
            EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (eAI != null)
            {
                eAI.Slow(ItemDataBase.instance.Variable2(47) + (ItemDataBase.instance.Variable3(47) * (GameManager.instance.FindItemCount(47) - 1)));
            }
        }
    }
}
