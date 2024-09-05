using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerAttack : MonoBehaviour
{
    public event EventHandler Click;
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
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
        if (other.tag == "Enemy")
        {
            damageAble?.Damage(20);
            Debug.Log("Hit Enemy");
        }
    }
}
