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
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

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

    public Volume _volume;
    Vignette _vignette;

    [SerializeField, Tooltip("대시 모션 시간")]
    protected float dashAnimTime;

    private WaitForSeconds DASH_ANIM_TIME;
    private Coroutine dashCoroutine;
    private Coroutine dashCoolTimeCoroutine;
    private int currentDashCount;
    private bool isMove = false;
    public static bool canMove = true;
    PlayerCharacter pi;
    GameManager gameManager;
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashIsChangeAnimation = Animator.StringToHash("change");

    public Image[] characterImages;
    public TextMeshProUGUI[] characterCooltimetexts;
    public GameObject[] WarriorSkillIcons;
    public GameObject[] ArcherSkillIcons;
    public GameObject[] WizardSkillIcons;

    public TextMeshProUGUI dashTimer;
    public Image dashCooltimerImage;
    float dashCoolTimer = 0;
    public static float changeCool = 0;

    private void Start()
    {
        gameManager = GameManager.instance;
        _volume.profile.TryGet(out _vignette);

        player = GetComponent<PlayerCharacter>();
        pi = PlayerCharacter.Instance;
        hasMoveAnimation = Animator.StringToHash("moveSpeed");

        DASH_ANIM_TIME = new WaitForSeconds(dashAnimTime);    

        DashState.CurrentDashCount = 0;

        AttackState.CanReInputTime = GameManager.instance._reInputTime;
    }

    private void Update()
    {
        if (gameManager.isDead || gameManager.isPause || gameManager.isOnUI)
        {
            player.rigidbody.angularVelocity = Vector3.zero;
            player.rigidbody.velocity = Vector3.zero;
            player.animator.SetFloat("moveSpeed", 0);
            return;
        }

        GetMousePosition();
        Move();
    }
    
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector3>();
        direction = new Vector3(input.x, 0f, input.z);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if(gameManager.isOnUI)
        {
            return;
        }
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnCharacterChange(InputAction.CallbackContext context)
    {
        if (context.performed && !player.isPlaySkill && player.canChange && !gameManager.isDead && !gameManager.isPause
            && !DashState.IsDash && !gameManager.isOnUI)
        {
            if (context.control.name == "1" && player.characterClass != CharacterType.Warrior)
            {
                player.weaponObjects[0].SetActive(true);
                player.weaponObjects[1].SetActive(false);
                player.weaponObjects[2].SetActive(false);
                foreach(var icon in WarriorSkillIcons)
                {
                    icon.SetActive(true);
                }
                foreach (var icon in ArcherSkillIcons)
                {
                    icon.SetActive(false);
                }
                foreach (var icon in WizardSkillIcons)
                {
                    icon.SetActive(false);
                }

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
                GameManager.instance.isHit = false;
                StartCoroutine(changeCooltime());
                StartCoroutine(realChangeCool());
                
            }
            if (context.control.name == "2" && player.characterClass != CharacterType.Archer)
            {
                player.weaponObjects[0].SetActive(false);
                player.weaponObjects[1].SetActive(true);
                player.weaponObjects[2].SetActive(false);
                foreach (var icon in WarriorSkillIcons)
                {
                    icon.SetActive(false);
                }
                foreach (var icon in ArcherSkillIcons)
                {
                    icon.SetActive(true);
                }
                foreach (var icon in WizardSkillIcons)
                {
                    icon.SetActive(false);
                }
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
                GameManager.instance.isHit = false;
                StartCoroutine(changeCooltime());
                StartCoroutine(realChangeCool());
            }
            if (context.control.name == "3" && player.characterClass != CharacterType.Wizard)
            {
                player.weaponObjects[0].SetActive(false);
                player.weaponObjects[1].SetActive(false);
                player.weaponObjects[2].SetActive(true);
                foreach (var icon in WarriorSkillIcons)
                {
                    icon.SetActive(false);
                }
                foreach (var icon in ArcherSkillIcons)
                {
                    icon.SetActive(false);
                }
                foreach (var icon in WizardSkillIcons)
                {
                    icon.SetActive(true);
                }
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
                GameManager.instance.isHit = false;
                StartCoroutine(changeCooltime());
                StartCoroutine(realChangeCool());
            }
        }
    }

    private IEnumerator changeCooltime()
    {
        player.canChange = false;
        changeCool = gameManager._changeCooldown;
        while (changeCool > 0.0f)
        {
            changeCool -= Time.deltaTime;
            
            string t = TimeSpan.FromSeconds(changeCool).ToString(@"ss");

            for (int i = 0; i < characterCooltimetexts.Length; i++)
            {
                characterCooltimetexts[i].text = string.Format("{0}", t);
                characterImages[i].fillAmount = changeCool / gameManager._changeCooldown;
                if (characterCooltimetexts[i].text == "00")
                {
                    characterCooltimetexts[i].text = "";
                }
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator realChangeCool()
    {
        yield return new WaitForSeconds(gameManager._changeCooldown);
        player.canChange = true;
    }

    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        if (gameManager.isPause || gameManager.isDead || gameManager.isOnUI)
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
                   (player.weaponManager.Weapon.ComboCount < 3) && !DashState.IsDash;

        if (isAvailableAttack)
        {
            AttackState.IsBaseAttack = true;
            AttackState.isHolding = true;
            AttackState.canAttack = true;
            AttackState.IsAttack = true;
            // Debug.Log("HoldInteraction AttackState");
            player.stateMachine.ChangeState(StateName.ATTACK);
        }
    }

    private void HandlePressInteraction()
    {
        bool isAvailableAttack = !AttackState.IsBaseAttack &&
                   (player.weaponManager.Weapon.ComboCount < 3) && !DashState.IsDash;

        if (isAvailableAttack)
        {
            AttackState.IsBaseAttack = true;
            AttackState.IsAttack = true;
            // Debug.Log("PressInteraction AttackState");

            player.stateMachine.ChangeState(StateName.ATTACK);
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.isPause && !gameManager.isDead && DashState.CurrentDashCount == 0 && !AttackState.IsAttack
            && !player.isPlaySkill && !gameManager.isOnUI)
        {
            if (!DashState.IsDash)
            {
                Debug.Log("Dash Input");
                StartCoroutine(realDashCool());
                StartCoroutine(DashCooltime());
                DashState.CurrentDashCount++;
                dashState = player.stateMachine.GetState(StateName.DASH);
                dashState.Init(gameManager._dashPower, gameManager._dashCool);
                player.stateMachine.ChangeState(StateName.DASH);
                canMove = false;
            }
        }
    }

    IEnumerator DashCooltime()
    {
        dashCoolTimer = gameManager._dashCool;
        while (dashCoolTimer > 0.0f)
        {
            dashCoolTimer -= Time.deltaTime;
            dashCooltimerImage.fillAmount = dashCoolTimer / gameManager._dashCool;

            string t = TimeSpan.FromSeconds(dashCoolTimer).ToString(@"ss");
            dashTimer.text = string.Format("{0}", t);

            if (dashTimer.text == "00")
            {
                dashTimer.text = "";
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator realDashCool()
    {
        yield return new WaitForSeconds(gameManager._dashCool);
        DashState.CurrentDashCount = 0;
    }

    public void OnClickQ(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack && !player.isPlaySkill && !gameManager.isPause && !gameManager.isDead && !DashState.IsDash && !gameManager.isOnUI)
        {
            if (context.interaction is PressInteraction)
            {
                bool isAvailableSkill = !AttackState.IsSkill_Q && gameManager.cooltimeManager.canUseSkill[player.characterClass.ToString() + 'Q'];
                // 스킬 쿨타임 다 찼을때 isAvailableSkill true로 초기화

                if (isAvailableSkill)
                {
                    AttackState.IsSkill_Q = true;
                    player.isPlaySkill = true;
                    ResetAnimator();
                    player.stateMachine.ChangeState(StateName.ATTACK);
                    AttackState.isHolding = false;
                    gameManager.cooltimeManager.UseSkill(player.characterClass.ToString(), player.characterClass.ToString() + 'Q');
                }
            }
        }
    }

    public void OnClickE(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack && !player.isPlaySkill && !gameManager.isPause && !gameManager.isDead && !DashState.IsDash && !gameManager.isOnUI)
        {
            bool isAvailableAttack = !AttackState.IsSkill_E && gameManager.cooltimeManager.canUseSkill[player.characterClass.ToString() + 'E'];

            if (isAvailableAttack)
            {
                AttackState.IsSkill_E = true;
                player.isPlaySkill = true;
                ResetAnimator();
                player.stateMachine.ChangeState(StateName.ATTACK);
                AttackState.isHolding = false;
                gameManager.cooltimeManager.UseSkill(player.characterClass.ToString(), player.characterClass.ToString() + 'E');
            }
        }
    }
    public void OnClickR(InputAction.CallbackContext context)
    {
        if (context.performed && !AttackState.IsBaseAttack && !player.isPlaySkill! && !gameManager.isPause && !gameManager.isDead && !DashState.IsDash && !gameManager.isOnUI)
        {
            bool isAvailableAttack = !AttackState.IsSkill_R && gameManager.cooltimeManager.canUseSkill[player.characterClass.ToString() + 'R'];

            if (isAvailableAttack)
            {
                AttackState.IsSkill_R = true;
                player.isPlaySkill = true;
                ResetAnimator();
                player.stateMachine.ChangeState(StateName.ATTACK);
                AttackState.isHolding = false;
                gameManager.cooltimeManager.UseSkill(player.characterClass.ToString(), player.characterClass.ToString() + 'R');
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
        if (!canMove || gameManager.isOnUI)
            return;

        #region #캐릭터 움직임 구현
        float curretnMoveSpeed = gameManager._movespeed * CONVERT_UNIT_VALUE;
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
    }


    public void Damage(float damageTaken)
    {
        if (gameManager.isDead || gameManager.isHit)
            return;
        //if (gameManager.canRebirth)
        //{
        //    gameManager._hp = gameManager._maxhp;
        //}
        gameManager.cameraManager.ShakeCamera(damageTaken * 0.1f, 0.3f);

        StopCoroutine("TakeDamageEffect");
        StartCoroutine("TakeDamageEffect");

        gameManager.isHit = true;
        player.animator.ResetTrigger("hit");
        player.animator.SetTrigger("hit");
        player.OnUpdateStat(gameManager._maxhp, gameManager._hp - damageTaken, gameManager._movespeed);
        if (player.CurrentHp <= 0)
        {
            Dead();
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        float intensity = 0.4f;

        _vignette.intensity.Override(intensity);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.05f;
            if (intensity < 0) intensity = 0;
            _vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Dead()
    {
        gameManager.isDead = true;
        player.animator.SetTrigger("dead");
        gameManager.uiManager.gameOverPanel.SetActive(true);
    }

    void GetMousePosition()
    {
        Ray cemeraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if(groundPlane.Raycast(cemeraRay, out rayLength))
        {
            Vector3 pointToLook = cemeraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    public void Attack()
    {

    }

    public void Shot()
    {

    }

    public void PlayKnockback(Vector3 direction, float knockbackPower, float knockbackDuration)
    {

    }
}
