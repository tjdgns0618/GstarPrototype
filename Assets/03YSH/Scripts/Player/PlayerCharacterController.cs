using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.VFX;
using CharacterController;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : MonoBehaviour, IDamageAble<float>
{
    public PlayerCharacter player { get; private set; }
    public Vector3 direction { get; private set; }  // Ű���� �Է� ����
    public Vector2 mousePosition { get; private set; }  // �Է¹��� ���콺 ����
    public Vector3 calculatedDirection { get; private set; }
    PlayerAttack playerAttack;

    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform shotPosition;
    [SerializeField]
    VisualEffect slash;
    [SerializeField]
    Camera cam;

    public enum PlayerState
    {
        MOVE,
        DASH,
        NDASH,
    }
    protected PlayerState playerState;

    [Header("��� �ɼ�")]
    [SerializeField, Tooltip("�뽬�� ���� ��Ÿ���� ��")]
    protected float dashPower;
    [SerializeField, Tooltip("��� ��� �ð�")]
    protected float dashAnimTime;
    [SerializeField, Tooltip("��� ���� ��, ���Է� ���� �� �ִ� �ð�")]
    protected float dashReInputTime;
    [SerializeField, Tooltip("��� ��, ���� �ð�")]
    protected float dashTetanyTime;
    [SerializeField, Tooltip("��� ���� ���ð�")]
    protected float dashCoolTime;

    private WaitForSeconds DASH_ANIM_TIME;
    private WaitForSeconds DASH_RE_INPUT_TIME;
    private WaitForSeconds DASH_TETANY_TIME;
    private Coroutine dashCoroutine;
    private Coroutine dashCoolTimeCoroutine;
    private int currentDashCount;



    private void Start()
    {
        player = GetComponent<PlayerCharacter>();

        DASH_ANIM_TIME = new WaitForSeconds(dashAnimTime);
        DASH_RE_INPUT_TIME = new WaitForSeconds(dashReInputTime);
        DASH_TETANY_TIME = new WaitForSeconds(dashTetanyTime);
    }

    private void Update()
    {
        // Move();
        GetMousePosition();
    }

    public void Damage(float damageTaken)
    {
        float currentHp = player.CurrentHp;
        player.animator.SetTrigger("hit");
        currentHp -= damageTaken;
        
        // Debug.Log("���� ü�� = " + player.hp);
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

    

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int dashCount = player.DashCount;
            bool isAvailableDash =
            playerState != PlayerState.DASH && currentDashCount < dashCount;

            if (isAvailableDash)
            {
                playerState = PlayerState.DASH;
                currentDashCount++;

                if(dashCoroutine != null && dashCoolTimeCoroutine != null)
                {
                    StopCoroutine(dashCoroutine);
                    StopCoroutine(dashCoolTimeCoroutine);
                }

                dashCoroutine = StartCoroutine(DashCoroutine());
            }
        }
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 dashDirection = direction;
        int dashCount = player.DashCount;

        player.animator.SetFloat("moveSpeed", 0f);
        player.animator.SetBool("IsDashing", true);
        player.animator.SetTrigger("Dash");
        player.rigidbody.velocity = dashDirection * dashPower;

        yield return DASH_ANIM_TIME;
        playerState = (dashCount > 1 && currentDashCount < dashCount) ? PlayerState.NDASH : PlayerState.DASH;

        yield return DASH_RE_INPUT_TIME;
        player.animator.SetBool("IsDashing", false);
        player.rigidbody.velocity = Vector3.zero;

        yield return DASH_TETANY_TIME;
        playerState = PlayerState.MOVE;

        dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());
    }

    private IEnumerator DashCoolTimeCoroutine()
    {
        float currentTime = 0f;
        int dashCount = player.DashCount;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dashCoolTime)
                break;
            yield return null;
        }

        if (currentDashCount == dashCount)
            currentDashCount = 0;
    }

    public void Dead()
    {

    }    
    
    
    void GetMousePosition()
    {
        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                mousePosition.x,
                mousePosition.y,
                Camera.main.transform.position.y));

        // ĳ���Ϳ� ���콺 ������ ���� ���
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.y = 0f;  // ĳ������ Y���� �������� ���� (���� ȸ���� ����)
                           // ���콺 ���������� ȸ�� ���

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public void AttackStateCollider()
    {
        playerAttack.gameObject.GetComponent<BoxCollider>().enabled =
            !playerAttack.gameObject.GetComponent<BoxCollider>().enabled;
    }

    public void Attack()
    {
        throw new NotImplementedException();
    }

    public void Shot()
    {
        throw new NotImplementedException();
    }

    public void Move()
    {
        throw new NotImplementedException();
    }
}
