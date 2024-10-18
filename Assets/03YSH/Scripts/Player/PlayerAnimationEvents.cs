using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    public PlayerAttack effect;

    public Dictionary<string, IEffect> myWeaponEffects { get; private set; }
    public Dictionary<string, ISound> mySounds { get; private set; }

    DashState dashState;
    private Coroutine dashCoolTimeCoroutine;

    public void OnStartAttack()
    {
        effect.PlayComboAttackEffects();
    }

    public void OnFinishedAttack()
    {
        AttackState.IsAttack = false;
        AttackState.IsBaseAttack = false;
        PlayerCharacter.Instance.animator.SetBool("IsAttack", false);

        AttackState.isClick = false;

        AttackState.canAttack = true;
    }

    public void OnStartDash()
    {
        PlayerCharacter.Instance.animator.SetBool("canHit", false);
    }

    public void OnFinishedDash()
    {
        PlayerCharacter.Instance.animator.SetBool("canHit", true);
    }

    public void OnStartHit()
    {
        PlayerCharacter.Instance.animator.SetBool("canHit", false);
    }

    public void OnFinishedHit()
    {
        PlayerCharacter.Instance.animator.SetBool("canHit", true);
    }

    public void OnStartSkill_Q()
    {
        effect.PlaySkillEffect();
    }

    public void OnFinishedSkill_Q()
    {
        PlayerCharacter.Instance.isPlaySkill = false;

        AttackState.IsAttack = false;
        AttackState.IsSkill_Q = false;
        PlayerCharacter.Instance.animator.SetBool("IsSkill_Q", false);
        // PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnStartSkill_E()
    {

        effect.PlaySkillEffect();
    }


    public void OnFinishedSkill_E()
    {
        PlayerCharacter.Instance.isPlaySkill = false;

        AttackState.IsAttack = false;
        AttackState.IsSkill_E = false;
        PlayerCharacter.Instance.animator.SetBool("IsSkill_E", false);
        // PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnStartSkill_R()
    {
        effect.PlaySkillEffect();
    }

    public void OnFinishedSkill_R()
    {
        PlayerCharacter.Instance.isPlaySkill = false;

        AttackState.IsAttack = false;
        AttackState.IsSkill_R = false;
        PlayerCharacter.Instance.animator.SetBool("IsSkill_R", false);
        // PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }
    
    public void OnEndChangeCharacter()
    {
        PlayerCharacter.Instance.canChange = true;
    }

    public void AttackColliderChange()
    {
        PlayerCharacter.Instance.attackRange.enabled = !PlayerCharacter.Instance.attackRange.enabled;
    }
}
