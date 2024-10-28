using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WizardBigbang : MonoBehaviour
{
    List<GameObject> insideEnemies = new List<GameObject>();
    string _EnemyTag = "Enemy";
    float knockbackTimer;
    float knockbackTime = 0.5f;
    float explosionTimer;
    float explosionTime = 4.7f;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 1f);

        knockbackTimer += Time.deltaTime;
        explosionTimer += Time.deltaTime;
        if (knockbackTime <= knockbackTimer)
        {
            for (int i = 0; i < insideEnemies.Count; i++)
            {
                Vector3 _dir = (transform.position - insideEnemies[i].transform.position).normalized;
                insideEnemies[i]?.GetComponent<EnemyAI>().PlayKnockback(_dir, 0.2f, 2f);
            }
            knockbackTimer = 0;
        }

        if(explosionTime <= explosionTimer)
        {
            for (int i = 0; i < insideEnemies.Count; i++)
            {
                insideEnemies[i]?.GetComponent<IDamageAble<float>>().Damage(GameManager.instance._damage * 3f);
            }
            explosionTimer = 0;
            gameObject.SetActive(false);            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_EnemyTag))
        {
            insideEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_EnemyTag))
        {
            insideEnemies.Remove(other.gameObject);
            
        }
    }
}
