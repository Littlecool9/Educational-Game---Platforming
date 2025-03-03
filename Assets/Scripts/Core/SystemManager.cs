using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using UnityEngine;


namespace EducationalGame.Core
{
    public static class SystemManager
    {
        public static PlayerController playerController { get; set; }
        public static PhysicsSystem physicsSystem { get; set; }
        public static RenderSystem renderSystem { get; set; }
        public static InteractSystem interactSystem { get; set; }
        public static CamaraSystem camaraSystem { get; set; }
        // public static StateMachine<PlayerStates> playerStateMachine { get; set; }

        public static List<ISystem> systems = new List<ISystem>();
        
        public static void Init() {
            playerController = new PlayerController();
            // playerStateMachine = new StateMachine<PlayerStates>();
            physicsSystem = new PhysicsSystem();
            renderSystem = new RenderSystem();
            interactSystem = new InteractSystem();
            camaraSystem = new CamaraSystem();

            systems.Add(playerController);
            // systems.Add(playerStateMachine);
            systems.Add(interactSystem);
            systems.Add(physicsSystem);
            systems.Add(renderSystem);
            systems.Add(camaraSystem);
            foreach(var system in systems) { system.Init(); }
        }
    }
}

