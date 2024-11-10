using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boomerang : MonoBehaviour
{
    public float throwDistance = 70f;  // 부메랑이 날아갈 거리
    public float speed = 5f;  // 부메랑의 이동 속도
    private Vector3 startPosition;  // 부메랑 시작 위치
    private Vector3 targetPosition;  // 목표 지점 (일정 거리)
    private bool returning = false;  // 부메랑이 돌아오는지 여부
    private Transform player;  // 플레이어의 위치

    void Start()
    {
        player = PlayerCharacter.Instance.transform;  // 플레이어 위치 설정
        startPosition = player.position;  // 부메랑 시작 위치는 플레이어 위치
        // 목표 지점 계산 (플레이어 기준으로 일정 거리만큼 앞에 위치)
        targetPosition = player.position + (transform.position - player.position)
                        .normalized * throwDistance;
        targetPosition.y = startPosition.y;  // y 값 고정
    }

    void Update()
    {
        if (!returning)
        {
            MoveTowardsTarget(targetPosition);  // 부메랑이 목표 지점으로 날아감
        }
        else
        {
            MoveTowardsTarget(startPosition);  // 부메랑이 원래 위치로 돌아옴
        }
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        // 목표 지점까지 부메랑 이동
        Vector3 targetWithFixedY = target;
        targetWithFixedY.y = transform.position.y;  // y 값 고정

        transform.position = Vector3.MoveTowards(transform.position, targetWithFixedY, speed * Time.deltaTime);

        // 목표 지점에 도달하면, 부메랑이 돌아오기 시작
        if (Vector3.Distance(transform.position, targetWithFixedY) < 0.1f)
        {
            returning = !returning;  // 돌아오는 방향으로 전환
            targetPosition = returning ? startPosition : player.position + (transform.position - player.position).normalized * throwDistance;  // 새로운 목표 설정
            targetPosition.y = startPosition.y;  // y 값 고정
        }
    }
}
