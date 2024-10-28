using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    int shieldHp = 5;

    public GameObject shieldBoomParticle;

    void BoomShield()
    {
        Instantiate(shieldBoomParticle, transform.position, Quaternion.identity);
        Debug.Log("particle inst");
        DestroyImmediate(shieldBoomParticle, true);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            shieldHp--;
            if (shieldHp <= 0)
            {
                Destroy(col.gameObject);
                BoomShield();
            }
        }
    }
}
