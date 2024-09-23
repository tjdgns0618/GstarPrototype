using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackColliderCheck : MonoBehaviour
{
    private void Start()
    {
        PlayerCharacter.Instance.weaponManager.Weapon.ItemChance += test1;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        if (damageAble != null)
        {
            PlayerCharacter.Instance.weaponManager.Weapon.ItemChance();
            damageAble?.Damage(PlayerCharacter.Instance.weaponManager.Weapon.AttackDamage);
        }
    }

    public void test1()
    {
        Debug.Log("1");
    }

}
