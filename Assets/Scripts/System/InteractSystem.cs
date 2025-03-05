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

            if (stateC.IsInteracting && !stateC.RelieveInteractable 
            && stateC.InteractingObject is SortingBoxes)
            {
                // Consistently interact objects
                Interact(stateC.InteractingObject as SortingBoxes); 
            }
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
                            stateC.SetInteractingObject(entity);            // TODO: Edit when more interactable objects added
                            stateC.IsInteracting = true;

                            Interact(entity as SortingBoxes);
                        }
                        foundInteracatble = true;
                        break;

                    }
                    else if (entity is XORLever){
                        // TODO: Implement interact logic

                        stateC.IsInteracting = false;       // 不会进入持续状态
                        Interact(entity as XORLever);
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
                Entity interactingObject = stateC.GetInteractingObject();
                if (interactingObject is SortingBoxes) QuitInteract(interactingObject as SortingBoxes);
                
                Debug.Log("Quit Interacting");
            }   
        }

        private void Interact(SortingBoxes box)         // Overload for different interactable
        {
            Debug.Log("Interacting Sorting Box");
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

        private void Interact(XORLever lever)
        {
            
        }

        private void QuitInteract(SortingBoxes box)
        {
            
        }

        private void QuitInteract(XORLever lever)
        {

        }

        private void DetermineAction()
        {
            if (inputC.InteractInput && stateC.CanInteract &&
            (stateC.GetCurrentState() == PlayerState.Idle))         // 可交互的状态
            {
                // Idle -> Interacting
                stateC.LookingInteractable = true;
            }
            else if (inputC.InteractInput && stateC.IsInteracting)
            {
                // Interacting -> Idle
                stateC.RelieveInteractable = true;
            }
        }
    }
}
