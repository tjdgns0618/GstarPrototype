using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBoom : MonoBehaviour
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
                damageAble.Damage(gm._damage * (ItemDataBase.instance.Variable(30) + ItemDataBase.instance.Variable2(30) * (gm.FindItemCount(30)-1)));
            }
        }
    }
}
