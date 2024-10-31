using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerExplosion : MonoBehaviour
{
    public SphereCollider SphereCollider;
    public float damageMultiplier = 1;

    private void OnEnable()
    {
        SphereCollider.enabled = false;
        StopCoroutine(enableCollider());
        StartCoroutine(enableCollider());
    }

    public IEnumerator enableCollider()
    {
        yield return new WaitForSeconds(1f);
        SphereCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        SphereCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            damageAble?.Damage(GameManager.instance._damage * damageMultiplier);
            enemy.PlayKnockback(enemy.transform.position - PlayerCharacter.Instance.transform.position, 0.2f, 0.2f);
        }
    }

}
