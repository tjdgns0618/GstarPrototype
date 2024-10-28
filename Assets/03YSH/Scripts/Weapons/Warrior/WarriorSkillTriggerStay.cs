using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkillTriggerStay : MonoBehaviour
{
    string _EnemyTag = "Enemy";
    float hitTimer;
    float hitTime = 0.5f;

    IDamageAble<float> damageAble;

    List<GameObject> insideEnemies;

    Coroutine timerCheck;

    private void Update()
    {
        transform.position = PlayerCharacter.Instance.transform.position + Vector3.up * 1f;
        transform.localScale = new Vector3(3, 1, 3);

        hitTimer += Time.deltaTime;

        if (hitTime <= hitTimer)
        {
            for (int i = 0; i < insideEnemies.Count; i++)
            {
                insideEnemies[i]?.GetComponent<IDamageAble<float>>().Damage(GameManager.instance._damage * 0.2f);
            }
            hitTimer = 0;
        }

    }

    private void OnEnable()
    {
        insideEnemies = new List<GameObject>();
    }

    private void OnDisable()
    {
        if(insideEnemies != null)
            insideEnemies.Clear();
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
            Destroy(other.gameObject);

        if (other.CompareTag("Enemy"))
        {            
            insideEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            insideEnemies.Remove(other.gameObject);
        }
    }

}
