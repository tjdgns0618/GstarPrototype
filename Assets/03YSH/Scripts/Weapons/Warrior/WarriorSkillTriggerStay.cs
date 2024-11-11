using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkillTriggerStay : MonoBehaviour
{
    string _EnemyTag = "Enemy";
    float hitTime = 0.2f;
    BoxCollider boxCollider;

    IDamageAble<float> damageAble;

    private void Update()
    {
        transform.position = PlayerCharacter.Instance.transform.position + Vector3.up * 2f;
        transform.localScale = new Vector3(3, 1, 3);
    }

    private void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider>();
        StartCoroutine(ColCoroutine());
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
            Destroy(other.gameObject);

        if (other.CompareTag("Enemy") && other.gameObject.layer != 7)
        {
            damageAble = other.GetComponent<IDamageAble<float>>();
            damageAble?.Damage(GameManager.instance._damage * 0.2f);
            if(other.gameObject.layer != 13)
                damageAble?.PlayKnockback(other.transform.position - PlayerCharacter.Instance.transform.position, 0.1f, 0.1f);
        }
    }

    IEnumerator ColCoroutine()
    {
        while (true)
        {
            boxCollider.enabled = true;
            yield return new WaitForSeconds(hitTime);
            boxCollider.enabled = false;
            yield return new WaitForSeconds(hitTime);
        }
    }

}
