using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCrackSkill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageable = other.GetComponent<IDamageAble<float>>();
        if (other.CompareTag("Enemy"))
        {
            damageable?.Damage(GameManager.instance._damage * 2);
        }
    }
}
