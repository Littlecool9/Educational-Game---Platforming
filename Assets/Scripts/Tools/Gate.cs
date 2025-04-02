using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float duration = 1f; // 动画持续时间
    private bool isDisappearing = false;
    private Vector3 initialScale;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
    }

    public void StartDisappearing()
    {
        if (!isDisappearing)
        {
            StartCoroutine(DisappearCoroutine());
        }
    }

    private IEnumerator DisappearCoroutine()
    {
        isDisappearing = true;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 缩小
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

            // 渐隐
            if (sr != null)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, 1f - t);
            }

            yield return null;
        }

        Destroy(gameObject); // 彻底删除（可改为 SetActive(false)）
    }
}
