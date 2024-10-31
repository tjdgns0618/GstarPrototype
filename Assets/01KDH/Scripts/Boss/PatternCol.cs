using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCol : MonoBehaviour
{
    private bool isIn = false;
    private IDamageAble<float> player;

    private void Update()
    {
        if (isIn)
        {
            player.Damage(GameManager.instance._maxhp * 0.02f);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = true;
            player = other.GetComponent<IDamageAble<float>>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
        }
    }
}
