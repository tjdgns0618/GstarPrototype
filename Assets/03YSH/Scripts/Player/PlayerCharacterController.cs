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
using UnityEngine.InputSystem.XR;
using UnityEditor.Animations;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;

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
    private Coroutine dashCoroutine;
    private Coroutine dashCoolTimeCoroutine;
    private int currentDashCount;
    public static bool canMove = true;
    PlayerCharacter pi;
    GameManager gameManager;
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashIsChangeAnimation = Animator.StringToHash("change");

    private void Start()
    {
        gameManager = GameManager.instance;

        player = GetComponent<PlayerCharacter>();
        pi = PlayerCharacter.Instance;
        hasMoveAnimation = Animator.StringToHash("moveSpeed");

        DASH_ANIM_TIME = new WaitForSeconds(dashAnimTime);    

        AttackState.CanReInputTime = GameManager.instance._reInputTime;
    }       

    private void Update()
    {
        if (gameManager.isDead || gameManager.isPause)
            return;
        GetMousePosition();
        Move();
    }

    public void Damage(float damageTaken)
    {
        if (gameManager.isDead || gameManager.isHit)
            return;
        gameManager.isHit = true;
        player.animator.ResetTrigger("hit");
        player.animator.SetTrigger("hit");
        player.OnUpdateStat(player.MaxHp, player.CurrentHp - damageTaken, player.MoveSpeed, player.DashCount);
        if(player.CurrentHp <= 0)
        {
            Dead();
        }
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

    public void OnCharacterChange(InputAction.CallbackContext context)
    {
        if (context.performed && !player.isPlaySkill && player.canChange && !gameManager.isDead && !gameManager.isPause)
        {
            if (context.control.name == "1" && player.characterClass != CharacterType.Warrior)
            {
                player.weaponObjects[0].SetActive(true);
                player.weaponObjects[1].SetActive(false);
                player.weaponObjects[2].SetActive(false);
                player.characterClass = CharacterType.Warrior;
                player.animator.runtimeAnimatorController = player.classControllers[0];
                player.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = player.classMesh[0];
                AttackState.comboCount = 0;
                gameManager.uiManager.ChangeCharacterUI(0);
                AttackState.IsAttack = false;
                AttackState.IsBaseAttack = false;
                player.animator.Rebind();
                player.animator.SetTrigger(hashIsChangeAnimation);
                player.canChange = false;
            }
            if (context.control.name == "2" && player.characterClass != CharacterType.Archer)
            {
                player.weaponObjects[0].SetActive(false);
                player.weaponObjects[1].SetActive(true);
                player.weaponObjects[2].SetActive(false);
                player.characterClass = CharacterType.Archer;                
                player.animator.runtimeAnimatorController = player.classControllers[1];
                player.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = player.classMesh[1];
                AttackState.comboCount = 0;
                gameManager.uiManager.ChangeCharacterUI(1);
                AttackState.IsAttack = false;
                AttackState.IsBaseAttack = false;
                player.animator.Rebind();
                player.animator.SetTrigger(hashIsChangeAnimation);
                player.canChange = false;
            }
            if (context.control.name == "3" && player.characterClass != CharacterType.Wizard)
            {
                player.weaponObjects[0].SetActive(false);
                player.weaponObjects[1].SetActive(false);
                player.weaponObjects[2].SetActive(true);
                player.characterClass = CharacterType.Wizard;
                player.animator.runtimeAnimatorController = player.classControllers[2];
                player.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = player.classMesh[2];
                AttackState.comboCount = 0;
                gameManager.uiManager.ChangeCharacterUI(2);
                AttackState.IsAttack = false; 
                AttackState.IsBaseAttack = false;
                player.animator.Rebind();
                player.animator.SetTrigger(hashIsChangeAnimation);
                player.canChange = false;
            }
        }
    }

    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        if (gameManager.isPause || gameManager.isDead)
            return;

        if (context.performed && !player.isPlaySkill)
        {
            // Debug.Log("OnClickLeftMouse");
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
            // Debug.Log("Context HoldInteraction");
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
            // Debug.Log("HoldInteraction AttackState");
            player.stateMachine.ChangeState(StateName.ATTACK);
        }
    }

    private void HandlePressInteraction()
    {
        bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3);

        if (isAvailableAttack)
        {
            AttackState.IsBaseAttack = true;
            // Debug.Log("PressInteraction AttackState");

            player.stateMachine.ChangeState(StateName.ATTACK);
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.isPause && !gameManager.isDead && DashState.CurrentDashCount == 0)
        {
            if (!DashState.IsDash)
            {
                Debug.Log("Dash Input");
                DashState.CurrentDashCount++;
                dashState = player.stateMachine.GetState(StateName.DASH);
                dashState.Init(dashPower, dashCoolTime);
                player.stateMachine.ChangeState(StateName.DASH);
                canMove = false;
            }
        }
    }
   

    public void OnClickQ(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack && !player.isPlaySkill && !gameManager.isPause || gameManager.isDead)
        {
            if (context.interaction is PressInteraction)
            {
                bool isAvailableSkill = !AttackState.IsSkill_Q;
                // 스킬 쿨타임 다 찼을때 isAvailableSkill true로 초기화

                if (isAvailableSkill)
                {
                    AttackState.IsSkill_Q = true;
                    player.isPlaySkill = true;
                    ResetAnimator();
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
    }
    public void OnClickE(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack && !player.isPlaySkill && !gameManager.isPause || gameManager.isDead)
        {
            bool isAvailableAttack = !AttackState.IsSkill_E;

            if (isAvailableAttack)
            {
                AttackState.IsSkill_E = true;
                player.isPlaySkill = true;
                ResetAnimator();
                player.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }
    public void OnClickR(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack && !player.isPlaySkill! && !gameManager.isPause || gameManager.isDead)
        {
            bool isAvailableAttack = !AttackState.IsSkill_R;

            if (isAvailableAttack)
            {
                AttackState.IsSkill_R = true;
                player.isPlaySkill = true;
                ResetAnimator();
                player.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }

    public void ResetAnimator()
    {
        player.animator.SetBool("IsSkill_Q", false);
        player.animator.SetBool("IsSkill_E", false);
        player.animator.SetBool("IsSkill_R", false);
    }

    public const float CONVERT_UNIT_VALUE = 0.01f;
    public const float DEFAULT_CONVERT_MOVESPEED = 1f;
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
        dashState = player.stateMachine.GetState(StateName.DASH);
        dashState.OnExitState();
        canMove = true;
        player.animator.SetBool("canHit", true);

        AttackState.IsBaseAttack = false;

        StartCoroutine(DashCooltime());
    }

    public IEnumerator DashCooltime()
    {
        yield return new WaitForSeconds(dashCoolTime);
        DashState.CurrentDashCount = 0;
    }

    public void Dead()
    {
        gameManager.isDead = true;
        player.animator.SetTrigger("dead");
    }

    void GetMousePosition()
    {
        //Vector3 mouseWorldPosition =
        //    Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        //Vector3 direction = mouseWorldPosition - (transform.position - new Vector3(0f, 0f, 2.8f));
        //direction.y = 0f;

        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //transform.rotation = targetRotation;

        Ray cemeraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if(groundPlane.Raycast(cemeraRay, out rayLength))
        {
            Vector3 pointToLook = cemeraRay.GetPoint(rayLength);
            Debug.DrawLine(cemeraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    public void Attack()
    {

    }

    public void Shot()
    {

    }
}
