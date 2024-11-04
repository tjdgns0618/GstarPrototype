using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRing : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 4f);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            IDamageAble<float> damageAble = col.GetComponent<IDamageAble<float>>();
            damageAble.Damage(GameManager.instance._maxhp * 0.02f);
        }
    }
}
