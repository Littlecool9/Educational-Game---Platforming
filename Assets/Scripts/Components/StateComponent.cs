using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public enum PlayerState { Idle, Walking, Interacting, Jumping, OnAir }

    public class StateComponent : IComponent 
    {
        // Stores Player States related params
        public PlayerState CurrentState { get; private set; }
        public PlayerState PreviousState { get; private set; }

        public bool IsGrounded { get; set; }
        public bool IsInteracting { get; set; }
        public bool OnAir { get; set; }
        public bool IsDashing = false;
        
        public void InitComponent()
        {
            
        }

        public void SetCurrentState(PlayerState state) 
        { 
            PreviousState = CurrentState;
            CurrentState = state; 

            // Manage Variables
            if (state == PlayerState.Interacting) IsInteracting = true;
            if (state == PlayerState.OnAir) OnAir = true;
            if (PreviousState == PlayerState.Interacting  && CurrentState == PlayerState.Idle) 
                IsInteracting = false;
            
        }
        public PlayerState GetCurrentState() { return CurrentState; }
        public PlayerState GetPreviousState() { return PreviousState; }
    }


}
