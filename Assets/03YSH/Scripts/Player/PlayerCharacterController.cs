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
using static UnityEngine.UI.CanvasScaler;
using UnityEditor.Animations;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : MonoBehaviour, IDamageAble<float>
{
    public PlayerCharacter player { get; private set; }
    public Vector3 direction { get; private set; }  // 키보드 입력 방향
    public Vector2 mousePosition { get; private set; }  // 입력받은 마우스 방향
    public Vector3 calculatedDirection { get; private set; }
    PlayerAttack playerAttack;
    BaseState dashState;

    public enum PlayerState
    {
        MOVE,
        DASH,
        NDASH,
    }
    protected PlayerState playerState;

    //[Header("캐릭터 애니메이터")]
    //[SerializeField, Tooltip("전사 애니메이터")]
    //protected RuntimeAnimatorController warriorAnimController;
    //[SerializeField, Tooltip("궁수 애니메이터")]
    //protected RuntimeAnimatorController archerAnimController;
    //[SerializeField, Tooltip("마법사 애니메이터")]
    //protected RuntimeAnimatorController wizardAnimController;
    //[Header("캐릭터 메쉬")]
    //[SerializeField, Tooltip("전사 메쉬")]
    //protected Mesh warriorMesh;
    //[SerializeField, Tooltip("궁수 메쉬")]
    //protected Mesh archerMesh;
    //[SerializeField, Tooltip("마법사 메쉬")]
    //protected Mesh wizardMesh;
    //[Header("캐릭터 무기")]
    //[SerializeField, Tooltip("전사 무기")]
    //protected PlayerAttack warriorWeapon;
    //[SerializeField, Tooltip("궁수 무기")]
    //protected PlayerAttack archerWeapon;
    //[SerializeField, Tooltip("마법사 무기")]
    //protected PlayerAttack wizardWeapon;

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
    PlayerCharacter pi;

    private void Start()
    {
        player = GetComponent<PlayerCharacter>();
        pi = PlayerCharacter.Instance;
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


    /*public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)
            {
                bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

                Debug.Log("true면 공격 가능 : " + isAvailableAttack);

                if (isAvailableAttack)
                {
                    AttackState.IsBaseAttack = true;
                    AttackState.isHolding = true;
                    AttackState.isClick = false;
                    Debug.Log("HoldInteraction AttackState");
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
            else if (context.interaction is PressInteraction && !AttackState.isHolding)
            {

                bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

                Debug.Log(AttackState.IsBaseAttack + "true 이면 공격 불가능");

                if (isAvailableAttack)
                {
                    AttackState.IsBaseAttack = true;
                    AttackState.isHolding = false;
                    AttackState.isClick = true;
                    Debug.Log("PressInteraction AttackState");

                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
        else if (context.canceled)
        {
            AttackState.isHolding = false;
        }
    }*/

    public void OnCharacterChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            /*
            if (context.control.name == "1")
            {
                Debug.Log("버튼 1");

                // player.weaponManager.SetWeapon(warriorWeapon.gameObject);
                player.animator.runtimeAnimatorController = warriorAnimController;
                player.characterClass = CharacterType.Warrior;
                player.skinnedMeshRenderer.sharedMesh = warriorMesh;
                // ItemManager.Instance.equipWeapon = warriorWeapon.gameObject;
                warriorWeapon.gameObject.SetActive(true);
                archerWeapon.gameObject.SetActive(false);
                wizardWeapon.gameObject.SetActive(false);
                player.animEvent.effect = warriorWeapon;
                player.weaponManager.Weapon.ComboCount = 0;
            }
            else if (context.control.name == "2")
            {
                Debug.Log("버튼 2");
                // player.weaponManager.SetWeapon(archerWeapon.gameObject);
                player.animator.runtimeAnimatorController = archerAnimController;
                player.characterClass = CharacterType.Archer;
                player.skinnedMeshRenderer.sharedMesh = archerMesh;
                // ItemManager.Instance.equipWeapon = archerWeapon.gameObject;
                warriorWeapon.gameObject.SetActive(false);
                archerWeapon.gameObject.SetActive(true);
                wizardWeapon.gameObject.SetActive(false);
                player.animEvent.effect = archerWeapon;
                player.weaponManager.Weapon.ComboCount = 0;


            }
            else if (context.control.name == "3") {
                Debug.Log("버튼 3");
                // player.weaponManager.SetWeapon(wizardWeapon.gameObject);
                player.animator.runtimeAnimatorController = wizardAnimController;
                player.characterClass = CharacterType.Wizard;
                player.skinnedMeshRenderer.sharedMesh = wizardMesh; 
                // ItemManager.Instance.equipWeapon = wizardWeapon.gameObject;
                warriorWeapon.gameObject.SetActive(false);
                archerWeapon.gameObject.SetActive(false);
                wizardWeapon.gameObject.SetActive(true);
                player.animEvent.effect = wizardWeapon;
                player.weaponManager.Weapon.ComboCount = 0;
            }
            */
            if (context.control.name == "1")
            {

            }
        }
    }

    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnClickLeftMouse");
            HandlePerformedInteraction(context);
        }
        else if (context.canceled)
        {
            AttackState.isHolding = false;
        }
    }

    private void HandlePerformedInteraction(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            Debug.Log("Context HoldInteraction");
            HandleHoldInteraction();
        }
        else if (context.interaction is PressInteraction && !AttackState.isHolding)
        {
            HandlePressInteraction();
        }
    }

    private void HandleHoldInteraction()
    {
        bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

        if (isAvailableAttack)
        {
            AttackState.IsBaseAttack = true;
            AttackState.isHolding = true;
            AttackState.canAttack = true;
            Debug.Log("HoldInteraction AttackState");
            player.stateMachine.ChangeState(StateName.ATTACK);
        }
        /*bool isAvailableAttack = !AttackState.IsBaseAttack &&
        //                         (player.weaponManager.Weapon.ComboCount < 3);

        //Debug.Log("true면 공격 가능 : " + isAvailableAttack);

        //if (isAvailableAttack)
        //{
        //    AttackState.IsBaseAttack = true;
        //    AttackState.isHolding = true;
        //    AttackState.isClick = false;
        //    Debug.Log("HoldInteraction AttackState");
        //    player.stateMachine.ChangeState(StateName.ATTACK);
        }*/
    }

    private void HandlePressInteraction()
    {
        bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

        if (isAvailableAttack)
        {
            AttackState.IsBaseAttack = true;
            Debug.Log("PressInteraction AttackState");

            player.stateMachine.ChangeState(StateName.ATTACK);
        }
        /*bool isAvailableAttack = !AttackState.IsBaseAttack &&
        //                         (player.weaponManager.Weapon.ComboCount < 3);

        //Debug.Log(AttackState.IsBaseAttack + "true 이면 공격 불가능");

        //if (isAvailableAttack)
        //{
        //    AttackState.IsBaseAttack = true;
        //    AttackState.isHolding = false;
        //    AttackState.isClick = true;
        //    Debug.Log("PressInteraction AttackState");
        //    player.stateMachine.ChangeState(StateName.ATTACK);
        }*/
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (DashState.CurrentDashCount >= player.DashCount)
                return;

            if (!DashState.IsDash)
            {
                Debug.Log("Dash Input");
                DashState.CurrentDashCount++;
                dashState = player.stateMachine.GetState(StateName.DASH);
                dashState.Init(dashPower, dashTetanyTime, dashCoolTime);
                player.stateMachine.ChangeState(StateName.DASH);
                canMove = false;
            }

            //int dashCount = player.DashCount;
            //bool isAvailableDash = playerState != PlayerState.DASH && currentDashCount < dashCount;
            //Debug.Log(playerState != PlayerState.DASH);
            //Debug.Log(playerState);
            //if (isAvailableDash)
            //{
            //    player.stateMachine.ChangeState(StateName.DASH);
            //}
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

        pi.rigidbody.velocity =
            direction * curretnMoveSpeed +
            Vector3.up * pi.rigidbody.velocity.y;

        if (animationPlaySpeed < 0f) animationPlaySpeed = 0f;

        pi.animator.SetFloat("moveSpeed", animationPlaySpeed);
        #endregion
    }

    public void OnFinishedDash()
    {
        if (DashState.IsDash && DashState.CurrentDashCount < player.DashCount)
        {
            // OnFinishedDash하고 첫번째 스테이트 변경
            pi.stateMachine.ChangeState(StateName.DASH);
            return;
        }
        dashState = player.stateMachine.GetState(StateName.DASH);
        dashState.OnExitState();
        canMove = true;
        AttackState.IsBaseAttack = false;

        if (dashCoolTimeCoroutine != null)
            StopCoroutine(dashCoolTimeCoroutine);
        dashCoolTimeCoroutine =
            StartCoroutine(CheckDashReInputLimitTime(DashState.dashCooltime));
    }

    private IEnumerator CheckDashReInputLimitTime(float limitTime)
    {
        float timer = 0f;

        while (true) {
            timer += Time.deltaTime;

            if (timer > limitTime)
            {
                DashState.IsDash = false;
                DashState.CurrentDashCount = 0;
                player.animator.ResetTrigger(DashState.Hash_DashTrigger);
                pi.stateMachine.ChangeState(StateName.MOVE);
                break;
            }
            yield return null;
        }
    }
    #region 대쉬 구현
    //public void Dash()
    //{
    //    currentDashCount++;

    //    if (dashCoroutine != null && dashCoolTimeCoroutine != null)
    //    {
    //        StopCoroutine(dashCoroutine);
    //        StopCoroutine(dashCoolTimeCoroutine);
    //    }

    //    dashCoroutine = StartCoroutine(DashCoroutine());
    //}

    //private IEnumerator DashCoroutine()
    //{
    //    int dashCount = player.DashCount;

    //    player.animator.SetFloat("moveSpeed", 0f);
    //    player.animator.SetBool("IsDashing", true);
    //    player.animator.SetTrigger("Dash");
    //    player.rigidbody.velocity = transform.forward * dashPower;

    //    yield return DASH_ANIM_TIME;
    //    playerState = (dashCount > 1 && currentDashCount < dashCount) ? PlayerState.NDASH : PlayerState.DASH;

    //    yield return DASH_RE_INPUT_TIME;
    //    player.animator.SetBool("IsDashing", false);
    //    player.rigidbody.velocity = Vector3.zero;

    //    yield return DASH_TETANY_TIME;
    //    player.stateMachine.ChangeState(StateName.MOVE);

    //    dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());
    //}

    //private IEnumerator DashCoolTimeCoroutine()
    //{
    //    float currentTime = 0f;

    //    while (true)
    //    {
    //        currentTime += Time.deltaTime;
    //        if (currentTime >= dashCoolTime)
    //        {
    //            player.stateMachine.ChangeState(StateName.MOVE);
    //            break;
    //        }
    //        yield return null;
    //    }

    //    if (currentDashCount == player.DashCount)
    //        currentDashCount = 0;
    //}
    #endregion

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
