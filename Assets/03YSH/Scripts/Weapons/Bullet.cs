using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20.0f;
    public string targetname;
    public GameObject EffectsOnCollision;
    public float DestroyTimeDelay;

    private void OnEnable()
    {
        Invoke("InActiveParticle", 7f);
    }

    private void OnDisable()
    {

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
            Debug.LogWarning(other.name);
            damageable?.Damage(GameManager.instance._damage);
            GameObject instance = Instantiate(EffectsOnCollision, other.transform.position, Quaternion.identity);
            GameManager.instance.particlePoolManager.ReturnParticle(this.gameObject);
            
        }
    }
}
