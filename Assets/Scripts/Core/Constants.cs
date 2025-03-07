using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace EducationalGame.Core
{
    public static class Constants 
    {
        // Store constants here

        public static LayerMask GroundLayer; // 用于检测地面
        public static float deltaTime;
        public static float SetDeltaTime(float dt) { deltaTime = dt; return dt; }

        #region Player Constant
        public readonly static string PlayerTag = "Player";
        public readonly static string SortingBoxTag = "SortingBox";
        public readonly static float JumpHBoost = 4f; //退离墙壁的力
        public readonly static float JumpGraceTime = 0.1f;//土狼时间
        public readonly static float JumpSpeed = 8.8f;  //最大跳跃速度
        public readonly static float PlayerMoveSpeed = 4f;

        public static GameObject player { get; private set; }
        public readonly static int DashCornerCorrection = 4;     //水平Dash时，遇到阻挡物的可纠正像素值
        public readonly static float DEVIATION = 0.002f;  //碰撞检测误差

        public readonly static float Gravity = -9f; //重力
        public readonly static float HalfGravThreshold = 4f; //滞空时间阈值
        public readonly static float MaxFall = 20f;
        # endregion

        public static GameObject camera = Camera.main.gameObject;


        #region Corner Correct
        public static int UpwardCornerCorrection = 4; //向上移动，X轴上边缘校正的最大距离
        #endregion

        public static GameObject SetPlayerPrefab(GameObject prefab) { player = prefab; return prefab; }
        

        public static void Init(){
            GroundLayer = LayerMask.GetMask("Ground");
        }
    }
}

