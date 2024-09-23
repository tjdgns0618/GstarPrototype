using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackColliderCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        damageAble?.Damage(PlayerCharacter.Instance.weaponManager.Weapon.AttackDamage);
    }

}
