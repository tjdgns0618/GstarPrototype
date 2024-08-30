using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour, IDamageAble<float>
{
    public float speed = 5f;
    Rigidbody characterRigidbody;
    Animator animator;

    private void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Damage(float damageTaken)
    {


    }

    public void Dead()
    {
        
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical"); 

        Vector3 velocity = new Vector3(inputX, 0, inputZ);
        // animator.SetFloat();
        velocity *= speed;
        characterRigidbody.velocity = velocity;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

        // Atan2를 이용하면 높이와 밑변(tan)으로 라디안(Radian)을 구할 수 있음
        // Mathf.Rad2Deg를 곱해서 라디안(Radian)값을 도수법(Degree)으로 변환
        float angle = Mathf.Atan2(
            this.transform.position.y - mouseWorldPosition.y,
            this.transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;

        // angle이 0~180의 각도라서 보정
        float final = -(angle + 90f);


        // Y축 회전
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, final, 0f));


    }

}
