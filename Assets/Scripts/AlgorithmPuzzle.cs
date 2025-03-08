using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using EducationalGame;
using UnityEngine;


public class AlgorithmPuzzle : MonoBehaviour
{

    public List<GameObject> SortingBoxes;
    public List<GameObject> SortingBoxSlots;
    // Start is called before the first frame update

    public List<InteractableComponent> Init()
    {
        List<InteractableComponent> interactables = new List<InteractableComponent>();

        // Slot Init
        foreach(GameObject sortingBoxSlot in SortingBoxSlots)
        {
            SortingBoxSlot slot = EntityManager.Instance.CreateEntity(EntityType.SortingBoxSlot) as SortingBoxSlot;
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
            InteractableComponent boxInteractableC = EntityManager.Instance.GetComponent<InteractableComponent>(box.ID);
            RenderComponent boxRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box.ID);
            SortingBoxComponent sbC = EntityManager.Instance.GetComponent<SortingBoxComponent>(box.ID);

            boxRenderC?.SetGameObject(sortingBox);
            sbC.SetOrder(boxRenderC.sortingBoxBridge.order);

            sbC.SetBridge(boxRenderC.sortingBoxBridge);

            boxInteractableC?.SetTrigger(boxRenderC.trigger);

            // interactionC?.AddInteractableToList(boxInteractableC);
            interactables.Add(boxInteractableC);
        }

        return interactables;
    }
}
