using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWork : MonoBehaviour
{
    public float ascentSpeed = 5f;          // 폭죽이 올라가는 속도
    public float trackingSpeed = 2f;        // 적을 추적할 때의 속도
    public float maxHeight = 10f;           // 폭죽이 올라갈 최대 높이
    public float rotationSpeed = 2f;
    public Transform target;                // 추적할 적
    private Vector3 targetPosition;         // 적의 위치
    private bool isTracking = false;        // 적을 추적하는 단계인지 여부
    private Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Rebind();
        isTracking = false;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        if (!isTracking)
        {
            // 1. 폭죽이 하늘로 올라가는 단계
            Ascend();
        }
        else
        {
            // 2. 적을 추적하는 단계 (곡선 비행)
            TrackTarget();
        }
    }

    private void Ascend()
    {
        // 상승 중일 때 Y축 방향으로 이동
        transform.position += Vector3.up * ascentSpeed * Time.deltaTime;

        // 일정 높이에 도달하면 적을 포착
        if (transform.position.y >= maxHeight)
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        // 적을 찾는 로직 (간단히 가까운 적을 찾는 방식으로 구현)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            // 가장 가까운 적을 찾기
            float closestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                if (!enemy.GetComponent<EnemyAI>().isDead)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }

            // 가까운 적을 추적 대상으로 설정
            if (closestEnemy != null)
            {
                target = closestEnemy.transform;
                targetPosition = target.position;
                isTracking = true; // 적을 추적하는 단계로 변경
            }
        }
    }

    private void TrackTarget()
    {
        if (target != null)
        {
            GetComponent<Animator>().SetTrigger("Rotate");

            // 3. 목표 적을 향해 곡선을 그리며 이동
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 곡선 경로를 따라 이동 (slerp를 이용한 부드러운 회전)
            transform.position = Vector3.Slerp(transform.position, targetPosition, trackingSpeed * Time.deltaTime);

            // 목표에 도달하면 오브젝트를 비활성화
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                // 목표에 도달했을 때 처리 (폭발 또는 비활성화)
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            damageAble?.Damage(GameManager.instance._damage);
            GameManager.instance.particlePoolManager.ReturnParticle(this.gameObject);
        }
    }
}
