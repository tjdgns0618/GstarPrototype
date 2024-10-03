using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public const float CONVERT_UNIT_VALUE = 0.01f;
        public const float DEFAULT_CONVERT_MOVESPEED = 3f;
        public const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
        private int hasMoveAnimation;

        public MoveState(PlayerCharacterController controller) : base(controller)
        {
            hasMoveAnimation = Animator.StringToHash("moveSpeed");
        }

        protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
        {
            if (Controller.direction == Vector3.zero)
            {
                return -DEFAULT_ANIMATION_PLAYSPEED;
            }

            return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.5f;
        }

        public override void OnEnterState()
        {

        }

        public override void OnExitState()
        {

        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void Init(float dashPower, float dashTetanyTime, float dashCooltime)
        {
        }
    }
}