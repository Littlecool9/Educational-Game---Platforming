using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class ColliderComponent : IComponent
    {
        // Collider detect, interact detect
        public bool IsGrounded = false;
        public bool IsDashing = false;

        public Collider2D Collider;
        public LayerMask GroundLayer; // 用于检测地面

        public float DEVIATION = 0.002f;  //碰撞检测误差

        public void InitComponent()
        {
        }

        public void SetCollider(Collider2D collider){
            Collider = collider;
            GroundLayer = LayerMask.GetMask("Ground");
        }
    }
}
