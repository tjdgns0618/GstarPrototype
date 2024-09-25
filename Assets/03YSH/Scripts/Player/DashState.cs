using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DashState : BaseState
    {
        public static bool IsDash = false;

        public DashState(PlayerCharacterController controller) : base(controller) {}

        public override void OnEnterState()
        {
            IsDash = true;
            Controller.Dash();
        }

        public override void OnExitState()
        {

        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
