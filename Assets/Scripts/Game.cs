using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;
using EducationalGame;
using EducationalGame.Component;
using System.Threading.Tasks;
using System.Linq;

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


        // await Task.WhenAll(SystemManager.asyncSystems.Select(asyncSystem => asyncSystem.UpdateAsync()));
        // lastAsyncTask = lastAsyncTask.ContinueWith(async _ =>
        // {
        //     await Task.WhenAll(SystemManager.asyncSystems.Select(system => system.UpdateAsync()));
        // });
        // foreach(var asyncSystem in SystemManager.asyncSystems)
        // {
        //     Task.Run(asyncSystem.UpdateAsync);
        // }

        foreach(var system in SystemManager.systems)
        {
            system.Update();
        }
    }

    // Act as an init system
    private void Init(){
        

        // The following logic will be put in level.cs in the future
        // Player init
        GameObject playerObject = Constants.player;

        Player player = EntityManager.Instance.CreateEntity(EntityType.Player, "Player") as Player;
        // Initialize Components
        RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(player.ID);
        renderC.SetGameObject(playerObject);        // Link Unity Components to customized Components

        InteractionComponent interactionC = EntityManager.Instance.GetComponent<InteractionComponent>(player.ID);


        // SortingBoxSlots init
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

            interactionC.AddInteractableToList(slotInteractableC);
        }

        // SortingBoxes init
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

            interactionC?.AddInteractableToList(boxInteractableC);
        }
        interactionC.InitInteracables();

    }

    [ExecuteInEditMode]
    private void OnDrawGizmos() {
        if (Camera.main == null) return;

        // 计算摄像机的可视范围
        // float camHeight = Camera.main.orthographicSize * 2;
        float camHeight = Camera.main.orthographicSize * 2;
        float camWidth = camHeight * Camera.main.aspect;

        // 计算当前摄像机在哪个 "页面"
        int xPage = Mathf.FloorToInt(transform.position.x / camWidth);
        int yPage = Mathf.FloorToInt(transform.position.y / camHeight);

        Gizmos.color = Color.green; // 设置边界颜色

        // 遍历绘制相邻的 3x3 视野区域
        for (int i = -1; i <= 3; i++)
        {
            for (int j = -1; j <= 3; j++)
            {
                float x = (xPage + i) * camWidth;
                float y = (yPage + j) * camHeight;

                Vector3 bottomLeft = new Vector3(x - camWidth / 2, y - camHeight / 2, 0);
                Vector3 bottomRight = new Vector3(x + camWidth / 2, y - camHeight / 2, 0);
                Vector3 topLeft = new Vector3(x - camWidth / 2, y + camHeight / 2, 0);
                Vector3 topRight = new Vector3(x + camWidth / 2, y + camHeight / 2, 0);

                // 画四条边线
                Gizmos.DrawLine(bottomLeft, bottomRight);
                Gizmos.DrawLine(bottomRight, topRight);
                Gizmos.DrawLine(topRight, topLeft);
                Gizmos.DrawLine(topLeft, bottomLeft);
            }
        }
    }
}

