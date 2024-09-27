using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    public GameObject ArrowPrefab;
    public Transform firePoint;
    public float ArrowSpeed = 10;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        // ���콺 �������� �����ͼ� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;  // ī�޶��� ����� Ŭ�� �÷��ο��� Z�� ����
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // �߻� ��ġ���� ���콺 ��ġ���� ���� ���
        Vector3 direction = (worldMousePosition - firePoint.position).normalized;

        // ����ü ����
        GameObject projectile = Instantiate(ArrowPrefab, firePoint.position, Quaternion.identity);

        // ����ü�� ��ǥ ��ġ ����
        Arrow projectileScript = projectile.GetComponent<Arrow>();
        if (projectileScript != null)
        {
            projectileScript.SetTargetPosition(worldMousePosition);
        }

    }
}
