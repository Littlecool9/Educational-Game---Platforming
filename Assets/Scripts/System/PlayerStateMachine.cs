using System;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using Unity.Mathematics;
using UnityEngine;

namespace EducationalGame
{
    public class PlayerStateMachine : ISystem
    {
        Player player;
        StateComponent stateC;
        InputComponent inputC;
        ColliderComponent colliderC;
        RenderComponent renderC;

        private static readonly Dictionary<PlayerState, HashSet<PlayerState>> StateTransitions = new Dictionary<PlayerState, HashSet<PlayerState>>()
        {
            { PlayerState.Idle, new HashSet<PlayerState> { PlayerState.Walking, PlayerState.Jumping, PlayerState.Interacting } },
            { PlayerState.Walking, new HashSet<PlayerState> { PlayerState.Idle, PlayerState.Jumping } },
            { PlayerState.Interacting, new HashSet<PlayerState> { PlayerState.Idle } },
            { PlayerState.Jumping, new HashSet<PlayerState> { PlayerState.Idle, PlayerState.Walking, PlayerState.OnAir } },
            { PlayerState.OnAir, new HashSet<PlayerState> { PlayerState.Idle, PlayerState.Walking } }
        };
        
        
        // Handle Player State Logic
        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
            stateC = EntityManager.Instance.GetComponent<StateComponent>(player);
            inputC = EntityManager.Instance.GetComponent<InputComponent>(player);
            colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(player);
            renderC = EntityManager.Instance.GetComponent<RenderComponent>(player);
            
        }

        public void Process()
        {
            
        }

        public void Update()
        {

            // Updated IsGrounded
            bool isgrounded = CheckGrounded();
            colliderC.IsGrounded = isgrounded;
            stateC.IsGrounded = isgrounded;


            PlayerState currentState = stateC.GetCurrentState();
            PlayerState nextState = DetermineNextState();

            if (currentState != nextState && CanTransition(currentState, nextState))
            {
                stateC.SetCurrentState(nextState);
            }
            Debug.Log("Current State: " + stateC.CurrentState); 
        }

        private PlayerState DetermineNextState()
        {
            
            if (inputC.JumpInput && stateC.IsGrounded)
            {
                // Jumping status last only one frame
                return PlayerState.Jumping;
            }
            if ( !stateC.IsGrounded && math.abs(inputC.MoveDir.x) < 0.1f)
            {
                return PlayerState.OnAir;       // avoid interacting on air
            }
            if (math.abs(inputC.MoveDir.x) > 0.1f)
            {
                // NOTE: air walking is enabled
                return PlayerState.Walking;
            }
            if (inputC.InteractInput && stateC.CurrentState == PlayerState.Idle)    // TODO: 加一个碰到交互物体的条件
            {
                return PlayerState.Interacting;
            }
            if (inputC.InteractInput && stateC.CurrentState == PlayerState.Interacting)
            {
                return PlayerState.Idle;
            }
            return PlayerState.Idle;
        }

        public static bool CanTransition(PlayerState from, PlayerState to)
        {
            return StateTransitions.ContainsKey(from) && StateTransitions[from].Contains(to);
        }

        private bool CheckGrounded()
        {
            if (colliderC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            Vector3 origion = colliderC.Collider.bounds.center;
            Vector3 boxSize = colliderC.Collider.bounds.size;
            boxSize.x -= 0.5f;      // 修复检测竖直碰撞时会触发水平碰撞检测的bug

            // Debug.DrawRay(origion, Vector3.up * 0.2f, Color.green, 2f);  // 绿色的点，持续2秒
            // Debug.DrawRay(origion, Vector3.right * 0.2f, Color.green, 2f); // 右侧的小线，方便看到

            // // 画出 BoxCast 检测方向
            // Debug.DrawRay(origion, Vector3.down * colliderC.DEVIATION, Color.red, 2f);  // 红色的射线表示 BoxCast 方向

            // // 可视化 `BoxCast` 的边界
            // DrawBox(origion, boxSize, Color.blue, 2f);

            RaycastHit2D hit = Physics2D.BoxCast(
                colliderC.Collider.bounds.center,
                // colliderC.Collider.bounds.size, 
                boxSize,
                0, 
                Vector2.down, 
                colliderC.DEVIATION,        // 投射距离
                colliderC.GroundLayer);    // 投射检测的层级
            if (hit && hit.normal == Vector2.up)
            {
                return true;
            }
            return false;
        }

        void DrawBox(Vector3 center, Vector3 size, Color color, float duration)
        {
            Vector3 halfSize = size * 0.5f;
            Vector3 topLeft = new Vector3(center.x - halfSize.x, center.y + halfSize.y, 0);
            Vector3 topRight = new Vector3(center.x + halfSize.x, center.y + halfSize.y, 0);
            Vector3 bottomLeft = new Vector3(center.x - halfSize.x, center.y - halfSize.y, 0);
            Vector3 bottomRight = new Vector3(center.x + halfSize.x, center.y - halfSize.y, 0);

            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topRight, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, topLeft, color, duration);
        }
    }
}

