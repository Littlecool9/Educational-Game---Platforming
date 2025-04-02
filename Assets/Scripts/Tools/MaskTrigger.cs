using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MaskTrigger : MonoBehaviour
{
    private bool isDisappearing = false;
    public GameObject mask; // 拖入遮罩对象
    public bool removeOnEnter = true; // 是否进入时消失

    private void Start() {
        if (mask == null)
        {
            mask = gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && removeOnEnter && !isDisappearing)
        {
            StartCoroutine(FadeOutMask());
        }
    }

    public void RemoveMask()
    {
        if (isDisappearing)
        {
            StartCoroutine(FadeOutMask());
        }
    }

    private IEnumerator FadeOutMask()
    {
        isDisappearing = true;

        SpriteRenderer sr = mask.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        float t = 0f;
        float duration = 1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        mask.SetActive(false);
    }
}
