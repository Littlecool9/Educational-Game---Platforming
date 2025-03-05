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
            DetermineAction();

            // if (!stateC.IsInteracting && stateC.CanInteract)
            if (stateC.LookingInteractable && stateC.CanInteract)
            {
                bool foundInteracatble = false;
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
                            stateC.SetInteractingObject(entity);            // TODO: Edit when more interactable objects added
                            stateC.IsInteracting = true;

                            
                        }
                        foundInteracatble = true;
                        break;

                    }
                    else if (entity is XORLever){
                        // TODO: Implement interact logic
                    }
                }
                if (!foundInteracatble) 
                {
                    // Failed to find an interactable
                    Debug.Log("Not Found Interacting");
                }

                stateC.LookingInteractable = false;
            }
            else if (stateC.RelieveInteractable && stateC.IsInteracting)
            {
                // 结束互动
                stateC.ResetInteractingObject();
                stateC.IsInteracting = false;

                stateC.RelieveInteractable = false;
                Debug.Log("Quit Interacting");
            }   
        }

        private void InteractWithSortingBox(SortingBoxes box)
        {
            Debug.Log("Interacting Sorting Box");

        }

        private void DetermineAction()
        {
            if (inputC.InteractInput && stateC.CanInteract &&
            (stateC.GetCurrentState() == PlayerState.Idle))         // 可交互的状态
            {
                // Idle -> Interacting
                stateC.LookingInteractable = true;
                // stateC.IsInteracting = true;
            }
            else if (inputC.InteractInput && stateC.IsInteracting)
            {
                // Interacting -> Idle
                Debug.Log("Relieve Request");
                stateC.RelieveInteractable = true;
            }
        }
    }
}
