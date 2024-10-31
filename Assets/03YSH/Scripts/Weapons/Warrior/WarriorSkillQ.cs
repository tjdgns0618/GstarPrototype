using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkillQ : MonoBehaviour
{
    SphereCollider sphereCollider;
    private void OnEnable()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            damageAble?.Damage(GameManager.instance._damage * 1f);
            damageAble?.PlayKnockback(other.transform.position - PlayerCharacter.Instance.transform.position, 0.2f, 0.2f);
        }
    }
}
