using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using EducationalGame;
using UnityEngine;
using System.Linq;
using System;


public class AlgorithmPuzzle : MonoBehaviour
{
    private static int nextID = 1;
    private int puzzleID;
    public int MaxTryTime;


    public List<GameObject> SortingBoxes;
    // public List<RenderComponent> Boxes = new List<RenderComponent>();
    public List<SortingBoxes> Boxes = new List<SortingBoxes>();
    public List<GameObject> SortingBoxSlots;
    // public List<RenderComponent> Slots = new List<RenderComponent>();
    public List<SortingBoxSlot> Slots = new List<SortingBoxSlot>();
    public List<GameObject> Gates;


    public float returnSpeed = 5f; // 飞回的速度

    // Trigger the puzzle, signs this puzzle is on
    private bool triggered = false;
    private Coroutine resetCoroutine;
    public float resetTime = 10f;

    private bool success = false;


    private Dictionary<RenderComponent, int> initialState = new Dictionary<RenderComponent, int>();         // TODO: Current implement way is not following framework

    public AlgorithmPuzzle()
    {
        puzzleID = nextID++;
        
    }

    public List<InteractableComponent> Init()
    {
        List<InteractableComponent> interactables = new List<InteractableComponent>();

        // Slot Init
        foreach(GameObject sortingBoxSlot in SortingBoxSlots)
        {
            SortingBoxSlot slot = EntityManager.Instance.CreateEntity(EntityType.SortingBoxSlot) as SortingBoxSlot;
            Slots.Add(slot);

            InteractableComponent slotInteractableC = EntityManager.Instance.GetComponent<InteractableComponent>(slot.ID);
            RenderComponent slotRenderC = EntityManager.Instance.GetComponent<RenderComponent>(slot.ID);
            BoxSlotComponent slotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot.ID);

            slotRenderC?.SetGameObject(sortingBoxSlot);   
            slotInteractableC?.SetTrigger(slotRenderC.trigger);
            slotC?.SetBridge(slotRenderC.slotBridge);
            slotInteractableC.Interactable = !slotC.isPlaced;

            // interactionC.AddInteractableToList(slotInteractableC);
            interactables.Add(slotInteractableC);
        }

        // SortingBoxes Init
        foreach(GameObject sortingBox in SortingBoxes)
        {
            SortingBoxes box = EntityManager.Instance.CreateEntity(EntityType.SortingBoxes) as SortingBoxes;
            Boxes.Add(box);

            InteractableComponent boxInteractableC = EntityManager.Instance.GetComponent<InteractableComponent>(box.ID);
            RenderComponent boxRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box.ID);
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box.ID);

            boxRenderC?.SetGameObject(sortingBox);
            sbC.SetOrder(boxRenderC.sortingBoxBridge.order);

            sbC.SetBridge(boxRenderC.sortingBoxBridge);

            boxInteractableC?.SetTrigger(boxRenderC.trigger);
            boxInteractableC.EnableInteraction += EnableTrigger;

            // interactionC?.AddInteractableToList(boxInteractableC);
            interactables.Add(boxInteractableC);

            // Store initial state
            initialState.Add(boxRenderC, sbC.slotIndex);
        }

        return interactables;
    }

    

    public void ResetPuzzle()
    {
        // Handle Animation
        // Put each sorting box back to its initial position
        foreach (SortingBoxes box in Boxes)
        {
            RenderComponent boxRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box.ID);
            if (initialState.ContainsKey(boxRenderC))
            {
                int index = initialState[boxRenderC];
                if (index >= 0 && index < Slots.Count)
                {
                    RenderComponent slotRenderC = EntityManager.Instance.GetComponent<RenderComponent>(Slots[index].ID);
                    Vector3 target = slotRenderC.transform.position;
                    StartCoroutine(MoveToPosition(boxRenderC.transform, target));
                }
            }
        }
    }

    private IEnumerator MoveToPosition(Transform obj, Vector3 targetPosition)
    {
        Debug.Log("Failed, Reseting");
        yield return new WaitForSeconds(1f);        // time for fail animation
        while (Vector3.Distance(obj.position, targetPosition) > 0.01f)
        {
            obj.position = Vector3.Lerp(obj.position, targetPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
        obj.position = targetPosition; // 确保到达最终位置
    }

    public void SolvePuzzle()
    {
        success = true;
        foreach (GameObject gate in Gates)
        {
            gate.SetActive(false);
        }
    }


    public int GetMaxTryTime() => MaxTryTime;
    public bool GetTriggerStatus() => triggered;
    public void DisableTrigger()
    {
        triggered = false;
        OnDisableTrigger?.Invoke();
    }
    public event Action OnDisableTrigger;

    public void EnableTrigger()
    {
        triggered = true;

        // 如果之前有倒计时协程在运行，先停止它
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        // 启动新的倒计时协程
        resetCoroutine = StartCoroutine(ResetTriggeredAfterDelay());
    }

    private IEnumerator ResetTriggeredAfterDelay()
    {
        yield return new WaitForSeconds(resetTime);
        DisableTrigger();
    }
}
