using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble<float>
{
    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Damage(float damageTaken)
    {
        throw new System.NotImplementedException();
    }

    public void Dead()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageAble<float> damageAble = collision.gameObject.GetComponent<IDamageAble<float>>();
        if(damageAble != null)
        {
            damageAble.Damage(20);
        }
    }
}
