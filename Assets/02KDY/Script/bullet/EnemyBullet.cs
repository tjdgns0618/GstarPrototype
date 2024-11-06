using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public enum FireMode
    {
        Single,        // 단일 방향 발사
        Triple,        // 삼방향 발사
        SpreadHoming,  // 확산 후 추적
        Homing         // 추적샷
    }

    public FireMode fireMode;           // 발사 모드 설정
    public float speed = 20.0f;         // 총알 속도 (Inspector에서 조절 가능)
    public string targetname;
    public GameObject hitEffectPrefab;
    private Rigidbody rb;
    public GameObject flash;
    public float destroyDelay = 10.0f;
    public float angleOffset = 30f;     // 각도 오프셋 (삼방향 및 확산용)
    public float homingDelay = 1.0f;    // 확산 후 추적 딜레이 시간
    private Transform target;           // 추적할 목표
    public float damage;


    private void OnEnable()
    {
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag(targetname)?.transform;


        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();

            if (flashPs != null)
                Destroy(flashInstance, flashPs.main.duration);
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }

        if (fireMode == FireMode.Triple)
        {
            FireTriple();
        }
        else if (fireMode == FireMode.SpreadHoming)
        {
            FireSpread();
            StartCoroutine(StartHoming());
        }
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        if (fireMode == FireMode.Homing && target != null)
        {
            // 목표 방향으로 이동하는 코드
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 2.0f); // 호밍 샷 유도율 조정
        }

        // 속도에 따라 총알 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void FireTriple()
    {
        GameObject bulletRight = Instantiate(gameObject, transform.position, transform.rotation);
        bulletRight.transform.Rotate(0, angleOffset, 0);
        bulletRight.GetComponent<EnemyBullet>().fireMode = FireMode.Single;

        GameObject bulletLeft = Instantiate(gameObject, transform.position, transform.rotation);
        bulletLeft.transform.Rotate(0, -angleOffset, 0);
        bulletLeft.GetComponent<EnemyBullet>().fireMode = FireMode.Single;

        fireMode = FireMode.Single;
    }

    private void FireSpread()
    {
        GameObject bulletLeft = Instantiate(gameObject, transform.position, transform.rotation);
        bulletLeft.transform.Rotate(0, -angleOffset, 0);
        bulletLeft.GetComponent<EnemyBullet>().fireMode = FireMode.Single;

        GameObject bulletRight = Instantiate(gameObject, transform.position, transform.rotation);
        bulletRight.transform.Rotate(0, angleOffset, 0);
        bulletRight.GetComponent<EnemyBullet>().fireMode = FireMode.Single;

        fireMode = FireMode.Single;
    }

    private IEnumerator StartHoming()
    {
        yield return new WaitForSeconds(homingDelay);

        while (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 2.0f); // 호밍 샷 유도율 조정

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageable = other.GetComponent<IDamageAble<float>>();
        if (other.tag == targetname)
        {
            damageable?.Damage(damage);
            if (hitEffectPrefab != null)
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
        }
    }
}
