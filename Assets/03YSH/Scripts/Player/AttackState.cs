using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public static bool IsAttack = false;
        public static bool IsBaseAttack = false;
        public static bool IsSkill_Q = false;
        public static bool IsSkill_E = false;
        public static bool IsSkill_R = false;
        public static bool isHolding = false;
        public static bool canAttack = true;

        public static int comboCount;

        public const float CanReInputTime = 0.2f;
        
        public AttackState(PlayerCharacterController controller) : base(controller) { }

        public override void OnEnterState()
        {
            IsAttack = true;            // ���� ����
            if (IsBaseAttack && !isHolding)            
                PlayerCharacter.Instance.weaponManager.Weapon.Attack(this);            
            else if (IsSkill_Q)
                PlayerCharacter.Instance.weaponManager.Weapon.Skill(this);
            else if (IsSkill_E)
                PlayerCharacter.Instance.weaponManager.Weapon.Skill(this);
            else if (IsSkill_R)
                PlayerCharacter.Instance.weaponManager.Weapon.UltimateSkill(this);
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {
            if (isHolding && comboCount < 3 && canAttack)
            {
                Debug.Log("AttackFixedUpdateState ComboCount = " + comboCount);
                Debug.Log("AttackFixedUpdateState animStart = " + canAttack);
                PlayerCharacter.Instance.weaponManager.Weapon.Attack(this);
                canAttack = false;
            }
        }

        public override void OnExitState()
        {

        }
    }
}