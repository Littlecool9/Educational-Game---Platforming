using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;
using EducationalGame;
using EducationalGame.Component;

public class Game : MonoBehaviour
{
    // Handle the main process of the game
    [SerializeField]
    public Vector2 StartPosition;
    public GameObject playerObject;
    public List<GameObject> SortingBoxes;
    public List<GameObject> SortingBoxSlots;

    void Start() {
        Application.targetFrameRate = 60; // 将游戏帧率锁定为 60 FPS
        Constants.SetPlayerPrefab(playerObject);
        Constants.Init();
        Init();         // Init Player\TriggerObject
        SystemManager.Init();       // Init Systems
    }

    void FixedUpdate() {
        Constants.SetDeltaTime(Time.deltaTime);
        foreach(var system in SystemManager.systems)
        {
            system.Update();
        }
    }

    private void Init(){
        

        // The following logic will be put in level.cs in the future
        // Player init
        GameObject playerObject = Constants.player;

        Player player = EntityManager.Instance.CreateEntity(EntityType.Player, "Player") as Player;
        // Initialize Components
        RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(player.ID);
        renderC.SetGameObject(playerObject);        // Link Unity Components to customized Components

        InteractionComponent interactionC = EntityManager.Instance.GetComponent<InteractionComponent>(player.ID);


        // SortingBoxes init
        foreach(GameObject sortingBox in SortingBoxes)
        {
            SortingBoxes box = EntityManager.Instance.CreateEntity(EntityType.SortingBoxes) as SortingBoxes;
            InteractableComponent boxInteractableC = EntityManager.Instance.GetComponent<InteractableComponent>(box.ID);
            RenderComponent boxRenderC = EntityManager.Instance.GetComponent<RenderComponent>(box.ID);

            boxRenderC.SetGameObject(sortingBox);
            boxInteractableC.SetTrigger(sortingBox.GetComponent<Trigger>());
            interactionC.AddInteractableToList(boxInteractableC);
        }

        interactionC.InitInteracables();

        // SortingBoxSlots init
        foreach(GameObject sortingBoxSlot in SortingBoxSlots)
        {
            SortingBoxSlot slot = EntityManager.Instance.CreateEntity(EntityType.SortingBoxSlot) as SortingBoxSlot;
            InteractableComponent slotInteractableC = EntityManager.Instance.GetComponent<InteractableComponent>(slot.ID);
            RenderComponent slotRenderC = EntityManager.Instance.GetComponent<RenderComponent>(slot.ID);

            slotRenderC.SetGameObject(sortingBoxSlot);   
            slotInteractableC.SetTrigger(sortingBoxSlot.GetComponent<Trigger>());
            interactionC.AddInteractableToList(slotInteractableC);
        }
    }
}

