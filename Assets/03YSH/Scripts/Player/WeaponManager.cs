using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager
{
    public BaseWeapon Weapon { get; private set; }
    public Action<GameObject> unRegisterWeapon { get; set; }
    private GameObject weaponObject;
    private List<GameObject> weapons = new List<GameObject>();
    

    public void RegisterWeapon(GameObject weapon)
    {
        if (!weapons.Contains(weapon))
        {
            BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
            weapons.Add(weapon);
        }
    }

    public void SetWeapon(GameObject weapon)
    {
        if(Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<BaseWeapon>();
            PlayerCharacter.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
            return;
        }

        weaponObject = weapon;
        Weapon = weapon.GetComponent<BaseWeapon>();
        PlayerCharacter.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
    }
}
