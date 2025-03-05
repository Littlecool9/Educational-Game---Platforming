using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public enum PlayerState { Idle, Walking, Jumping, OnAir }

    public class StateComponent : IComponent 
    {
        // Stores Player States related params
        public PlayerState CurrentState { get; private set; }
        public PlayerState PreviousState { get; private set; }

        public bool IsGrounded { get; set; }
        public bool CanInteract { get; set; }
        public bool IsInteracting { get; set; }
        public Entity InteractingObject { get; set; }
        // public bool OnAir { get; set; }
        // public bool IsDashing = false;
        
        public void InitComponent()
        {
            CanInteract = true;
        }

        public void SetCurrentState(PlayerState state) 
        { 
            PreviousState = CurrentState;
            CurrentState = state; 

            // Update Variables
            if (state == PlayerState.OnAir) {
                // OnAir = true;
                CanInteract = false;
            }
            if (PreviousState == PlayerState.OnAir && (CurrentState == PlayerState.Idle || CurrentState == PlayerState.Walking)) {
                CanInteract = true;
            }
        }
        public PlayerState GetCurrentState() { return CurrentState; }
        public PlayerState GetPreviousState() { return PreviousState; }
        
        public void SetInteractingObject(Entity entity) 
        { 
            InteractingObject = entity; 
            CanInteract = false;
        }
        public void ResetInteractingObject() { InteractingObject = null; CanInteract = true; }
    }


}
