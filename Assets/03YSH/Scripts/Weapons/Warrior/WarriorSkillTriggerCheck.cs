using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WarriorSkillTriggerCheck : MonoBehaviour
{
    [SerializeField]private float speed = 20.0f;


    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit Enemy");

            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            damageAble?.Damage(GameManager.instance._damage * 2F);
        }
    }
}
