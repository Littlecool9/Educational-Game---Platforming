using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;


namespace EducationalGame.Component
{
    public class InputComponent : IComponent
    {
        // Storing input information

        public Vector2 MoveDir { get; private set; }      // Jump and Move
        public int Facing
        {
            get { if (MoveDir.x == 0) return 0;
                return (int)Mathf.Sign(MoveDir.x); }
            set { }
        }

        public bool InteractInput { get; set; }     // Interact
        public bool JumpInput { get; set; }         // Jump
        public bool MoveInput { get; set; }

        // public KeyCode InteractKey { get; set; }

        public void SetMoveDir(Vector2 input) { MoveDir = input; }

        public void InitComponent()
        {
            
        }
    }

}
