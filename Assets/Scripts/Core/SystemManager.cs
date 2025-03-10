using System.Collections.Generic;


namespace EducationalGame.Core
{
    public static class SystemManager
    {
        public static PlayerController playerController { get; private set; }
        public static PhysicsSystem physicsSystem { get; private set; }
        public static RenderSystem renderSystem { get; private set; }
        public static InteractSystem interactSystem { get; private set; }
        public static AlgorithmSystem algorithmSystem { get; private set; }
        public static CamaraSystem camaraSystem { get; private set; }
        public static PlayerStateMachine playerStateMachine { get; private set; }

        public static List<ISystem> systems = new List<ISystem>();
        public static List<IAsyncUpdate> asyncSystems = new List<IAsyncUpdate>();
        
        public static void Init() {
            playerController = new PlayerController();
            playerStateMachine = new PlayerStateMachine();
            physicsSystem = new PhysicsSystem();
            renderSystem = new RenderSystem();
            interactSystem = new InteractSystem();
            algorithmSystem = new AlgorithmSystem();
            camaraSystem = new CamaraSystem();

            // Adding order determines the order of execution
            systems.Add(playerController);      // 接受输入
            systems.Add(playerStateMachine);    // 处理状态
            systems.Add(interactSystem);        // 处理互动逻辑
            systems.Add(algorithmSystem);       // 处理算法区域的谜题判定
            systems.Add(physicsSystem);         // 物理系统模拟
            systems.Add(renderSystem);          // 处理动画
            systems.Add(camaraSystem);          // 处理相机

            // asyncSystems.Add(algorithmSystem);
            foreach(var system in systems) { system.Init(); }
        }

        // Event Bus
        
    }
}

