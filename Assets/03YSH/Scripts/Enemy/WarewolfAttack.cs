using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarewolfAttack : MonoBehaviour
{
    BoxCollider boxCollider;
    WareWolfBoss warewolf;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        warewolf = GetComponentInParent<WareWolfBoss>();
    }

    private void Start()
    {
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        if (other.tag == "Player")
            damageAble?.Damage(warewolf.damage);
    }

    private void OnTriggerStay(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        if (other.tag == "Player")
            damageAble?.Damage(warewolf.damage);
    }
}
