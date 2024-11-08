using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCrackSkill2 : MonoBehaviour
{
    public GameObject EffectsOnCollision;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;

    [SerializeField] private float damagePercentage;
    BoxCollider boxCollider;

    private void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        GameManager.instance.cameraManager.ShakeCamera(2f, 0.3f);
        StartCoroutine(disableCollider());
    }

    private IEnumerator disableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageable = other.GetComponent<IDamageAble<float>>();
        if (other.CompareTag("Enemy"))
        {
            damageable?.Damage(GameManager.instance._damage * damagePercentage);
            damageable?.PlayKnockback(other.transform.position - PlayerCharacter.Instance.transform.position, 0.2f, 0.2f);
            var instance = GameManager.instance.particlePoolManager.GetParticle("YellowFlash");
            instance.transform.position = other.transform.position;
            if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
            else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else
            {
                instance.transform.rotation *= Quaternion.Euler(rotationOffset);
            }
        }
    }
}