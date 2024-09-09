using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleCollisionCheck : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        damageAble?.Damage(20f);
    }

}
