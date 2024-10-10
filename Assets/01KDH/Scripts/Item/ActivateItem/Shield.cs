using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    int shieldHp = 1;

    private bool isShieldDestroy = false;

    private float shieldGenT;
    private float shieldT = 3f;


    public GameObject shieldBoomParticle;

    void BoomShield()
    {
        Instantiate(shieldBoomParticle, transform.position, Quaternion.identity);
        Debug.Log("particle inst");
        DestroyImmediate(shieldBoomParticle, true);
        isShieldDestroy = true;
        gameObject.SetActive(false);
    }

    void ShieldRegen()
    {
        BoomShield();
        if (isShieldDestroy)
        {
            shieldGenT += Time.deltaTime;
            Debug.Log(shieldGenT);
            if(shieldGenT <= shieldT)
            {
                isShieldDestroy = false;
                shieldGenT = 0;
                gameObject.SetActive(true);
            }
        }
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
            //Debug.Log(" def");
            //shieldHp--;
        }
    }
}
