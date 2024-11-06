using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAttack : MonoBehaviour
{
    BoxCollider boxCollider;
    EnemyAI enemyAI;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        enemyAI = GetComponentInParent<EnemyAI>();       
    }

    public void Start()
    {
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        if (other.tag == "Player")
            damageAble?.Damage(enemyAI.damage);
    }
}
