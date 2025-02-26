using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Component;
using EducationalGame.Core;

namespace EducationalGame
{
    public class StateMachine<T> : ISystem where T : System.Enum
    {
        

        // State Machine
        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            if (typeof(T) == typeof(PlayerStates))
            {
                // Player's state machine    
                Player entity = EntityManager.Instance.GetEntityWithID(0) as Player;
                StateComponent stateC = EntityManager.Instance.GetComponent<StateComponent>(entity.ID);
                InputComponent inputC = EntityManager.Instance.GetComponent<InputComponent>(entity.ID);

                if (inputC.InteractInput)
                {
                    // Priority in Interacting
                    if (stateC.Interactable)
                    {
                        stateC.SetCurrentState(PlayerStates.Interacting);
                        stateC.Movable = false;
                        stateC.Jumpable = false;
                    }
                }
                else {
                    if (inputC.MoveInput){
                        if (stateC.Movable == false) { stateC.Movable = true; }
                        stateC.SetCurrentState(PlayerStates.Walking);
                    }
                    if (inputC.JumpInput){
                        
                    }
                }
            }
        }
    }
}
