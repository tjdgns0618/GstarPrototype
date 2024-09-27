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
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : MonoBehaviour, IDamageAble<float>
{
    public PlayerCharacter player { get; private set; }
    public Vector3 direction { get; private set; }  // 키보드 입력 방향
    public Vector2 mousePosition { get; private set; }  // 입력받은 마우스 방향
    public Vector3 calculatedDirection { get; private set; }
    PlayerAttack playerAttack;

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
    public static bool canMove = true;

    private void Start()
    {
        player = GetComponent<PlayerCharacter>();
        hasMoveAnimation = Animator.StringToHash("moveSpeed");

        DASH_ANIM_TIME = new WaitForSeconds(dashAnimTime);
        DASH_RE_INPUT_TIME = new WaitForSeconds(dashReInputTime);
        DASH_TETANY_TIME = new WaitForSeconds(dashTetanyTime);
    }

    private void Update()
    {
        GetMousePosition();
        Move();
        // Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        
    }

    public void Damage(float damageTaken)
    { 
        player.animator.ResetTrigger("hit");
        player.animator.SetTrigger("hit");
        player.OnUpdateStat(player.MaxHp, player.CurrentHp - damageTaken, player.MoveSpeed, player.DashCount);
    }
    
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // player.stateMachine.ChangeState(StateName.MOVE);
        Vector3 input = context.ReadValue<Vector3>();
        direction = new Vector3(input.x, 0f, input.z);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
    float timer;
    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {        
        if (context.performed)
        {         
            if (context.interaction is HoldInteraction)
            {
                bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

                if (isAvailableAttack)
                {
                    AttackState.IsBaseAttack = true;
                    AttackState.isHolding = true;
                    Debug.Log("HoldInteraction AttackState");
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
            else if (context.interaction is PressInteraction)
            {
                Debug.Log(AttackState.isHolding);
                bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

                if (isAvailableAttack)
                {
                    AttackState.IsBaseAttack = true;
                    AttackState.isHolding = false;
                    Debug.Log("PressInteraction AttackState");

                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
        if (context.canceled)
            AttackState.isHolding = false;
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
    public void OnClickQ(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack)
        {
            if (context.interaction is PressInteraction)
            {
                bool isAvailableSkill = !AttackState.IsSkill_Q;
                // 스킬 쿨타임 다 찼을때 isAvailableSkill true로 초기화

                if (isAvailableSkill)
                {
                    AttackState.IsSkill_Q = true;
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
        
    }
    public void OnClickE(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isAvailableAttack = !AttackState.IsSkill_E;

            if (isAvailableAttack)
            {
                AttackState.IsSkill_E = true;
                player.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }
    public void OnClickR(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isAvailableAttack = !AttackState.IsSkill_R;

            if (isAvailableAttack)
            {
                AttackState.IsSkill_R = true;
                player.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }

    public const float CONVERT_UNIT_VALUE = 0.01f;
    public const float DEFAULT_CONVERT_MOVESPEED = 3f;
    public const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
    private int hasMoveAnimation;


    protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
    {
        if (direction == Vector3.zero)
        {
            return -DEFAULT_ANIMATION_PLAYSPEED;
        }

        return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.5f;
    }

    public void Move()
    {
        if (!canMove)
            return;

        #region #캐릭터 움직임 구현
        float curretnMoveSpeed = player.MoveSpeed * CONVERT_UNIT_VALUE;
        float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED *
                                    GetAnimationSyncWithMovement(curretnMoveSpeed);

        PlayerCharacter.Instance.rigidbody.velocity =
            direction * curretnMoveSpeed +
            Vector3.up * PlayerCharacter.Instance.rigidbody.velocity.y;

        if (animationPlaySpeed < 0f) animationPlaySpeed = 0f;

        PlayerCharacter.Instance.animator.SetFloat("moveSpeed", animationPlaySpeed);
        #endregion
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
        player.stateMachine.ChangeState(StateName.MOVE);

        dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());
    }

    private IEnumerator DashCoolTimeCoroutine()
    {
        float currentTime = 0f;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dashCoolTime)
                break;
            yield return null;
        }

        if (currentDashCount == player.DashCount)
            currentDashCount = 0;
    }

    public void Dead()
    {

    }
    
    void GetMousePosition()
    {
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
