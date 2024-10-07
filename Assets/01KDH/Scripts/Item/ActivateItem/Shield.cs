using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    int shieldHp = 1;

    public GameObject shieldBoomParticle;

    WaitForSeconds shieldgen;

    private void Start()
    {
        shieldgen = new WaitForSeconds(3f);
    }

    private void Update()
    {
        if(shieldHp <= 0)
            StartCoroutine(ShieldRegen());
    }

    void BoomShield()
    {
        Instantiate(shieldBoomParticle, transform.position, Quaternion.identity);
        Destroy(shieldBoomParticle);
        gameObject.SetActive(false);
    }

    IEnumerator ShieldRegen()
    {
        BoomShield();
        Debug.Log("regen...");
        yield return shieldgen;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
            shieldHp--;
        }
    }
}
