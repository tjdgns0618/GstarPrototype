using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniping : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Camera mainCamera = Camera.main;

        Ray cemeraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        // 오브젝트의 현재 위치를 가져옵니다.
        Vector3 currentPosition = transform.position;

        // 카메라의 방향을 가져옵니다.
        Vector3 directionToCamera = (mainCamera.transform.position - currentPosition).normalized;


        if (groundPlane.Raycast(cemeraRay, out rayLength))
        {
            Vector3 pointToLook = cemeraRay.GetPoint(rayLength);
            Debug.DrawLine(cemeraRay.origin, pointToLook, Color.blue);

            transform.position = (new Vector3(pointToLook.x, transform.position.y, pointToLook.z) + (directionToCamera * 5));
        }
    }
}

