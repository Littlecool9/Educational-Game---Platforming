using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Timers;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

namespace EducationalGame
{
    public class AlgorithmSystem : ISystem
    {
        // Judge the logic of whether an algorithm problem is successfully solved
        // Async possible
                        
        // Flags
        private bool requireSlotCheck;      // Triggered when putting a box into the slot
        private bool requireBoxCheck;       // Triggered when taking a box out of the slot
        private bool requireSwapCheck;      // Triggered when swapping two boxes

        // private SortingBoxSlot slot;
        // private SortingBoxes box;
        private AlgorithmPuzzle puzzle;
        private int MaxTriTime = -1;
        private int TriTime = 0;

        public void Init()
        {
            requireSlotCheck = false;

            // Event Management
            SystemManager.interactSystem.OnInteractSlot += SlotCheck;
            SystemManager.interactSystem.OnInteractBox += BoxCheck;
            SystemManager.interactSystem.OnSwapBoxes += SwapCheck;
            foreach(AlgorithmPuzzle puzzle in Constants.Game.algorithmPuzzles)
            {
                puzzle.OnEnableTrigger += UpdatePuzzle;
                puzzle.OnEnableTrigger += DisplayRestTime;
                puzzle.OnDisableTrigger += RemoveDisplay;
            }
        }

        public void Process()
        {
            
        }

        public void Update()
        {
            if (puzzle == null) return;
            if (requireSlotCheck)
            {
                // 放下箱子的逻辑
                SortingBoxes box = null;
                SortingBoxSlot slot = null;
                // foreach (Entity entity in EntityManager.Instance.GetAllEntities())
                foreach (Entity entity in puzzle.GetEntities())     // 节约搜索性能
                {
                    // 找到正在互动的箱子和槽
                    InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                    if (interactableC == null) continue;

                    if (interactableC.InteractedBuffer)
                    {
                        if (entity is SortingBoxSlot)           
                        {
                            slot = entity as SortingBoxSlot;
                        }
                        else if (entity is SortingBoxes)
                        {
                            box = entity as SortingBoxes;
                        }
                    }
                    if (box != null && slot != null) break;
                }
                

                if (slot != null && box != null)
                {
                    InteractableComponent bIC = EntityManager.Instance.GetComponent<InteractableComponent>(box);
                    InteractableComponent sIC = EntityManager.Instance.GetComponent<InteractableComponent>(slot);
                    bIC.ComsumeInteractionBuffer();
                    sIC.ComsumeInteractionBuffer();


                    BoxSlotComponent slotComponent = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                    RenderComponent slotRC = EntityManager.Instance.GetComponent<RenderComponent>(slot);
                    SpriteRenderer slotSR = slotRC?.sr;

                    // May be moved to render system?
                    if (slotComponent.isTempSlot)
                    {
                        slotSR.color = slotComponent.incorrectColor;
                    }
                    else 
                    {
                        BoxCorrectlyInSlot(box, slot);
                    }

                    TriTime++;
                }
                
                requireSlotCheck = false;
            }
            else if (requireBoxCheck)
            {
                // 拿起箱子时的逻辑
                SortingBoxes box = null;
                SortingBoxSlot slot = null;
                Player player = EntityManager.Instance.GetEntityWithID(0) as Player;
                StateComponent stateC = EntityManager.Instance.GetComponent<StateComponent>(player);
                // foreach (var entity in EntityManager.Instance.GetAllEntities())
                foreach (Entity entity in puzzle.GetEntities())
                {
                    if (entity.ID == stateC.InteractingObject.ID)
                    {
                        box = entity as SortingBoxes;
                        SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
                        slot = InteractSystem.FindCorrespondSlot(box, puzzle);
                        BoxSlotComponent slotComponent = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                    }
                }
                if (box != null && slot != null)
                {
                    BoxSlotComponent slotComponent = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                    if (!slotComponent.isTempSlot)
                    {
                        RenderComponent slotRC = EntityManager.Instance.GetComponent<RenderComponent>(slot);
                        SpriteRenderer slotSR = slotRC?.sr;
                        slotSR.color = slotComponent.incorrectColor;
                    }
                }
                requireBoxCheck = false;
            }
            else if (requireSwapCheck)
            {
                Debug.Log("swap check");
                // 交换箱子时的逻辑
                // SortingBoxes[] swapBoxes = InteractSystem.FindPreviousSwapBoxes(puzzle);
                SortingBoxes[] swapBoxes = puzzle.LastSwaps;
                if (swapBoxes[0] == null || swapBoxes[1] == null) throw new Exception("Swap boxes not found");

                SortingBoxComponent sbC0 = EntityManager.Instance.GetComponent<SortingBoxComponent>(swapBoxes[0]);
                SortingBoxComponent sbC1 = EntityManager.Instance.GetComponent<SortingBoxComponent>(swapBoxes[1]);

                InteractableComponent bIC0 = EntityManager.Instance.GetComponent<InteractableComponent>(swapBoxes[0]);
                InteractableComponent bIC1 = EntityManager.Instance.GetComponent<InteractableComponent>(swapBoxes[1]);
                bIC0.ComsumeInteractionBuffer();
                bIC1.ComsumeInteractionBuffer();

                SortingBoxSlot slot0 = null;
                SortingBoxSlot slot1 = null;

                foreach (SortingBoxSlot slot in puzzle.Slots.Cast<SortingBoxSlot>())
                {
                    BoxSlotComponent slotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                    if (slotC.index == sbC0.slotIndex) slot0 = slot;
                    else if (slotC.index == sbC1.slotIndex) slot1 = slot;
                }
                if (slot0 == null || slot1 == null) throw new Exception("Slot not found");
                BoxCorrectlyInSlot(swapBoxes[0], slot0);
                BoxCorrectlyInSlot(swapBoxes[1], slot1);

                TriTime++;
                
                requireSwapCheck = false;

            }

            if (CheckPuzzleSuccess(puzzle))
            {
                Debug.Log("successfully solved the puzzle");
                
                DisplaySuccess();
                puzzle.SolvePuzzle();       // Haven't handle status: Solved -> Unsolved
                TriTime = 0;
            }
            else if (TriTime >= MaxTriTime)
            {
                // Reset the puzzle
                Debug.Log("Failed to solve the puzzle");
                puzzle.OnEnableTrigger -= UpdatePuzzle;
                DisplayFail();
                puzzle.ResetPuzzle();
                puzzle.OnEnableTrigger += UpdatePuzzle;
                TriTime = 0;
            }
        }

