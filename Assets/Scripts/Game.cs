using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;
using EducationalGame;
using EducationalGame.Component;

public class Game : MonoBehaviour
{
    
    // Handle the main process of the game
    [SerializeField]
    public GameObject playerObject;

    [SerializeField]
    public List<AlgorithmPuzzle> algorithmPuzzles; 

    void Start() {
        Application.targetFrameRate = 60; // 将游戏帧率锁定为 60 FPS
        Constants.SetPlayerPrefab(playerObject);

        Constants.Init(this);
        Init();                     // Init Player\TriggerObject
        SystemManager.Init();       // Init Systems
    }

    void FixedUpdate() {
        Constants.SetDeltaTime(Time.deltaTime);

        foreach(var system in SystemManager.systems)
        {
            system.Update();
        }
        // if (GetTriggerPuzzle() != null) Debug.Log("Trigger puzzle: " + Constants.Game.GetTriggerPuzzle().puzzleID);
        
    }

    // Act as an init system
    private void Init(){
        

        // The following logic will be put in level.cs in the future
        // Player init
        GameObject playerObject = Constants.player;

        Player player = EntityManager.Instance.CreateEntity(EntityType.Player, "Player") as Player;
        // Initialize Components
        RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(player.ID);
        renderC.SetGameObject(playerObject);        

        InteractionComponent interactionC = EntityManager.Instance.GetComponent<InteractionComponent>(player.ID);


        // Algorithm Area puzzles init
        foreach (AlgorithmPuzzle puzzle in algorithmPuzzles)
        {
            // Debug.Log("id:" + puzzle.puzzleID);
            List<InteractableComponent> interactables = puzzle.Init();
            interactionC.AddInteractableToList(interactables);
        }

        interactionC.InitInteracables();

    }

    public AlgorithmPuzzle GetTriggerPuzzle()
    {
        foreach (AlgorithmPuzzle puzzle in Constants.Game.algorithmPuzzles)
        {
            if (puzzle.GetTriggerStatus())
            {
                return puzzle;
            }
            // TODO: Multiple puzzles may be triggered at the same time, need to adjust the trigger last time
        }
        return null;
    }

    [ExecuteInEditMode]
    // 计算摄像机的可视范围
    private void OnDrawGizmos() {
        if (Camera.main == null) return;

        // Draw the grid of the map
        float camHeight = Camera.main.orthographicSize * 2;
        float camWidth = camHeight * Camera.main.aspect;

        // 计算当前摄像机在哪个 "页面"
        int xPage = Mathf.FloorToInt(transform.position.x / camWidth);
        int yPage = Mathf.FloorToInt(transform.position.y / camHeight);

        Gizmos.color = Color.green; // 设置边界颜色

        for (int i = -3; i <= 5; i++)
        {
            for (int j = -3; j <= 5; j++)
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

