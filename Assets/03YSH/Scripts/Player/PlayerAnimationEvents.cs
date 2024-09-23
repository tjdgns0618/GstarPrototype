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
    public Dictionary<string, ISound> mySounds {  get; private set; }

    public void OnStartAttack()
    {
        effect.PlayComboAttackEffects();
        // if(mySounds.TryGetValue(PlayerCharacter.Instance.weaponManager.Weapon.Name, out ISound weaponSound))
        //     weaponSound.PlayComboAttackSound();
    }

    public void OnFinishedAttack()
    {
        AttackState.IsAttack = false;
        AttackState.IsBaseAttack = false;
        PlayerCharacter.Instance.animator.SetBool("IsAttack", false);
        PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedSkill_Q()
    {
        AttackState.IsAttack = false;
        AttackState.IsSkill_Q = false;
        PlayerCharacter.Instance.animator.SetBool("IsSkill_Q", false);
        PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedSkill_E()
    {
        AttackState.IsAttack = false;
        AttackState.IsSkill_E = false;
        PlayerCharacter.Instance.animator.SetBool("IsAttack", false);
        PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedSkill_R()
    {
        AttackState.IsAttack = false;
        AttackState.IsSkill_R = false;
        PlayerCharacter.Instance.animator.SetBool("IsAttack", false);
        PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDash()
    {
        DashState.IsDash = false;
        PlayerCharacter.Instance.animator.SetBool("IsDashing", false);
        PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void AttackColliderChange()
    {
        PlayerCharacter.Instance.attackRange.enabled = !PlayerCharacter.Instance.attackRange.enabled;
    }
}
