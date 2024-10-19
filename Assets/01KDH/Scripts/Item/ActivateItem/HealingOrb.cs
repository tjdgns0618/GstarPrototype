using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    GameManager gm;
    void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            gm.Heal(5f);
        }
    }
}
