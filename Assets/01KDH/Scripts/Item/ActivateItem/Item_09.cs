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
        Vector3 playerPosition = PlayerCharacter.Instance.transform.position; // �÷��̾� ��ġ

        for (int i = 0; i < totalCount; i++)
        {
            if (shields[i] != null)
            {
                // ������ ȸ�� ���� ���
                float angle = 360f / totalCount * i + Time.time * rotationSpeed; // ȸ�� ����
                Vector3 rotvec = Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0.5f, 0);
                shields[i].position = playerPosition + rotvec; // ���� ��ġ ������Ʈ

                // ���а� �÷��̾ �ٶ󺸵��� ȸ�� ����
                Vector3 direction = playerPosition - shields[i].position; // �÷��̾� ����
                Quaternion rotation = Quaternion.LookRotation(-direction); // �ǵ��� ������ �÷��̾ �ݴ�� ���� ����
                shields[i].rotation = rotation; // ���� ȸ�� ����
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
                    shields[i].gameObject.SetActive(false); // ���� ��Ȱ��ȭ
                }
            }
        }
    }
}
