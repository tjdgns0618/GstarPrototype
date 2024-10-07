using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWork : MonoBehaviour
{
    public float ascentSpeed = 5f;          // ������ �ö󰡴� �ӵ�
    public float trackingSpeed = 2f;        // ���� ������ ���� �ӵ�
    public float maxHeight = 10f;           // ������ �ö� �ִ� ����
    public float rotationSpeed = 2f;
    public Transform target;                // ������ ��
    private Vector3 targetPosition;         // ���� ��ġ
    private bool isTracking = false;        // ���� �����ϴ� �ܰ����� ����
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
            // 1. ������ �ϴ÷� �ö󰡴� �ܰ�
            Ascend();
        }
        else
        {
            // 2. ���� �����ϴ� �ܰ� (� ����)
            TrackTarget();
        }
    }

    private void Ascend()
    {
        // ��� ���� �� Y�� �������� �̵�
        transform.position += Vector3.up * ascentSpeed * Time.deltaTime;

        // ���� ���̿� �����ϸ� ���� ����
        if (transform.position.y >= maxHeight)
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        // ���� ã�� ���� (������ ����� ���� ã�� ������� ����)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            // ���� ����� ���� ã��
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

            // ����� ���� ���� ������� ����
            if (closestEnemy != null)
            {
                target = closestEnemy.transform;
                targetPosition = target.position;
                isTracking = true; // ���� �����ϴ� �ܰ�� ����
            }
        }
    }

    private void TrackTarget()
    {
        if (target != null)
        {
            GetComponent<Animator>().SetTrigger("Rotate");

            // 3. ��ǥ ���� ���� ��� �׸��� �̵�
            Vector3 direction = (targetPosition - transform.position).normalized;

            // � ��θ� ���� �̵� (slerp�� �̿��� �ε巯�� ȸ��)
            transform.position = Vector3.Slerp(transform.position, targetPosition, trackingSpeed * Time.deltaTime);

            // ��ǥ�� �����ϸ� ������Ʈ�� ��Ȱ��ȭ
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                // ��ǥ�� �������� �� ó�� (���� �Ǵ� ��Ȱ��ȭ)
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
