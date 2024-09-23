using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.VFX;
using CharacterController;
using System.Data;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : MonoBehaviour, IDamageAble<float>
{
    public PlayerCharacter player { get; private set; }
    public Vector3 direction { get; private set; }  // 키보드 입력 방향
    public Vector2 mousePosition { get; private set; }  // 입력받은 마우스 방향
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

    [Header("대시 옵션")]
    [SerializeField, Tooltip("대쉬의 힘을 나타내는 값")]
    protected float dashPower;
    [SerializeField, Tooltip("대시 모션 시간")]
    protected float dashAnimTime;
    [SerializeField, Tooltip("대시 시작 후, 재입력 받을 수 있는 시간")]
    protected float dashReInputTime;
    [SerializeField, Tooltip("대시 후, 경직 시간")]
    protected float dashTetanyTime;
    [SerializeField, Tooltip("대시 재사용 대기시간")]
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
        GetMousePosition();
    }

    public void Damage(float damageTaken)
    { 
        player.animator.ResetTrigger("hit");
        player.animator.SetTrigger("hit");
        player.OnUpdateStat(player.MaxHp, player.CurrentHp - damageTaken, player.MoveSpeed, player.DashCount);

        //Debug.Log("남은 체력 = " + player.CurrentHp);
    }
    
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        player.stateMachine.ChangeState(StateName.MOVE);
        Vector3 input = context.ReadValue<Vector3>();
        direction = new Vector3(input.x, 0f, input.z);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)
            {

            }
            else if(context.interaction is PressInteraction)
            {
                bool isAvailableAttack = !AttackState.IsAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

                if (isAvailableAttack)
                {
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int dashCount = player.DashCount;
            bool isAvailableDash = playerState != PlayerState.DASH && currentDashCount < dashCount;

            if (isAvailableDash)
            {
                player.stateMachine.ChangeState(StateName.DASH);
            }
        }
    }

    public void Dash()
    {
        Debug.Log("Dashing");
        currentDashCount++;

        if (dashCoroutine != null && dashCoolTimeCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            StopCoroutine(dashCoolTimeCoroutine);
        }

        dashCoroutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 dashDirection = direction;
        int dashCount = player.DashCount;

        player.animator.SetFloat("moveSpeed", 0f);
        player.animator.SetBool("IsDashing", true);
        player.animator.SetTrigger("Dash");
        player.rigidbody.velocity = transform.forward * dashPower;

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
        //Vector3 mouseWorldPosition =
        //    Camera.main.ScreenToWorldPoint(
        //        new Vector3(
        //        mousePosition.x,
        //        mousePosition.y,
        //        Camera.main.transform.position.y));

        //// 캐릭터와 마우스 사이의 방향 계산
        //Vector3 direction = mouseWorldPosition - transform.position;
        //direction.y = 0f;  // 캐릭터의 Y축은 변경하지 않음 (수평 회전만 적용)
        //                   // 마우스 방향으로의 회전 계산

        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //player.targetRotation = targetRotation;
        //transform.rotation = targetRotation;

        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        Vector3 direction = mouseWorldPosition - (transform.position - new Vector3(0f, 0f, 2.8f));
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
    
    public void Attack()
    {

    }

    public void Shot()
    {

    }
}
