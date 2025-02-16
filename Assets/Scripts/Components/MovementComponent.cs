using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;

namespace EducationalGame.Component
{
    public class MovementComponent : IComponent
    {
        // Stores movement related data
        public bool IsMoving { get; set; }
        public bool Jumpable { get; set; }
        public Vector2 MoveDir { get; private set; }    // Input move direction
        public Vector2 Speed { get; private set; }      // Accumulated Moving Speed this frame
        public int facing = 0; // -1 = left, 1 = right

        public float JumpHBoost = 4f; //退离墙壁的力 (可以按主体调整)
        public float JumpSpeed = 10.5f;  //最大跳跃速度 (可以按主体调整)
        public float PlayerMoveSpeed = 4f;   // 水平移动速度 (可以按主体调整)
        

        public Vector2 SetDirection(Vector2 direction){
            MoveDir = direction;
            return MoveDir;
        }
        public Vector2 SetSpeed(Vector2 speed){
            Speed = speed;
            return Speed;
        }
        public Vector2 SetSpeed(float x, float y){
            Speed = new Vector2(x, y);
            return Speed;
        }
        public void ResetSpeed(){
            Speed = Vector2.zero;
        }
        

    }
}
