using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCol : MonoBehaviour
{
    GameManager gm;

    void Start()
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
                damageAble.Damage(gm._damage * (ItemDataBase.instance.Variable2(38) + (ItemDataBase.instance.Variable3(38) * (gm.FindItemCount(38) - 1))));
            }
        }
    }
}
