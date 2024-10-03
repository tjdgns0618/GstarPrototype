using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    private static ItemManager instance;
    public GameObject equipWeapon;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        equipWeapon.SetActive(true);
        PlayerCharacter.Instance.weaponManager.SetWeapon(equipWeapon);
        PlayerCharacter.Instance.weaponManager.RegisterWeapon(equipWeapon);
    }
}
