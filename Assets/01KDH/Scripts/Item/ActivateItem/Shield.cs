using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);
        Debug.Log(col.gameObject.name);
        if (col.gameObject.CompareTag("Bullet"))
        {
            GameManager.instance.particlePoolManager.GetParticle("Popcorn");
            Destroy(col.gameObject);
        }
    }
}
