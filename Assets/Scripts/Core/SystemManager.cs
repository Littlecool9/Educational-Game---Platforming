using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using UnityEngine;


namespace EducationalGame.Core
{
    public static class SystemManager
    {
        public static PlayerController playerController { get; private set; }
        public static PhysicsSystem physicsSystem { get; private set; }
        public static RenderSystem renderSystem { get; private set; }
        public static InteractSystem interactSystem { get; private set; }
        public static CamaraSystem camaraSystem { get; private set; }
        public static PlayerStateMachine playerStateMachine { get; private set; }

        public static List<ISystem> systems = new List<ISystem>();
        
        public static void Init() {
            playerController = new PlayerController();
            playerStateMachine = new PlayerStateMachine();
            physicsSystem = new PhysicsSystem();
            renderSystem = new RenderSystem();
            interactSystem = new InteractSystem();
            camaraSystem = new CamaraSystem();

            systems.Add(playerController);      // 接受输入
            systems.Add(playerStateMachine);    // 处理状态
            systems.Add(interactSystem);        // 处理互动逻辑
            systems.Add(physicsSystem);         // 物理系统模拟
            systems.Add(renderSystem);          // 处理动画
            systems.Add(camaraSystem);          // 处理相机
            foreach(var system in systems) { system.Init(); }
        }
    }
}

