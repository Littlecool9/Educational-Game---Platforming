using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using EducationalGame;
using UnityEngine;
using System.Linq;
using System;
using TMPro;


public class AlgorithmPuzzle : PuzzleBase
{
    // An instance of a algorithm puzzle
    public int MaxTryTime;

    public TextMeshPro text;


    [SerializeField] public List<GameObject> SortingBoxes;
    [SerializeField] public List<GameObject> SortingBoxSlots;
    public List<Entity> Boxes = new List<Entity>();
    public List<Entity> Slots = new List<Entity>();
    public override List<Entity> Entities { 
        get{ return Boxes.Concat(Slots).ToList();} 
        set => throw new NotSupportedException(); 
    }

    
    [SerializeField] private List<Gate> _gates;

    public override List<Gate> Gates
    {
        get => _gates;
        set => _gates = value;
    }

    [SerializeField] private List<MaskTrigger> _mapMasks;
    public override List<MaskTrigger> MapMasks 
    {
        get => _mapMasks;
        set => _mapMasks = value;
    }

    
    public SortingBoxes[] LastSwaps = new SortingBoxes[2];

    public float returnSpeed = 5f; // 飞回的速度



    private Dictionary<RenderComponent, int> initialState = new Dictionary<RenderComponent, int>();         // TODO: Current implement way is not following framework


    public override List<InteractableComponent> Init()
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
            slotRenderC.bridge.LinkEntity(slotC);
            
            slotRenderC.sr.color = slotC.initialColor;
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

            boxRenderC.bridge.LinkEntity(sbC);

            boxInteractableC?.SetTrigger(boxRenderC.trigger);
            boxInteractableC.OnStayTrigger += RefreshTrigger;
            boxInteractableC.EnableInteraction += RefreshTrigger;


            interactables.Add(boxInteractableC);

            // Store initial state
            initialState.Add(boxRenderC, sbC.slotIndex);
        }

        return interactables;
    }

    

    public override void ResetPuzzle()
    {
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

        // Reset each component
        foreach (Entity entity in Entities)
        {
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(entity.ID);
            sbC?.Reset();
            BoxSlotComponent slotC = EntityManager.Instance.GetComponent<BoxSlotComponent>(entity.ID);
            slotC?.Reset();
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

    public int GetMaxTryTime() => MaxTryTime;

    public bool ContainEntities(Entity entity)
    {
        foreach (Entity e in Entities)
        {
            if (e.ID == entity.ID) return true;
        }
        return false;
    }

}
