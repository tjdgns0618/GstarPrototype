using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;

namespace CharacterController
{
    public class DashState : BaseState
    {
        public static int CurrentDashCount { get; set; } = 0;
        public static bool CanAddInputBuffer { get; set; }
        public static bool IsDash {  get; set; }
        public static int Hash_DashTrigger { get; private set; }
        public static int Hash_IsDashBool { get; private set; }
        public static int Hash_DashPlaySpeedFloat { get; private set; }

        public const float DEFAULT_ANIMATION_SPEED = 2f;
        public static float dashPower { get; set; }
        public static float dashTetanyTime { get; set; }
        public static float dashCooltime { get; set; }

        public DashState(PlayerCharacterController controller) : base(controller) {
            Hash_DashTrigger = Animator.StringToHash("Dash");
            Hash_IsDashBool = Animator.StringToHash("IsDashing");
            Hash_DashPlaySpeedFloat = Animator.StringToHash("DashPlaySpeed");
        }

        public override void OnEnterState()
        {
            Debug.Log("OnEnterState 대시 스테이트 진입");
            Dash();
        }

        private void Dash()
        {
            PlayerCharacter.Instance.animator.SetBool(Hash_IsDashBool, true);
            PlayerCharacter.Instance.animator.SetTrigger(Hash_DashTrigger);
            
            float dashAnimationPlaySpeed = DEFAULT_ANIMATION_SPEED + (PlayerCharacter.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE - MoveState.DEFAULT_CONVERT_MOVESPEED) * 0.1f;
            PlayerCharacter.Instance.animator.SetFloat(Hash_DashPlaySpeedFloat, dashAnimationPlaySpeed);
            PlayerCharacter.Instance.rigidbody.velocity = PlayerCharacter.Instance.transform.forward * (PlayerCharacter.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * dashPower;
        }


        public override void OnExitState()
        {
            PlayerCharacter.Instance.rigidbody.velocity = Vector3.zero;
            PlayerCharacter.Instance.animator.SetBool(Hash_IsDashBool,false);
            AttackState.IsBaseAttack = false;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }

        public override void Init(float dashPower, float dashTetanyTime, float dashCooltime)
        {
            DashState.dashPower = dashPower;
            DashState.dashTetanyTime = dashTetanyTime;
            DashState.dashCooltime = dashCooltime;
        }
    }
}
