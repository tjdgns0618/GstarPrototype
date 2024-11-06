/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ParticleCollisionInstance : MonoBehaviour
{
    public GameObject[] EffectsOnCollision;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 transformOffset = new Vector3(0,0,0);
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    public bool rotationZero = false;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;
    public int itemID;
    public float damageMultiplier = 1;
    public bool isBaseAttack = false;


    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    public void InActiveParticle()
    {
        GameManager.instance.particlePoolManager.ReturnParticle(this.gameObject);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            damageAble?.Damage(GameManager.instance._damage * (ItemDataBase.instance.Variable2(itemID) +
                                  (ItemDataBase.instance.Variable3(itemID) * (GameManager.instance.FindItemCount(itemID) - 1))));

            if (isBaseAttack)
                GameManager.instance.enemyhitDelegate(other.gameObject);
            for (int i = 0; i < numCollisionEvents; i++)
            {
                foreach (var effect in EffectsOnCollision)
                {
                    var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * Offset + transformOffset, Quaternion.identity) as GameObject;
                    if (!UseWorldSpacePosition) instance.transform.parent = transform;
                    if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
                    else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
                    else
                    {
                        instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                        instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                    }
                    if (rotationZero)
                    {
                        instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                    }
                }
            }
            if (DestoyMainEffect == true)
            {
                // GameManager.instance.particlePoolManager.ReturnParticle(this.gameObject);
            }
        }
    }
}
