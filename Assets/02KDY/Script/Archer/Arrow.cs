using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;           // 투사체 속도
    public float lifetime = 5f;         // 투사체의 생명 시간 (초)
    private Vector3 targetPosition;     // 목표 위치 (마우스 클릭 위치)

    // 투사체를 초기화하는 함수
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

        // 투사체가 목표 위치를 향하도록 회전
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        // 목표 위치로 투사체 이동
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 투사체가 일정 시간이 지나면 파괴
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
