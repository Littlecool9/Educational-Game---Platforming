using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using System;
using System.Reflection;
using UnityEditor.UIElements;

namespace EducationalGame.Component
{
    public class MovementComponent : IComponent
    {
        // Stores movement related data
        public float Gravity = Physics.gravity.y; // 自定义重力

        
        public Vector2 Speed;    // Delta Moving Speed this frame
        public float JumpHBoost = 4f; //退离墙壁的力 (可以按主体调整)
        public float JumpSpeed = 0.5f;  //最大跳跃速度 (可以按主体调整)
        public float PlayerMoveSpeed = 5f;   // 水平移动速度 (可以按主体调整)

        public float MaxMoveSpeed {get { return PlayerMoveSpeed; } }
        public float MaxFallSpeed = -5f; // 限制最大下落速度
        public float DashSpeed = 20f;
        

        public void SetSpeed(float x, float y){
            if (Speed == null) Speed = Vector2.zero;
            Speed = new Vector2(
                Mathf.Clamp(x, -MaxMoveSpeed, MaxMoveSpeed),
                Mathf.Max(y, MaxFallSpeed)
            );
        }
        public void SetSpeed(Vector2 speed){
            SetSpeed(speed.x, speed.y);
        }

        public void AddSpeed(Vector2 speedDelta)
        {
            if (Speed == null) Speed = Vector2.zero;
            Speed += speedDelta;
            Speed = new Vector2(
                Mathf.Clamp(Speed.x, -MaxMoveSpeed, MaxMoveSpeed),
                Mathf.Max(Speed.y, MaxFallSpeed)
            );
        }
        public void AddSpeed(float x, float y){
            if (Speed == null) Speed = Vector2.zero;
            Vector2 deltaSpeed = new Vector2(x, y);
            AddSpeed(deltaSpeed);
        }


        public void ResetSpeed(){
            Speed = Vector2.zero;
        }

        public void InitComponent()
        {
            
        }
    }
}
