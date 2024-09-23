using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    public PlayerAttack effect;
    public BoxCollider baseAttackRange;

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
        baseAttackRange.enabled = !baseAttackRange.enabled;
    }
}
