using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggerCheck : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageAble<float> damageable = other.GetComponent<IDamageAble<float>>();
            damageable?.Damage(GameManager.instance._damage);
        }
    }

}
