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
        public int Facing { get; private set; }     // -1 for left, 1 for right, NO 0

        public int PreviousFacing { get; set; }

        public bool InteractInput { get; set; }     // Interact
        public bool JumpInput { get; set; }         // Jump
        public bool MoveInput { get; set; }
        public bool ReturnInput { get; set; }

        // public KeyCode InteractKey { get; set; }

        public void SetMoveDir(Vector2 input)
        {
            PreviousFacing = Facing;
            Facing = input.x == 0 ? PreviousFacing : (int)Mathf.Sign(input.x);
            MoveDir = input;
        }

        public void InitComponent()
        {
            Facing = -1;
            PreviousFacing = -1;
        }
    }

}
