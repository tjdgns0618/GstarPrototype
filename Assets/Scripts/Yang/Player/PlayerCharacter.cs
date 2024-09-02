using System;
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
    float hp;
    Rigidbody characterRigidbody;
    Animator animator;
    PlayerAttack playerAttack;

    [SerializeField]
    GameObject bullet;

    private void Awake()
    {
        playerAttack = FindAnyObjectByType<PlayerAttack>();
        playerAttack.Click += new EventHandler(Attack);

        hp = 50f;
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        if (Input.GetMouseButtonDown(0))
        {
            playerAttack.MouseButtonDown();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shot();
        }
    }

    public void Damage(float damageTaken)
    {
        animator.SetTrigger("hit");
        hp -= damageTaken;
        Debug.Log("남은 체력 = " + hp);
    }

    public void Shot()
    {
        GameObject temp = Instantiate(bullet, Vector3.zero, Quaternion.identity);
        animator.SetTrigger("shot");
    }

    public void Dead()
    {
        
    }

    public void Attack(object sender, EventArgs e)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack01"))
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("attack");
        }
    }
    public void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical"); 

        Vector3 velocity = new Vector3(inputX, 0, inputZ);

        if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            animator.SetLayerWeight(1, 0);
        }
        animator.SetFloat("moveSpeed", velocity.magnitude);
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

    public void AttackStateCollider()
    {
        playerAttack.gameObject.GetComponent<MeshCollider>().enabled =
            !playerAttack.gameObject.GetComponent<MeshCollider>().enabled;
    }

    
}
