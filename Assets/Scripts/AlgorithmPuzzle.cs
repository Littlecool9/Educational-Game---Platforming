using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using EducationalGame;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using TMPro;


public class AlgorithmPuzzle : MonoBehaviour        
{
    // TODO: Puzzle common variable and function can be defined with interface at the end
    // TODO: Refactor when more puzzles implementing
    private static int nextID = 1;
    public int puzzleID { get; private set; }
    public int MaxTryTime;

    public TextMeshPro text;


    public List<GameObject> SortingBoxes;
    public List<Entity> Boxes = new List<Entity>();
    public List<GameObject> SortingBoxSlots;
    public List<Entity> Slots = new List<Entity>();
    private List<Entity> Entities => Boxes.Concat(Slots).ToList();        // TODO: Current implement way is not following framework>
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
            slotRenderC.sr.color = slotC.initialColor;
            slotInteractableC.Interactable = !slotC.isPlaced;
            slotInteractableC.EnableInteraction += RefreshTrigger;
            slotInteractableC.OnStayTrigger += RefreshTrigger;

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
            sbC.SetOrder(boxRenderC.sortingBoxBridge.boxIndex);

            sbC.SetBridge(boxRenderC.sortingBoxBridge);

            boxInteractableC?.SetTrigger(boxRenderC.trigger);
            boxInteractableC.OnStayTrigger += RefreshTrigger;
            boxInteractableC.EnableInteraction += RefreshTrigger;


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
        foreach (SortingBoxes box in Boxes.Cast<SortingBoxes>())
        {
            RenderComponent boxRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box.ID);
            if (initialState.ContainsKey(boxRenderC))
            {
                int slotIndex = initialState[boxRenderC];
                foreach (SortingBoxSlot slot in Slots.Cast<SortingBoxSlot>())
                {
                    BoxSlotComponent slotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot.ID);
                    if (slotC.index == slotIndex)
                    {
                        RenderComponent slotRenderC = EntityManager.Instance.GetComponent<RenderComponent>(slot.ID);
                        Vector3 target = slotRenderC.transform.position;
                        StartCoroutine(MoveToPosition(boxRenderC.transform, target));
                        break;
                    }
                }
            }
        }
        foreach (SortingBoxSlot slot in Slots.Cast<SortingBoxSlot>())
        {
            RenderComponent slotRenderC = EntityManager.Instance.GetComponent<RenderComponent>(slot.ID);
            BoxSlotComponent slotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(slot.ID);
            slotRenderC.sr.color = slotC.initialColor;
        }
    }

    private IEnumerator MoveToPosition(Transform obj, Vector3 targetPosition)
    {
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
        foreach (Entity entity in Entities)
        {
            InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity.ID);
            if (interactableC == null) throw new Exception("Missing InteractableComponent in SolvePuzzle()");
            interactableC.DisableComponent();
        }
        DisableTrigger();       // Unsubscribe objects
    }

    public List<Entity> GetEntities() => Entities;
    public int GetMaxTryTime() => MaxTryTime;
    public bool GetTriggerStatus() => triggered;

    public bool ContainEntities(Entity entity)
    {
        foreach (Entity e in Entities)
        {
            if (e.ID == entity.ID) return true;
        }
        return false;
    }

    public event Action OnDisableTrigger;
    public event Action OnEnableTrigger;
    public void DisableTrigger()
    {
        Debug.Log("Disable Puzzle "+puzzleID);
        triggered = false;
        Debug.Log("invoke disable trigger");
        OnDisableTrigger?.Invoke();
    }

    public void RefreshTrigger()
    {
        triggered = true;
        Debug.Log("Enable Puzzle "+puzzleID);
        OnEnableTrigger?.Invoke();

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
