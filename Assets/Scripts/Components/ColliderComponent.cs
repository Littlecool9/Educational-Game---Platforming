using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class ColliderComponent : IComponent
    {
        public bool IsGrounded = false;
        public bool IsDashing = false;

        public bool IsTrigger = false;

        public float DashTime = 0.2f;
        public float DashCooldown = 1.0f;
        public float LastDashTime = -1f;

        public Collider2D Collider;
        public LayerMask GroundLayer; // 用于检测地面

        public void InitComponent()
        {
        }

        public void SetCollider(Collider2D collider){
            Collider = collider;
            GroundLayer = LayerMask.GetMask("Ground");
        }
    }
}
