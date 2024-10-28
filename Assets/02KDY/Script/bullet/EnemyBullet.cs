using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 20.0f;
    public string targetname;
    public GameObject hitEffectPrefab;
    private Rigidbody rb;
    public GameObject flash;
    private void OnEnable()
    {
        Invoke("InActiveParticle", 5f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }
    public void InActiveParticle()
    {
        GameManager.instance.particlePoolManager.ReturnParticle(this.gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageable = other.GetComponent<IDamageAble<float>>();
        if (other.tag == targetname)
        {
            damageable?.Damage(GameManager.instance._damage);
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }
}