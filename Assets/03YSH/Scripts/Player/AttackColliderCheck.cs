using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackColliderCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        if (damageAble != null)
        {
            damageAble?.Damage(GameManager.instance._damage);
            GameManager.instance.enemyhitDelegate(other.gameObject);
            damageAble.PlayKnockback(other.transform.position - PlayerCharacter.Instance.transform.position, 0.2f, 0.2f);
        }
    }

}
