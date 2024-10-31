using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCol : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            var component = collision.collider.gameObject.GetComponent<IDamageAble<float>>();
            component.Damage(GameManager.instance._maxhp * 0.02f);
        }
    }
}
