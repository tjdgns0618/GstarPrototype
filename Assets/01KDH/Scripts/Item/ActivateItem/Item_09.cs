using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_09 : MonoBehaviour
{
    public int prevCount = 3;
    public int totalCount = 2;
    public int itemID;
    float rotationSpeed = 50f;
    float radius = 1.8f;


    public Transform[] shields;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            totalCount++;
            Batch();
        }

        if (shields != null && shields.Length > 0)
        {
            ShieldsTurn();
        }
    }

    public void UseItem()
    {
        totalCount++;
        Batch();
        if (shields != null && shields.Length > 0)
        {
            ShieldsTurn();
        }
    }

    void Batch()
    {   
        DeactiveShields();
        shields = new Transform[totalCount];

        for (int i = 0; i < totalCount; i++)
        {
            Transform shield = ItemPoolManager.instance.Get(0).transform;
            shield.parent = transform;
            shields[i] = shield;
        }
    }

    void ShieldsTurn()
    {
        Vector3 playerPosition = PlayerCharacter.Instance.transform.position; // 플레이어 위치

        for (int i = 0; i < totalCount; i++)
        {
            if (shields[i] != null)
            {
                // 방패의 회전 각도 계산
                float angle = 360f / totalCount * i + Time.time * rotationSpeed; // 회전 각도
                Vector3 rotvec = Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0.5f, 0);
                shields[i].position = playerPosition + rotvec; // 방패 위치 업데이트

                // 방패가 플레이어를 바라보도록 회전 설정
                Vector3 direction = playerPosition - shields[i].position; // 플레이어 방향
                Quaternion rotation = Quaternion.LookRotation(-direction); // 실드의 방향은 플레이어를 반대로 보는 방향
                shields[i].rotation = rotation; // 방패 회전 설정
            }
        }
    }

    void DeactiveShields()
    {
        if (shields != null)
        {
            for (int i = 0; i < shields.Length; i++)
            {
                if (shields[i] != null)
                {
                    shields[i].gameObject.SetActive(false); // 방패 비활성화
                }
            }
        }
    }
}
