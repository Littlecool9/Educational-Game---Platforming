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

            if (stateC.InteractingObject is SortingBoxes)        // The type needs consistent interaction
            {
                // Consistently interact with an object
                if (stateC.InteractingObject is SortingBoxes) {
                    Interact(stateC.InteractingObject as SortingBoxes); 
                }
            }
            if (stateC.LookingInteractable)
            {
                bool foundInteracatble = false;
                foreach(Entity entity in EntityManager.Instance.GetAllEntities())
                {
                    InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                    if (interactableC == null) continue;

                    if (entity is SortingBoxes)     // TODO: Edit when more interactable objects added
                    {
                        if (stateC.InteractingObject is SortingBoxes) continue;
                        if (interactableC.Interactable){

                            interactableC.InvokeInteractionEvent();
                            stateC.SetInteractingObject(entity);            // TODO: Edit when more interactable objects added

                            Interact(entity as SortingBoxes);
                        }
                        foundInteracatble = true;
                        break;

                    }
                    else if (entity is SortingBoxSlot){
                        if (interactableC.Interactable && stateC.InteractingObject is SortingBoxes){

                            stateC.RelieveInteractable = false;
                            interactableC.InvokeInteractionEvent();

                            Interact(entity as SortingBoxSlot, stateC.InteractingObject as SortingBoxes);

                            stateC.ResetInteractingObject();
                        }
                        foundInteracatble = true;
                        break;
                    }
                    else if (entity is XORLever){
                        // TODO: Implement interact logic
                        if (interactableC.Interactable)
                        {
                            Interact(entity as XORLever);
                        }
                        // stateC.IsInteracting = false;       // 不会进入持续状态
                    }
                }
                if (!foundInteracatble) 
                {
                    // Failed to find an interactable
                    
                }

                stateC.LookingInteractable = false;
            }
        }

        private void Interact(SortingBoxes box)         // Overload for different interactable
        {
            InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(box.ID);

            interactableC.BeingInteracted = true;
            interactableC.Interactable = false;

            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box.ID);
            RenderComponent boxRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box.ID);
            Transform boxTransform = boxRenderC?.transform;

            RenderComponent playerRenderC = EntityManager.Instance.GetComponent<RenderComponent>(player);
            Transform playerTransform = playerRenderC?.transform;
            if(playerTransform == null || boxTransform == null) Debug.Log("Missing Transform in InteractWithSortingBox()");

            Vector2 direction = playerTransform.right * -1; // 让跟随物体始终在主角背后
            Vector2 targetPosition = (Vector2)playerTransform.position + direction.normalized * sbC.distance;
            boxTransform.position = Vector2.Lerp(boxTransform.position, targetPosition, sbC.followSpeed * Constants.deltaTime);

        }

        // Interacting with a SortingBoxSlot
        private void Interact(SortingBoxSlot slot, SortingBoxes box)
        {
            Debug.Log("Interacting Sorting Box Slot");
            
            InteractableComponent boxIC = EntityManager.Instance.GetComponent<InteractableComponent>(slot);
            boxIC.Interactable = true;
            boxIC.BeingInteracted = false;

            RenderComponent boxRC = EntityManager.Instance.GetComponent<RenderComponent>(box);
            RenderComponent slotRC = EntityManager.Instance.GetComponent<RenderComponent>(slot);
            if (boxRC == null || slotRC == null) Debug.Log("Missing RenderComponent in InteractWithSortingBoxSlot()");

            boxRC.MoveTransform(slotRC.transform.position - boxRC.transform.position);
        }

        private void Interact(XORLever lever)
        {
            
        }

        private void DetermineAction()
        {   
            if (stateC.InteractingObject is SortingBoxes && inputC.InteractInput)
            {
                stateC.LookingInteractable = true;
            }
            if (inputC.InteractInput && stateC.CanInteract &&
            (stateC.GetCurrentState() == PlayerState.Idle))         // 可交互的状态
            {
                // Idle -> Interacting
                stateC.LookingInteractable = true;
                return;
            }
            if (inputC.InteractInput && stateC.InteractingObject != null)
            {
                // Interacting -> Idle
                stateC.RelieveInteractable = true;
                return;
            }
        }
    }
}
