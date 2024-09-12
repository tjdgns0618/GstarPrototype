using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public static bool IsAttack = false;
        public const float CanReInputTime = 1f;
        
        public AttackState(PlayerCharacterController controller) : base(controller) { }

        public override void OnEnterState()
        {
            // 공격 실행
            IsAttack = true;
            PlayerCharacter.Instance.weaponManager.Weapon.Attack(this);
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {

        }
    }
}