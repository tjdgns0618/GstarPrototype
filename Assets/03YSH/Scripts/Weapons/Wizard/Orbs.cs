using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.Burst burst;

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        burst = ps.emission.GetBurst(0);
        burst.count = 4;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            damageAble?.Damage(10f);

            var emission = ps.emission;
            burst.count = 3;
            emission.SetBurst(0, burst);
        }
    }
}
