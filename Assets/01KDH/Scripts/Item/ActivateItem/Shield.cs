using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Transform player;
    public int shieldCount;
    public int itemID;
    public float rotationSpeed=30f;
    public float radius = 3f;

    public Transform[] shields;

    private void Start()
    {
        Batch();
    }

    void Update()
    {
        for (int i = 0; i < shieldCount; i++)
        {
            if (shields[i] != null)
            {
                float angle = 360f / shieldCount * i + Time.time * rotationSpeed; // 각도 계산
                Vector3 rotvec = Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0, 0);
                shields[i].position = player.position + rotvec; // 자식 오브젝트의 위치 설정
            }
        }
    }

    void Batch()
    {
        shields = new Transform[shieldCount];

        for (int i = 0; i < shieldCount; i++)
        {
            Transform shield = GameManager.instance.itempools.Get(itemID).transform;
            shield.parent = transform;
            shields[i] = shield;
        }
    }
}
