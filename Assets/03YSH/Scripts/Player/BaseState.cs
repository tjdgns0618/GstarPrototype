using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace CharacterController
{
    public abstract class BaseState
    {
        protected PlayerCharacterController Controller { get; private set; }

        public BaseState(PlayerCharacterController controller)
        {
            this.Controller = controller;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}