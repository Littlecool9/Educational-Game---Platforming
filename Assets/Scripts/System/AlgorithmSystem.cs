using System.Collections;
using System.Collections.Generic;
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
                        
        private bool requireSlotCheck;      // Triggered when putting a box into the slot
        private bool requireBoxCheck;       // Triggered when taking a box out of the slot

        private SortingBoxSlot slot;
        private SortingBoxes box;
        private AlgorithmPuzzle puzzle;
        private int MaxTriTime = -1;
        private int TriTime = 0;

        public void Init()
        {
            requireSlotCheck = false;

            // Event Management
            SystemManager.interactSystem.OnInteractSlot += RequireSlotCheck;
            SystemManager.interactSystem.OnInteractBox += RequireBoxCheck;
            foreach(AlgorithmPuzzle puzzle in Constants.Game.algorithmPuzzles)
            {
                puzzle.OnEnableTrigger += UpdatePuzzle;
                puzzle.OnEnableTrigger += DisplayRestTime;
                puzzle.OnDisableTrigger += RemoveDisplayTime;
            }
        }

        public void Process()
        {
            
        }

        public void Update()
        {
            if (requireSlotCheck && puzzle != null)
            {
                // 放下箱子的逻辑
                bool foundBox = false;
                bool foundSlot = false;
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
                            foundSlot = true;
                        }
                        else if (entity is SortingBoxes)
                        {
                            box = entity as SortingBoxes;
                            foundBox = true;
                        }
                    }
                    if (foundBox && foundSlot) break;
                }
                

                if (slot != null && box != null)
                {
                    InteractableComponent bIC = EntityManager.Instance.GetComponent<InteractableComponent>(box);
                    InteractableComponent sIC = EntityManager.Instance.GetComponent<InteractableComponent>(slot);
                    bIC.ComsumeInteractionBuffer();
                    sIC.ComsumeInteractionBuffer();


                    BoxSlotComponent slotComponent = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                    SortingBoxComponent boxComponent = EntityManager.Instance.GetComponent<SortingBoxComponent>(box);
                    RenderComponent slotRC = EntityManager.Instance.GetComponent<RenderComponent>(slot);
                    SpriteRenderer slotSR = slotRC?.sr;

                    // May be moved to render system?
                    if (slotComponent.isTempSlot)
                    {
                        slotSR.color = slotComponent.incorrectColor;
                    }
                    else if(boxComponent.index == slotComponent.index)
                    {
                        // Correctly placed
                        slotSR.color = slotComponent.correctColor;
                        slotComponent.correctlyPlaced = true;
                    }
                    else if (boxComponent.index != slotComponent.index)
                    {
                        // Incorrectly placed
                        slotSR.color = slotComponent.incorrectColor;
                        slotComponent.correctlyPlaced = false;
                    }

                    TriTime += 1;
                    
                    if (CheckPuzzleSuccess(puzzle))
                    {
                        Debug.Log("successfully solved the puzzle");
                        puzzle.SolvePuzzle();       // Haven't handle status: Solved -> Unsolved
                    }
                    else if (TriTime >= MaxTriTime)
                    {
                        // Reset the puzzle
                        Debug.Log("Failed to solve the puzzle");
                        puzzle.ResetPuzzle();
                        TriTime = 0;
                    }
                    
                }
                
                requireSlotCheck = false;
            }
            else if (requireBoxCheck)
            {
                // 拿起箱子时的逻辑
                bool foundBox = false;
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
                        foundBox = true;
                    }
                    if (foundBox) break;
                }
                if (foundBox)
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
            SetBoxAndSlotNull();
        }

        private void RequireSlotCheck() => requireSlotCheck = true;
        private void RequireBoxCheck() => requireBoxCheck = true;
        private void SetBoxAndSlotNull() 
        {
            slot = null;
            box = null;
        }
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
        }

        private bool CheckPuzzleSuccess(AlgorithmPuzzle puzzle)
        {
            foreach(Entity slot in puzzle.Slots)           
            {
                // Check if all slots are correctly placed
                BoxSlotComponent sC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot);
                if (!sC.correctlyPlaced && !sC.isTempSlot)
                {
                    return false;
                }
            }
            return true;
        }

        private void DisplayRestTime()
        {
            if (puzzle == null) return;
            int restTriTime = MaxTriTime - TriTime;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(true);
            tmp.text = "Available Time: " + restTriTime.ToString();
        }

        private void RemoveDisplayTime()
        {
            if (puzzle == null) return;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(false);
        }
    }
}
