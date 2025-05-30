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
        RenderComponent renderC;

        private static readonly Dictionary<PlayerState, HashSet<PlayerState>> StateTransitions = new Dictionary<PlayerState, HashSet<PlayerState>>()
        {
            { PlayerState.Idle, new HashSet<PlayerState> { PlayerState.Walking, PlayerState.Jumping,} },
            { PlayerState.Walking, new HashSet<PlayerState> { PlayerState.Idle, PlayerState.Jumping } },
            { PlayerState.Jumping, new HashSet<PlayerState> { PlayerState.Idle, PlayerState.Walking, PlayerState.OnAir } },
            { PlayerState.OnAir, new HashSet<PlayerState> { PlayerState.Idle, PlayerState.Walking } }
        };
        
        
        // Handle Player State Logic
        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
            stateC = EntityManager.Instance.GetComponent<StateComponent>(player);
            inputC = EntityManager.Instance.GetComponent<InputComponent>(player);
            renderC = EntityManager.Instance.GetComponent<RenderComponent>(player);
        }

        public void Process()
        {
            
        }

        public void Update()
        {

            // Updated IsGrounded
            bool isgrounded = CheckGrounded();
            stateC.IsGrounded = isgrounded;

            // Updated State
            PlayerState currentState = stateC.GetCurrentState();
            PlayerState nextState = DetermineNextState();

            if (currentState != nextState && CanTransition(currentState, nextState))
            {
                stateC.SetCurrentState(nextState);
            }
            Debug.Log("Current State: <<" + stateC.CurrentState + ">>"); 
        }

        private PlayerState DetermineNextState()
        {
            if (inputC.JumpInput && stateC.IsGrounded)
            {
                // Jumping status last only one frame
                // Idle -> Jumping
                return PlayerState.Jumping;
            }
            if (!stateC.IsGrounded && math.abs(inputC.MoveDir.x) < 0.1f)
            {
                // Idle/Jumping -> OnAir
                return PlayerState.OnAir;       // avoid interacting on air
            }
            if (math.abs(inputC.MoveDir.x) > 0.1f)
            {
                // NOTE: air walking is enabled
                // Idle/OnAir -> Walking
                return PlayerState.Walking;
            }
            return PlayerState.Idle;
        }



        public static bool CanTransition(PlayerState from, PlayerState to)
        {
            return StateTransitions.ContainsKey(from) && StateTransitions[from].Contains(to);
        }

        private bool CheckGrounded()
        {
            if (renderC == null) throw new Exception("Missing RenderComponent or RenderComponent in CheckGrounded()");

            Vector3 origion = renderC.Collider.bounds.center;
            Vector3 boxSize = renderC.Collider.bounds.size;
            boxSize.x -= 0.5f;      // 修复检测竖直碰撞时会触发水平碰撞检测的bug,用实际检测碰撞箱水平大小小于实际collider实现

            // 画出用于检测的碰撞箱
            // Debug.DrawRay(origion, Vector3.up * 0.2f, Color.green, 2f);  // 绿色的点，持续2秒
            // Debug.DrawRay(origion, Vector3.right * 0.2f, Color.green, 2f); // 右侧的小线，方便看到

            // // 画出 BoxCast 检测方向
            // Debug.DrawRay(origion, Vector3.down * colliderC.DEVIATION, Color.red, 2f);  // 红色的射线表示 BoxCast 方向

            // // 可视化 `BoxCast` 的边界
            // DrawBox(origion, boxSize, Color.blue, 2f);

            RaycastHit2D hit = Physics2D.BoxCast(
                renderC.Collider.bounds.center,
                boxSize,
                0, 
                Vector2.down, 
                Constants.DEVIATION,        // 投射距离
                Constants.GroundLayer);    // 投射检测的层级
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

