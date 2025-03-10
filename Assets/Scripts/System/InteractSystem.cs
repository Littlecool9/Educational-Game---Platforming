using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using EducationalGame.Component;
using System;

namespace EducationalGame
{
    public class InteractSystem : ISystem
    {
        // Handle Interact Input and Behaviours and result
        private Player player;
        private InputComponent inputC;
        private StateComponent stateC;
        public event Action OnInteractSlot;
        public event Action OnInteractBox;
        
        private AlgorithmPuzzle puzzle;     // Record the puzzle interacting
        
        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
            inputC = EntityManager.Instance.GetComponent<InputComponent>(player);
            stateC = EntityManager.Instance.GetComponent<StateComponent>(player);

            // Event Management
            foreach(AlgorithmPuzzle puzzle in Constants.Game.algorithmPuzzles) 
            { 
                puzzle.OnEnableTrigger += UpdatePuzzle; 
            }
        }

        public void Process()
        {
            throw new NotImplementedException();
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
                    // Choose only the entities within the puzzle
                    if (!puzzle.ContainEntities(entity)) continue;

                    InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                    if (interactableC == null) continue;

                    if (entity is SortingBoxes)     // TODO: Edit when more interactable objects added
                    {
                        if (stateC.InteractingObject is SortingBoxes) continue;      // Skip sorting boxes when already interacting
                        if (interactableC.Interactable){

                            stateC.SetInteractingObject(entity);            

                            SortingBoxSlot slot = FindCorrespondSlot(entity as SortingBoxes);
                            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(entity);
                            InteractableComponent sIC = EntityManager.Instance.GetComponent<InteractableComponent>(slot);
                            BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);

                            // Update Box Status
                            interactableC.ActivateInteractionBuffer();
                            interactableC.Interactable = false;
                            sbC.slotIndex = -1;

                            // Update Slot Status
                            if (slot == null) Debug.Log("Slot not found");
                            bsC.isPlaced = false;
                            sIC.Interactable = true;
                            
                            Interact(entity as SortingBoxes);
                            OnInteractBox?.Invoke();


                            foundInteracatble = true;
                            break;
                        }
                    }
                    else if (entity is SortingBoxSlot){
                        if (interactableC.Interactable && stateC.InteractingObject is SortingBoxes){

                            InteractableComponent boxIC = EntityManager.Instance.GetComponent<InteractableComponent>(stateC.InteractingObject);

                            BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity);

                            // Update Box Status
                            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(stateC.InteractingObject);
                            sbC.slotIndex = bsC.index;
                            boxIC.Interactable = true;

                            // Update Slot Status
                            bsC.isPlaced = true;
                            interactableC.Interactable = false;
                            interactableC.ActivateInteractionBuffer();

                            Interact(entity as SortingBoxSlot, stateC.InteractingObject as SortingBoxes);
                            OnInteractSlot?.Invoke();

                            stateC.ResetInteractingObject();
                            foundInteracatble = true;

                            break;
                        }
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

        private void UpdatePuzzle()
        {
            puzzle = Constants.Game.GetTriggerPuzzle();
            puzzle.OnDisableTrigger += SetPuzzleNull;
        }
        private void SetPuzzleNull()
        {
            puzzle = null;
        }

        private void Interact(SortingBoxes box)         // Overload for different interactable
        {
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
            // Permit looking for interactables
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
        }

        public static SortingBoxSlot FindCorrespondSlot(SortingBoxes box)
        {
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box.ID);
            foreach (var entity in EntityManager.Instance.GetAllEntities())
            {
                if (entity is SortingBoxSlot)
                {
                    BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity.ID);
                    if (bsC.index == sbC.slotIndex) return entity as SortingBoxSlot;
                }
            }
            return null;
        }

        public static SortingBoxSlot FindPreviousSlot(SortingBoxes box)
        {
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
            foreach (var entity in EntityManager.Instance.GetAllEntities())
            {
                if (entity is SortingBoxSlot)
                {
                    BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity.ID);
                    if (bsC.index == sbC.previousSlotIndex) return entity as SortingBoxSlot;
                }
            }
            return null;
        }
    }
}
