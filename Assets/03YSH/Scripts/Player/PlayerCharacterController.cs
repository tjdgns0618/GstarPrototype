using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterController : MonoBehaviour, IDamageAble<float>
{
    Rigidbody characterRigidbody;
    Animator animator;
    PlayerAttack playerAttack;
    PlayerCharacter player;
    public Vector3 direction { get; private set; }
    public Vector2 mousePosition { get; private set; }

    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform shotPosition;
    [SerializeField]
    VisualEffect slash;
    [SerializeField]
    Camera cam;

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
        GetMousePosition();
        // if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsName("attack01"))
        //    Attack();
        // if (Input.GetKeyDown(KeyCode.Q) && !animator.GetCurrentAnimatorStateInfo(1).IsName("shot"))
        //    animator.SetTrigger("shot");
    }

    public void Damage(float damageTaken)
    {
        animator.SetTrigger("hit");
        player.hp -= damageTaken;
        // Debug.Log("남은 체력 = " + player.hp);
    }

    public void Shot()
    {
        Fire();
    }

    public void Fire()
    {
        bullet.GetComponent<Bullet>().targetname = "Enemy";
        GameObject temp = Instantiate(bullet, shotPosition.position, Quaternion.identity);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector3>();
        direction = new Vector3(input.x, 0f, input.z);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        Attack();
    }

    public void Dead()
    {

    }

    public void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("attack01"))
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("attack");
            slash.Play();
        }
    }

    public void Move()
    {
        animator.SetFloat("moveSpeed", direction.magnitude);

        characterRigidbody.velocity = direction * player.speed;        
    }
    
    void GetMousePosition()
    {
        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                mousePosition.x,
                mousePosition.y,
                Camera.main.transform.position.y));

        // 캐릭터와 마우스 사이의 방향 계산
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.y = 0f;  // 캐릭터의 Y축은 변경하지 않음 (수평 회전만 적용)
                           // 마우스 방향으로의 회전 계산

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public void AttackStateCollider()
    {
        playerAttack.gameObject.GetComponent<BoxCollider>().enabled =
            !playerAttack.gameObject.GetComponent<BoxCollider>().enabled;
    }


}
