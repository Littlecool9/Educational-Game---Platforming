using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform player; // 玩家角色的 Transform
    public float parallaxEffect = 0.5f; // 视差强度

    private float startPosX;

    void Start()
    {
        // startPosX = transform.position.x;
    }

    void Update()
    {
        if (player == null)
        {
            player = Constants.player.transform;
            startPosX = transform.position.x;
        }
        float distX = (player.position.x * parallaxEffect);
        transform.position = new Vector3(startPosX + distX, transform.position.y, transform.position.z);
    }
}
