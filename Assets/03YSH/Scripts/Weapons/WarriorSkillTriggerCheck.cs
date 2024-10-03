using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WarriorSkillTriggerCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit Enemy");

            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            damageAble?.Damage(GameManager.instance._damage);
        }
    }
}
