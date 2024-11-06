using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward*Time.deltaTime*8f);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = col.GetComponent<IDamageAble<float>>();
            if (damageAble != null)
            {
                damageAble.Damage(gm._damage * (ItemDataBase.instance.Variable(44) + (ItemDataBase.instance.Variable2(44) * (gm.FindItemCount(44) - 1))));
            }
        }
    }
}
