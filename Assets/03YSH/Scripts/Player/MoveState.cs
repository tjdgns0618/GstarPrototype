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
            PlayerCharacter.Instance.animator.SetFloat(hasMoveAnimation, 0f);
            PlayerCharacter.Instance.rigidbody.velocity = Vector3.zero;
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {
            float curretnMoveSpeed = Controller.player.MoveSpeed * CONVERT_UNIT_VALUE;
            float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED *
                                        GetAnimationSyncWithMovement(curretnMoveSpeed);

            PlayerCharacter.Instance.rigidbody.velocity = 
                Controller.direction * curretnMoveSpeed + 
                Vector3.up * PlayerCharacter.Instance.rigidbody.velocity.y;

            if(animationPlaySpeed < 0f) animationPlaySpeed = 0f;

            PlayerCharacter.Instance.animator.SetFloat("moveSpeed", animationPlaySpeed);
        }        
    }
}