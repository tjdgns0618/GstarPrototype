using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeulCol : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = col.GetComponent<IDamageAble<float>>();
            if (damageAble != null)
            {
                damageAble.Damage(gm._damage * (ItemDataBase.instance.Variable3(36)));
            }
        }
    }
}
