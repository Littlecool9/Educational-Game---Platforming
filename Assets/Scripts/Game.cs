using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;
using EducationalGame.Component;
using System.Linq;

public class Game : MonoBehaviour
{
    
    // Handle the main process of the game
    public GameObject playerObject;

    
    [SerializeField] public List<AlgorithmPuzzle> algorithmPuzzles; 
    [SerializeField] public List<EquationPuzzle> equationPuzzles;
    [SerializeField] public List<LLMPuzzle> llmPuzzles;
    public List<IPuzzle> PuzzlesList {
        get => algorithmPuzzles.Concat<IPuzzle>(equationPuzzles).ToList().Concat(llmPuzzles).ToList();
        private set { throw new System.NotSupportedException("puzzle not supported to be set"); }
    }

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

            SystemManager.Execute(GetTriggerPuzzle());
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

        foreach (IPuzzle puzzle in PuzzlesList) { puzzle.Init(); }
    }

    public IPuzzle GetTriggerPuzzle()
    {
        // TODO: Multiple puzzles may be triggered at the same time, need to adjust the trigger last time
        foreach (IPuzzle puzzle in PuzzlesList) 
        { if (puzzle.triggered) { return puzzle; } }
        return null;
    }

    [ExecuteInEditMode]
    // 给出关卡的segments grids
    private void OnDrawGizmos() {
        if (Camera.main == null) return;
        int cellCount = 7;

        // Draw the grid of the map
        float camHeight = Camera.main.orthographicSize * 2;
        float camWidth = camHeight * Camera.main.aspect;

        // 计算当前摄像机在哪个 "页面"
        int xPage = Mathf.FloorToInt(transform.position.x / camWidth);
        int yPage = Mathf.FloorToInt(transform.position.y / camHeight);

        Gizmos.color = Color.green; // 设置边界颜色

        for (int i = -cellCount; i <= cellCount; i++)
        {
            for (int j = -cellCount; j <= cellCount; j++)
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

    // TODO: 整理一个函数helper类
    public static IBridge GetBridgeComponent(GameObject obj)
    {
        foreach (MonoBehaviour comp in obj.GetComponents<MonoBehaviour>())
        {
            if (comp is IBridge bridge) return bridge;
        }
        return null;
    }
}

