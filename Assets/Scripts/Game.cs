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

    private void Awake() 
    {
        Myd.Platform.Player.OnPlayerInstantiate += InitPlayer;
    }

    void Start() 
    {
        Application.targetFrameRate = 60; // 将游戏帧率锁定为 60 FPS

        EntityManager.Instance.CreateEntity(EntityType.Player, "Player");

        Constants.Init(this);
        InitPuzzles();
        SystemManager.Init();       // Init Systems
    }

    void FixedUpdate() 
    {

        if (playerObject == null)
        {
            // InitPlayer(temp);  
            // Debug.Log("player null");
        }
        else
        {
            Constants.SetDeltaTime(Time.deltaTime);

            foreach(var system in SystemManager.systems)
            {
                system.Update();
            }
        }
        
    }

    private void InitPlayer(GameObject player)
    {
        RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(EntityManager.Instance.GetPlayer());

        Constants.SetPlayerPrefab(player);
        renderC.SetGameObject(player);    

        playerObject = player;
    }

    // Act as an init system
    private void InitPuzzles()
    {
        // The following logic will be put in level.cs in the future

        // Algorithm Area puzzles init
        foreach (AlgorithmPuzzle puzzle in algorithmPuzzles)
        {
            puzzle.Init();
        }
    }

    public AlgorithmPuzzle GetTriggerPuzzle()
    {
        foreach (AlgorithmPuzzle puzzle in algorithmPuzzles)
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

    private void SetPlayerObject()
    {

    }
}

