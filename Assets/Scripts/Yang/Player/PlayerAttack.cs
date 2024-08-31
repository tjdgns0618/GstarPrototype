using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class PlayerAttack : MonoBehaviour
{
    public event EventHandler Click;
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

    public void MouseButtonDown()
    {
        if(this.Click != null)
        {
            Click(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        damageAble?.Damage(20);
    }
}
