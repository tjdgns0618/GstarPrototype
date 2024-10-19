using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_09 : MonoBehaviour
{
    public int prevCount = 3;
    public int totalCount => shields.Count;
    public int itemID;
    float rotationSpeed = 50f;
    float radius = 1.8f;

    public List<GameObject> shields;
    public List<Transform> _shields;

    private void Start()
    {
        _shields = new List<Transform>(_shields.Count);
        for (int i = 0; i < _shields.Count; i++)
        {
            _shields.Add(shields[i].transform); // List�� �߰�
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Batch();
        }

        if (_shields != null && _shields.Count > 0)
        {
            ShieldsTurn();
        }
    }

    public void UseItem()
    {
        Batch();
        if (_shields != null && _shields.Count > 0)
        {
            ShieldsTurn();
        }
    }

    void Batch()
    {
        //DeactiveShields();
        _shields.Clear(); // ���� ����Ʈ �ʱ�ȭ

        for (int i = 0; i < totalCount; i++) // totalCount ���
        {
            Transform shield = ItemPoolManager.instance.Get(0).transform;
            shield.parent = transform;
            _shields.Add(shield); // List�� �߰�
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
                float angle = 360f / _shields.Count * i + Time.time * rotationSpeed; // ȸ�� ����
                Vector3 rotvec = Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0.5f, 0);
                _shields[i].position = playerPosition + rotvec; // ���� ��ġ ������Ʈ

                // ���а� �÷��̾ �ٶ󺸵��� ȸ�� ����
                Vector3 direction = playerPosition - _shields[i].position; // �÷��̾� ����
                Quaternion rotation = Quaternion.LookRotation(-direction); // �ǵ��� ������ �÷��̾ �ݴ�� ���� ����
                _shields[i].rotation = rotation; // ���� ȸ�� ����
            }
        }
    }

    void DeactiveShields()
    {
        if (_shields != null)
        {
            for (int i = 0; i < _shields.Count; i++)
            {
                if (_shields[i] != null)
                {
                    _shields[i].gameObject.SetActive(false); // ���� ��Ȱ��ȭ
                }
            }
        }
    }
}