        private void SlotCheck(AlgorithmPuzzle puzzle) => requireSlotCheck = true;
        private void BoxCheck(AlgorithmPuzzle puzzle) => requireBoxCheck = true;
        private void SwapCheck(AlgorithmPuzzle puzzle) => requireSwapCheck = true;

        private void SetPuzzleNull()
        {
            if (puzzle == null) return;
            puzzle.OnDisableTrigger -= SetPuzzleNull;
            puzzle = null;
            MaxTriTime = -1;
        }

        private void UpdatePuzzle()
        {
            puzzle = Constants.Game.GetTriggerPuzzle();
            if (puzzle == null) return;
            MaxTriTime = puzzle.GetMaxTryTime();
            puzzle.OnDisableTrigger += SetPuzzleNull;
            puzzle.OnSolvePuzzle += SetPuzzleNull;
        }

        private bool CheckPuzzleSuccess(AlgorithmPuzzle puzzle)
        {
            foreach(Entity slot in puzzle.Slots)           
            {
                // Check if all slots are correctly placed
                BoxSlotComponent sC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                if (!sC.correctlyPlaced && !sC.isTempSlot) return false;
            }
            return true;
        }

        private void DisplayRestTime()
        {
            if (puzzle == null) return;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(true);;
            int restTriTime = MaxTriTime - TriTime;
            tmp.text = "Available Move: " + restTriTime.ToString();
        }
        private void DisplaySuccess()
        {
            if (puzzle == null) return;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(true);
            tmp.text = "Solved a Puzzle!";
        }
        private void DisplayFail()
        {
            if (puzzle == null) return;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(true);
            tmp.text = "Failed a Puzzle!";
        }

        private void RemoveDisplay()
        {
            if (puzzle == null) return;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(false);
        }

        private bool BoxCorrectlyInSlot(SortingBoxes box, SortingBoxSlot slot)
        {
            // Respond after a box is placed in a slot

            SpriteRenderer slotSR = EntityManager.Instance.GetComponent<RenderComponent>(slot).sr;
            BoxSlotComponent slotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
            SortingBoxComponent boxC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
            if(boxC.index == slotC.index)
            {
                // Correctly placed
                slotSR.color = slotC.correctColor;
                slotC.correctlyPlaced = true;
                return true;
            }
            else
            {
                // Incorrectly placed
                slotSR.color = slotC.incorrectColor;
                slotC.correctlyPlaced = false;
                return false;
            }
        }
    }
}
