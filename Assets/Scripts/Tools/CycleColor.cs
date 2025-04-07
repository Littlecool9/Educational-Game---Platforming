using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleColor : MonoBehaviour
{
    // 控制颜色变化速度
    [Range(0f, 1f)]
    public float colorChangeSpeed = 0.2f;

    private SpriteRenderer spriteRenderer;
    private float hue = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 根据时间变化循环色环 (HSV模式中Hue取值为0~1)
        hue += colorChangeSpeed * Time.deltaTime;
        if (hue > 1f)
            hue -= 1f;

        // HSV转RGB，设置饱和度和亮度为1
        spriteRenderer.color = Color.HSVToRGB(hue, 1f, 1f);
    }
}
