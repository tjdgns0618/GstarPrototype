using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControll : MonoBehaviour
{
    private float speed = 20.0f;
    public string targetname;
    public GameObject EffectsOnCollision;
    public float DestroyTimeDelay;
    public float damagePercentage = 1;
    public bool isBaseAttack = false;

    //private void OnEnable()
    //{
    //    Invoke("InActiveParticle", 7f);
    //}

    private void OnEnable()
    {
        Invoke("InActiveParticle", DestroyTimeDelay);
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
            damageable?.Damage(GameManager.instance._damage * damagePercentage);
            if(isBaseAttack)
                GameManager.instance.enemyhitDelegate(other.gameObject);
            damageable?.PlayKnockback(other.transform.position - PlayerCharacter.Instance.transform.position, 0.2f, 0.2f);
            GameObject instance = Instantiate(EffectsOnCollision, other.transform.position, Quaternion.identity);
            // GameManager.instance.particlePoolManager.ReturnParticle(this.gameObject);            
        }
    }
}
