using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace EducationalGame.Core
{
    public static class SystemManager
    {
        private static PlayerController playerController { get; set; }
        private static PhysicsSystem physicsSystem { get; set; }
        private static RenderSystem renderSystem { get; set; }
        public static InteractSystem interactSystem { get; set; }
        private static CamaraSystem camaraSystem { get; set; }
        private static PlayerStateMachine playerStateMachine { get; set; }

        private static AlgorithmSystem algorithmSystem { get; set; }
        private static EquationSystem equationSystem { get; set; }
        private static LLMSystem llmSystem { get; set; }

        private static List<ISystem> systems = new List<ISystem>();
        private static List<ISystem> updateSystems = new List<ISystem>();
        private static List<ISystem> puzzleSystems = new List<ISystem>();
        
        
        public static void Init() {
            playerController = new PlayerController();
            // playerStateMachine = new PlayerStateMachine();
            // physicsSystem = new PhysicsSystem();
            // renderSystem = new RenderSystem();
            interactSystem = new InteractSystem();
            camaraSystem = new CamaraSystem();

            algorithmSystem = new AlgorithmSystem();
            equationSystem = new EquationSystem();
            llmSystem = new LLMSystem();

            // Adding order determines the order of execution
            updateSystems.Add(playerController);      // 接受输入
            // systems.Add(playerStateMachine);    // 处理状态
            systems.Add(interactSystem);        // 处理互动逻辑
            // systems.Add(algorithmSystem);       // 处理算法区域的谜题判定
            // systems.Add(physicsSystem);         // 物理系统模拟
            // systems.Add(renderSystem);          // 处理动画
            systems.Add(camaraSystem);          // 处理相机

            // asyncSystems.Add(algorithmSystem);
            foreach(var system in updateSystems) { system.Init(); }
            foreach(var system in systems) { system.Init(); }

            puzzleSystems.Add(algorithmSystem);
            puzzleSystems.Add(equationSystem);
            puzzleSystems.Add(llmSystem);

            foreach(var system in puzzleSystems) { system.Init(); }

        }

        public static void Update()
        {
            foreach (var sys in updateSystems)
            {
                sys.Update();
            }
        }

        public static void FixedUpdate(PuzzleBase activePuzzle)
        {
            foreach (var sys in systems)
            {
                sys.Update();
            }

            if (activePuzzle == null) return;

            switch (activePuzzle)
            {
                case AlgorithmPuzzle algorithmPuzzle:
                    algorithmSystem.Update();
                    break;
                case EquationPuzzle equationPuzzle:
                    equationSystem.Update();
                    break;
                case LLMPuzzle llmPuzzle:
                    llmSystem.Update();
                    break;
                default:
                    throw new System.Exception("Invalid Puzzle Type");
            }
        }
        
    }
}

