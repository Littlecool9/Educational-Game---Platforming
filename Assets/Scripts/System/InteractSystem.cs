using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using EducationalGame.Component;
using System;
using System.Linq;
using UnityEngine.UIElements;

namespace EducationalGame
{
    public class InteractSystem : ISystem
    {
        // Handle Interact Input and Behaviours and result
        private Player player;
        private InputComponent inputC;
        private StateComponent stateC;
        public event Action<AlgorithmPuzzle> OnInteractSlot;
        public event Action<AlgorithmPuzzle> OnInteractBox;
        public event Action<AlgorithmPuzzle> OnSwapBoxes;
        
        private AlgorithmPuzzle puzzle;     // Record the puzzle interacting
        private SortingBoxes[] neighbors = new SortingBoxes[2];
        
        
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
            
            if (puzzle != null && stateC.LookingInteractable)
            {
                bool foundInteracatble = false;
                // TODO: if puzzle == algorithm puzzle
                foreach(Entity entity in puzzle.GetEntities())
                {
                    // Choose only the entities within the puzzle
                    // if (!puzzle.ContainEntities(entity)) continue;

                    InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                    if (interactableC == null) continue;

                    if (entity is SortingBoxes)     // TODO: Edit when more interactable objects added
                    {
                        SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(entity);

                        if (interactableC.Interactable && stateC.InteractingObject == null)     // Pick a box to follow
                        {
                            stateC.SetInteractingObject(entity);            

                            SortingBoxSlot slot = FindCorrespondSlot(entity as SortingBoxes, puzzle);
                            
                            InteractableComponent sIC = EntityManager.Instance.GetComponent<InteractableComponent>(slot);
                            BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                            int slotIndex = sbC.slotIndex;

                            // Enable neighbor boxes to be interacables
                            neighbors = FindNeighbors(entity as SortingBoxes, puzzle);
                            SortingBoxes leftNeighbor = neighbors[0];
                            SortingBoxes rightNeighbor = neighbors[1];

                            if (rightNeighbor != null) EnableSortingBoxSwap(rightNeighbor);
                            if (leftNeighbor != null) EnableSortingBoxSwap(leftNeighbor);

                            // Disable Non-neighbor
                            foreach (SortingBoxes box in puzzle.Boxes.Cast<SortingBoxes>())
                            {
                                if (box.ID == neighbors[0]?.ID || box.ID == neighbors[1]?.ID || box.ID == entity.ID) continue;
                                SortingBoxComponent boxC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
                                boxC.swapable = false;
                            }

                            // Update Box Status
                            interactableC.ActivateInteractionBuffer();
                            interactableC.Interactable = false;
                            sbC.slotIndex = -1;

                            // Update Slot Status
                            if (slot == null) Debug.Log("Slot not found");
                            bsC.isPlaced = false;
                            // sIC.Interactable = true;


                            Interact(entity as SortingBoxes);
                            OnInteractBox?.Invoke(puzzle);


                            foundInteracatble = true;
                            break;
                        }
                        else if (interactableC.Interactable && stateC.InteractingObject is SortingBoxes && sbC.swapable)        
                        {
                            // Interactable确保碰到，InteractingObject确保执行交换，swapable确保在交换相邻的box
                            // Swap the slot of Interacting Box and Target Box

                            // entity is the box that is going to be swaped
                            SortingBoxComponent targetBox = EntityManager.Instance.GetComponent<SortingBoxComponent>(entity);
                            SortingBoxComponent selfBox = EntityManager.Instance.GetComponent<SortingBoxComponent>(stateC.InteractingObject);

                            // Update Boxes Status
                            InteractableComponent sbIC = EntityManager.Instance.GetComponent<InteractableComponent>(stateC.InteractingObject);
                            interactableC.ActivateInteractionBuffer();

                            // Update Slots Status
                            SortingBoxSlot targetSlot = FindCorrespondSlot(entity as SortingBoxes, puzzle);
                            SortingBoxSlot selfSlot = FindPreviousSlot(stateC.InteractingObject as SortingBoxes, puzzle);
                            BoxSlotComponent targetSlotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(targetSlot);
                            BoxSlotComponent selfSlotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(selfSlot);
                            selfSlotC.isPlaced = true;
                            targetSlotC.isPlaced = true;

                            // Update Previous neighbor status
                            CompleteSwap();

                            // Swap position
                            selfBox.slotIndex = targetSlotC.index;
                            targetBox.slotIndex = selfSlotC.index;
                            puzzle.LastSwaps[0] = stateC.InteractingObject as SortingBoxes;
                            puzzle.LastSwaps[1] = entity as SortingBoxes;

                            Interact(stateC.InteractingObject as SortingBoxes, targetSlot, entity as SortingBoxes, selfSlot);
                            OnSwapBoxes?.Invoke(puzzle);
                            stateC.ResetInteractingObject();


                            foundInteracatble = true;
                            break;
                        }
                    }
                    else if (entity is SortingBoxSlot)
                    {
                        BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity);
                        
                        if (interactableC.Interactable && stateC.InteractingObject is SortingBoxes && bsC.isPlaced == false)
                        {
                            // Interactable确保碰到，InteractingObject确保执行放下box，isPlaced确保可以放下箱子

                            // put the interacting box to the empty slot

                            InteractableComponent boxIC = EntityManager.Instance.GetComponent<InteractableComponent>(stateC.InteractingObject);

                            // Update Box Status
                            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(stateC.InteractingObject);
                            sbC.slotIndex = bsC.index;

                            // Update Slot Status
                            bsC.isPlaced = true;
                            interactableC.Interactable = false;
                            interactableC.ActivateInteractionBuffer();          // pass to judge system

                            CompleteSwap();

                            Interact(entity as SortingBoxSlot, stateC.InteractingObject as SortingBoxes);
                            OnInteractSlot?.Invoke(puzzle);

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
            if (puzzle == null) return;
            puzzle.OnDisableTrigger += SetPuzzleNull;
        }
        private void SetPuzzleNull()
        {
            if (puzzle == null) return;
            puzzle.OnDisableTrigger -= SetPuzzleNull;
            puzzle = null;
        }

        /// <summary>
        /// Move the given SortingBoxes entity to follow the player with a certain distance.
        /// </summary>
        /// <param name="box">The entity of the SortingBoxes.</param>
        private void Interact(SortingBoxes box)         
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

        private void Interact(SortingBoxes selfBox, SortingBoxSlot selfTargetSlot, SortingBoxes targetBox, SortingBoxSlot targetSlot)
        {
            // move selfBox to targetSlot, move targetBox to selfTargetSlot
            Interact(selfTargetSlot, selfBox);
            Interact(targetSlot, targetBox);
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
            // Update every frame
            // Permit looking for interactables
            if (!inputC.InteractInput)
            {
                stateC.LookingInteractable = false;
                return;
            }
            if (inputC.InteractInput && stateC.InteractingObject is SortingBoxes)
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

        public static SortingBoxes[] FindPreviousSwapBoxes(AlgorithmPuzzle puzzle)
        {
            //换的是1和3时，后面无论查找多少次返回都是1和3
            SortingBoxes[] res = new SortingBoxes[2];
            for (int i = 0; i < puzzle.Boxes.Count; i++)
            {
                bool found = false;
                SortingBoxComponent sb1C = EntityManager.Instance.GetComponent<SortingBoxComponent>(puzzle.Boxes[i]);
                for (int j = i+1; j < puzzle.Boxes.Count; j++)
                {
                    SortingBoxComponent sb2C = EntityManager.Instance.GetComponent<SortingBoxComponent>(puzzle.Boxes[j]);
                    if (sb1C.previousSlotIndex == sb2C.slotIndex && sb2C.previousSlotIndex == sb1C.slotIndex)
                    {
                        res[0] = puzzle.Boxes[i] as SortingBoxes;
                        res[1] = puzzle.Boxes[j] as SortingBoxes;
                        Debug.Log("box 1: " + sb1C.index + " box 2: " + sb2C.index);
                        Debug.Log("box 1 pre slot: " + sb1C.previousSlotIndex + " box 2 pre slot: " + sb2C.previousSlotIndex);
                        found = true;
                    }
                    if (found) break;

                }
                if (found) break;
            }

            return res;
        }

        public static SortingBoxSlot FindCorrespondSlot(SortingBoxes box, AlgorithmPuzzle puzzle)
        {
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box.ID);
            // foreach (var entity in EntityManager.Instance.GetAllEntities())
            foreach (Entity entity in puzzle.GetEntities())
            {
                if (entity is SortingBoxSlot)
                {
                    BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity.ID);
                    if (bsC.index == sbC.slotIndex) return entity as SortingBoxSlot;
                }
            }
            return null;
        }

        public static SortingBoxSlot FindPreviousSlot(SortingBoxes box, AlgorithmPuzzle puzzle)
        {
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
            // foreach (var entity in EntityManager.Instance.GetAllEntities())
            foreach (Entity entity in puzzle.GetEntities())
            {
                if (entity is SortingBoxSlot)
                {
                    BoxSlotComponent bsC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity.ID);
                    if (bsC.index == sbC.previousSlotIndex) return entity as SortingBoxSlot;
                }
            }
            return null;
        }

