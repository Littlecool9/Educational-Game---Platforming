using System.Collections;
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

    void Start() {
        Constants.SetPlayerPrefab(playerObject);
        SystemManager.Init();
        Init();
    }

    void FixedUpdate() {
        Constants.SetDeltaTime(Time.deltaTime);
        foreach(var system in SystemManager.systems)
        {
            system.Update();
        }
    }

    private void Init(){

        // Player init
        Player player = EntityManager.Instance.CreateEntity(EntityType.Player, "Player") as Player;
        RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(player.ID);
        GameObject playerObject = Constants.player;
        renderC.SetGameObject(playerObject);

        
    }
}

