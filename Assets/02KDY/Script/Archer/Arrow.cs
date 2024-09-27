using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;           // ����ü �ӵ�
    public float lifetime = 5f;         // ����ü�� ���� �ð� (��)
    private Vector3 targetPosition;     // ��ǥ ��ġ (���콺 Ŭ�� ��ġ)

    // ����ü�� �ʱ�ȭ�ϴ� �Լ�
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

        // ����ü�� ��ǥ ��ġ�� ���ϵ��� ȸ��
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        // ��ǥ ��ġ�� ����ü �̵�
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // ����ü�� ���� �ð��� ������ �ı�
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
