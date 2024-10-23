using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using DG.Tweening.Core.Easing;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    public PlayerAttack effect;

    public Dictionary<string, IEffect> myWeaponEffects { get; private set; }
    public Dictionary<string, ISound> mySounds { get; private set; }

    DashState dashState;
    Coroutine dashCoolTimeCoroutine;
    PlayerCharacter playerInstance;
    string canHit = "canHit";

    private void Start()
    {
        playerInstance = PlayerCharacter.Instance;
    }

    public void OnStartAttack()
    {
        effect.PlayComboAttackEffects();
    }

    public void OnFinishedAttack()
    {
        AttackState.IsAttack = false;
        AttackState.IsBaseAttack = false;
        playerInstance.animator.SetBool("IsAttack", false);

        AttackState.isClick = false;

        AttackState.canAttack = true;
    }

    public void OnStartDash()
    {
        playerInstance.animator.SetBool(canHit, false);
    }

    public void OnFinishedDash()
    {
        playerInstance.animator.SetBool(canHit, true);
    }

    public void OnStartHit()
    {
        playerInstance.animator.SetBool(canHit, false);
    }

    public void OnFinishedHit()
    {
        GameManager.instance.isHit = false;
        playerInstance.animator.SetBool(canHit, true);
    }

    public void OnStartSkill_Q()
    {
        effect.PlaySkillEffect();
    }

    public void OnFinishedSkill_Q()
    {
        playerInstance.isPlaySkill = false;

        AttackState.IsAttack = false;
        AttackState.IsSkill_Q = false;
        playerInstance.animator.SetBool("IsSkill_Q", false);
        // playerInstance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnStartSkill_E()
    {

        effect.PlaySkillEffect();
    }


    public void OnFinishedSkill_E()
    {
        playerInstance.isPlaySkill = false;

        AttackState.IsAttack = false;
        AttackState.IsSkill_E = false;
        playerInstance.animator.SetBool("IsSkill_E", false);
        // playerInstance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnStartSkill_R()
    {
        effect.PlaySkillEffect();
    }

    public void OnFinishedSkill_R()
    {
        playerInstance.isPlaySkill = false;

        AttackState.IsAttack = false;
        AttackState.IsSkill_R = false;
        playerInstance.animator.SetBool("IsSkill_R", false);
        // playerInstance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnStartChangeCharacter()
    {
        playerInstance.changeArea.enabled = true;
        playerInstance.changeArea.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        playerInstance.changeArea.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
    }

    public void OnEndChangeCharacter()
    {
        playerInstance.changeArea.enabled = false;
        playerInstance.canChange = true;
    }

    public void AttackColliderChange()
    {
        playerInstance.attackRange.enabled = !playerInstance.attackRange.enabled;
    }
}
