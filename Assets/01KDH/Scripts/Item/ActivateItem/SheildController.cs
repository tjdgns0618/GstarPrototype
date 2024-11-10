using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private float rotationSpeed = 50f; // 실드 회전 속도
    private float radius = 1.8f;       // 실드와 플레이어 간의 거리
    private bool isFirstUse = true;    // 첫 번째 아이템 사용 여부 체크
    private List<Transform> _shields;  // 실드의 Transform을 담는 리스트

    private void Start()
    {
        _shields = new List<Transform>();
    }

    private void Update()
    {
        if (_shields.Count > 0)
        {
            RotateShields(); // 실드를 회전시킴
        }
    }

    public void UseItem()
    {
        AddShield(); // 아이템을 사용할 때마다 실드 추가
    }

    private void AddShield()
    {
        int shieldCountToAdd = isFirstUse ? 3 : 1; // 첫 번째 사용 시 3개, 이후에는 1개 추가
        for (int i = 0; i < shieldCountToAdd; i++)
        {
            Transform shield = GameManager.instance.particlePoolManager.GetParticle("Shield").transform; // 오브젝트 풀에서 실드 가져오기
            shield.parent = transform; // 부모를 현재 오브젝트로 설정
            _shields.Add(shield); // 실드 Transform 리스트에 추가
        }
        isFirstUse = false; // 첫 사용 이후로 변경
    }

    private void RotateShields()
    {
        Vector3 playerPosition = PlayerCharacter.Instance.transform.position; // 플레이어 위치

        for (int i = 0; i < _shields.Count; i++)
        {
            if (_shields[i] != null)
            {
                // 방패의 회전 각도 계산
                float angle = 360f / _shields.Count * i + Time.time * rotationSpeed;
                Vector3 offset = Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0, 0);
                _shields[i].position = playerPosition + offset;

                // 실드가 플레이어를 바라보도록 회전 설정
                _shields[i].rotation = (Quaternion.LookRotation(_shields[i].position - playerPosition));
            }
        }
    }
}