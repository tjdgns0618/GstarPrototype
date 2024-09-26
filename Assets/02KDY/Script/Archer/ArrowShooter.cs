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
        // 마우스 포지션을 가져와서 월드 좌표로 변환
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;  // 카메라의 가까운 클립 플레인에서 Z축 설정
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 발사 위치에서 마우스 위치로의 방향 계산
        Vector3 direction = (worldMousePosition - firePoint.position).normalized;

        // 투사체 생성
        GameObject projectile = Instantiate(ArrowPrefab, firePoint.position, Quaternion.identity);

        // 투사체에 목표 위치 설정
        Arrow projectileScript = projectile.GetComponent<Arrow>();
        if (projectileScript != null)
        {
            projectileScript.SetTargetPosition(worldMousePosition);
        }

    }
}
