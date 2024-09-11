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
        PlayerCharacter.Instance.animator.SetBool("IsAttack", false);
        PlayerCharacter.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void AttackColliderChange()
    {
        PlayerCharacter.Instance.weaponManager.Weapon.GetComponent<MeshCollider>().enabled = 
            !PlayerCharacter.Instance.weaponManager.Weapon.GetComponent<MeshCollider>().enabled;
    }
}
