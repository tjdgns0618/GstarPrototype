using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class EnemyAttack : MonoBehaviour
{
    MeshCollider MeshCollider;

    private void Awake()
    {
        MeshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        MeshCollider.convex = true;
        MeshCollider.isTrigger = true;
        MeshCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        damageAble?.Damage(20);
    }
}
