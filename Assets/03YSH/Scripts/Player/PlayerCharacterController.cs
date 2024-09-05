using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterController : MonoBehaviour, IDamageAble<float>
{
    Rigidbody characterRigidbody;
    Animator animator;
    PlayerAttack playerAttack;
    PlayerCharacter player;

    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform shotPosition;
    [SerializeField]
    Transform cam;
    [SerializeField]
    VisualEffect slash;

    private void Awake()
    {
        playerAttack = FindAnyObjectByType<PlayerAttack>();
        // playerAttack.Click += new EventHandler(Attack);

        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = PlayerCharacter.Instance;
    }

    private void Update()
    {
        Move();
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsName("attack01"))
            Attack();
        if (Input.GetKeyDown(KeyCode.Q) && !animator.GetCurrentAnimatorStateInfo(1).IsName("shot"))
            animator.SetTrigger("shot");
    }

    public void Damage(float damageTaken)
    {
        animator.SetTrigger("hit");
        player.hp -= damageTaken;
        Debug.Log("���� ü�� = " + player.hp);
    }

    public void Shot()
    {
        Fire();
    }

    public void Fire()
    {
        bullet.GetComponent<Bullet>().targetname = "Enemy";
        GameObject temp = Instantiate(bullet, shotPosition.position, Quaternion.Euler(new Vector3(0f, GetMousePosition(), 0f)));
    }

    public void Dead()
    {

    }

    public void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack01"))
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("attack");
            slash.Play();
        }
    }
    public void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(inputX, 0, inputZ);

        // if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        // {
        //     animator.SetLayerWeight(1, 0);
        // }
        animator.SetFloat("moveSpeed", velocity.magnitude);
        velocity *= player.speed;
        characterRigidbody.velocity = velocity;

        // Y�� ȸ��
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, GetMousePosition(), 0f));
        // cam.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 5);
    }

    float GetMousePosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

        // Atan2�� �̿��ϸ� ���̿� �غ�(tan)���� ����(Radian)�� ���� �� ����
        // Mathf.Rad2Deg�� ���ؼ� ����(Radian)���� ������(Degree)���� ��ȯ
        float angle = Mathf.Atan2(
            this.transform.position.y - mouseWorldPosition.y,
            this.transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;

        // angle�� 0~180�� ������ ����
        float final = -(angle + 90f);

        return final;
    }

    public void AttackStateCollider()
    {
        playerAttack.gameObject.GetComponent<BoxCollider>().enabled =
            !playerAttack.gameObject.GetComponent<BoxCollider>().enabled;
    }


}
