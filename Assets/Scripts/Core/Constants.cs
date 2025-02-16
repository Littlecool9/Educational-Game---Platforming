using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace EducationalGame.Core
{
    public class Constants : MonoBehaviour
    {
        // Store constants here

        public static string PlayerTag = "Player";
        public static string SortingBoxTag = "SortingBox";
        public static float deltaTime;
        public static float SetDeltaTime(float dt) { deltaTime = dt; return dt; }
        public static float JumpHBoost = 4f; //退离墙壁的力
        public static float JumpGraceTime = 0.1f;//土狼时间
        public static float JumpSpeed = 10.5f;  //最大跳跃速度
        public static float PlayerMoveSpeed = 4f;

        public static GameObject player { get; private set; }

        public static GameObject SetPlayerPrefab(GameObject prefab) { player = prefab; return prefab; }
    }
}