        public static SortingBoxes[] FindNeighbors(SortingBoxes box, AlgorithmPuzzle puzzle)
        {
            SortingBoxes[] neighbors = new SortingBoxes[2];
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
            int slotIndex = sbC.slotIndex;

            foreach (Entity neighbor in puzzle.GetEntities())
            {
                if (neighbor is SortingBoxes)
                {
                    SortingBoxes nb = neighbor as SortingBoxes;
                    SortingBoxComponent nbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(neighbor);
                    
                    if (nbC.slotIndex == slotIndex - 1)
                    {
                        neighbors[0] = nb;
                    }
                    if (nbC.slotIndex == slotIndex + 1)
                    {
                        neighbors[1] = nb;
                    }
                }
            }

            return neighbors;
        }

        public static void EnableSortingBoxSwap(SortingBoxes box)
        {
            SortingBoxComponent nbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
            RenderComponent nbRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box);
            // nbIC.Interactable = true;
            nbC.swapable = true;
            nbRenderC.sr.color = nbC.enabledSwapColor;   
        }

        public static void DisableSwap(SortingBoxes box)
        {
            // Called after swap complete
            SortingBoxComponent nbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
            RenderComponent nbRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box);
            nbC.swapable = false;
            nbRenderC.sr.color = nbC.initColor;   
        }
        public void CompleteSwap()
        {
            if (neighbors[0] != null) DisableSwap(neighbors[0]);
            if (neighbors[1] != null) DisableSwap(neighbors[1]);
            neighbors[0] = null;
            neighbors[1] = null;
        }
    }
}
