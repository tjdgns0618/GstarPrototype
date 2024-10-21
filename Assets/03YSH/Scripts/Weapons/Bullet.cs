using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20.0f;
    public string targetname;
    private void OnEnable()
    {
        Invoke("InActiveParticle", 5f);
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
            gameObject.SetActive(false);
        }
    }
}
