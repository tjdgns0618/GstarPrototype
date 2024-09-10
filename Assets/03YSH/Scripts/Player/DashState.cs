using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DashState : BaseState
    {
        public DashState(PlayerCharacterController controller) : base(controller) {}

        public override void OnEnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            PlayerCharacter.Instance.rigidbody.velocity = Vector3.zero;
        }

        public override void OnFixedUpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
}
