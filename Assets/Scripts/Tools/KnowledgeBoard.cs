using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnowledgeBoard : MonoBehaviour
{
    public TextMeshPro textMeshPro; // 需要操作的TextMeshPro组件
    public float duration = 0.6f; // 动画持续时间
    public float offsetY = 50f; // 上升/下降的偏移量

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Color startColor = new Color(1f, 1f, 1f, 0f);
    private Color endColor = new Color(1f, 1f, 1f, 1f);
    private bool isMoving = false;

    void Start()
    {
        if (textMeshPro == null) textMeshPro = GetComponent<TextMeshPro>();

        // 记录初始位置
        startPosition = textMeshPro.transform.position;
        targetPosition = startPosition + new Vector3(0, offsetY, 0);
        textMeshPro.color = startColor;
    }

    public void RiseText()
    {
        // StartCoroutine(MoveText(startPosition, targetPosition));
        StartCoroutine(ChangeColor(startColor, endColor));
        
    }

    public void FallText()
    {
        
        // StartCoroutine(MoveText(targetPosition, startPosition));
        StartCoroutine(ChangeColor(endColor, startColor));
    }

    IEnumerator MoveText(Vector3 from, Vector3 to)
    {
        isMoving = true;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0, 1, t); // 缓动函数，模仿Premiere Pro缓入缓出
            textMeshPro.transform.position = Vector3.Lerp(from, to, t);
            // textMeshPro.color = Color.Lerp(fromColor, toColor, t);
            yield return null;
        }

        textMeshPro.transform.position = to; // 确保最终位置精确
        isMoving = false;
    }

    IEnumerator ChangeColor(Color fromColor, Color toColor) {
        isMoving = true;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0, 1, t); // 缓动函数，模仿Premiere Pro缓入缓出
            textMeshPro.color = Color.Lerp(fromColor, toColor, t);
            yield return null;
        }

        
        isMoving = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            RiseText();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            FallText();
        }
    }
}
