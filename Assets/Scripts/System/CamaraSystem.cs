using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class CamaraSystem : ISystem
    {
        private GameObject camera;
        private Transform player;
        private Vector3 startPos; // 记录摄像机初始位置
        private float camWidth;
        private float camHeight;
        private string camName = "Camera";

        public void Init()
        {
            camera = GameObject.Find(camName);
            // player = Constants.player.transform;

            camHeight = Camera.main.orthographicSize * 2;
            camWidth = camHeight * Camera.main.aspect;

            startPos = camera.transform.position; // 记录摄像机初始位置
        }

        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            if (player == null) player = Constants.player.transform;


            Vector3 playerPos = player.position;
            Vector3 camPos = camera.transform.position;

            bool needMove = false;
            Vector3 newCamPos = camPos;

            // 判断玩家是否超出摄像机的可视范围
            if (playerPos.x > camPos.x + camWidth / 2) // 玩家超出右侧
            {
                newCamPos.x += camWidth;
                needMove = true;
            }
            else if (playerPos.x < camPos.x - camWidth / 2) // 玩家超出左侧
            {
                newCamPos.x -= camWidth;
                needMove = true;
            }

            if (playerPos.y > camPos.y + camHeight / 2) // 玩家超出上侧
            {
                newCamPos.y += camHeight;
                needMove = true;
            }
            else if (playerPos.y < camPos.y - camHeight / 2) // 玩家超出下侧
            {
                newCamPos.y -= camHeight;
                needMove = true;
            }

            // 只有超出范围才移动摄像机
            if (needMove)
            {
                Vector3 targetPos = new Vector3(newCamPos.x, newCamPos.y, camPos.z);
                camera.transform.position = targetPos;
                Constants.Background.transform.position = targetPos;
            }
        }

    }
}
