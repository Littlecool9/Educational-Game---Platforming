using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using EducationalGame.Component;

namespace EducationalGame
{
    public class InteractSystem : ISystem
    {
        // Handle Interact Input and Behaviours and result
        private Player player;
        private InputComponent inputC;
        private StateComponent stateC;
        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
            inputC = EntityManager.Instance.GetComponent<InputComponent>(player);
            stateC = EntityManager.Instance.GetComponent<StateComponent>(player);
        }

        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            DetermineInteracting();

            if (stateC.IsInteracting && stateC.CanInteract)
            {
                bool findInteracatble = false;
                foreach(Entity entity in EntityManager.Instance.GetAllEntities())
                {
                    if (entity is SortingBoxes)     // TODO: Edit when more interactable objects added
                    {
                        InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                        if (interactableC.Interactable){
                            interactableC.InvokeInteractionEvent();
                            // implement interact logic
                            InteractWithSortingBox(entity as SortingBoxes);

                            
                            interactableC.Interactable = false;
                            findInteracatble = true;
                            // stateC.CanInteract = false;
                            stateC.SetInteractingObject(entity);
                            break;
                        }
                    }
                    else if (entity is XORLever){
                        // TODO: Implement interact logic
                    }
                }
                if (!findInteracatble) 
                {
                    // Failed to find an interactable
                    // return to previous state
                    // stateC.SetCurrentState(stateC.GetPreviousState());
                    
                    Debug.Log("Not Interacting");
                }
            }
            else if (stateC.IsInteracting && inputC.InteractInput)
            {
                // 
                stateC.ResetInteractingObject();
            }            
        }

        private void InteractWithSortingBox(SortingBoxes box)
        {
            Debug.Log("Interacting Sorting Box");

        }

        private void DetermineInteracting()
        {
            if (inputC.InteractInput && stateC.CanInteract &&
            (stateC.GetCurrentState() == PlayerState.Idle))         // 可交互的状态
            {
                // Idle -> Interacting
                stateC.CanInteract = false;
                stateC.IsInteracting = true;
            }
            else if (inputC.InteractInput && stateC.IsInteracting)
            {
                // Interacting -> Idle
                stateC.IsInteracting = false;
                stateC.CanInteract = true;
            }
        }
    }
}
