using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EducationalGame.Core
{
    public static class SystemManager
    {
        public static PlayerController playerController { get; set; }
        public static WalkingSystem walkingSystem { get; set; }
        public static RenderSystem renderSystem { get; set; }

        public static List<ISystem> systems = new List<ISystem>();
        
        public static void Init() {
            playerController = new PlayerController();
            walkingSystem = new WalkingSystem();
            renderSystem = new RenderSystem();
            systems.Add(playerController);
            systems.Add(walkingSystem);
            systems.Add(renderSystem);
        }
    }
}

