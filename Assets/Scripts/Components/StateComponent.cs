using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public enum PlayerStates { Idle, Walking, Interacting }
    public enum BoxStates { Open, Closed }

    public class StateComponent<T> : IComponent where T : System.Enum
    {
        // Stores Objects States related params
        public T CurrentState { get; private set; }

        // Player params
        public bool IsMoving;
        public bool Movable;
        public bool Jumpable;
        public bool Interactable;
        public bool IsInteracting;
        

        public void InitComponent()
        {
            if (typeof(T) == typeof(PlayerStates)){
                
                IsMoving = false;
                Jumpable = true;
            }
            else if (typeof(T) == typeof(BoxStates)){
                
            }

        }

        public void SetCurrentState(T state) { CurrentState = state; }

        public struct States{
            public bool IsMoving;
            public bool Jumpable;
            public bool Interactable;
            public bool IsInteracting;
            public static States Idle{
                get{
                    States idle = new States();
                    idle.IsMoving = false;
                    idle.Jumpable = true;
                    idle.Interactable = false;
                    idle.IsInteracting = false;
                    return idle;
                }
            }
        }
        
    }


}
