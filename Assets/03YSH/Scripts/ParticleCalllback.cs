using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleCalllback : MonoBehaviour
{
    ParticleSystem ps;    

    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            if (damageAble != null)
            {
                damageAble.Damage(GameManager.instance._damage * 2);
            }
        }
    }

    //private void OnParticleTrigger()
    //{
    //    // particles
    //    List<ParticleSystem.Particle> insideParticles = new List<ParticleSystem.Particle>();

    //    // get
    //    int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideParticles);

    //    for (int i = 0; i < numInside; i++)
    //    {
    //        ParticleSystem.Particle particle = insideParticles[i];
    //        Collider[] colliders = Physics.OverlapSphere(particle.position, 1f);

    //        foreach (Collider collider in colliders)
    //        {
    //            if (collider.CompareTag("Enemy"))
    //            {
    //                IDamageAble<float> damageAble = collider.GetComponent<IDamageAble<float>>();
    //                if (damageAble != null)
    //                {
    //                    damageAble.Damage(GameManager.instance._damage * 2);
    //                }
    //            }
    //        }
    //    }
    //}
}
