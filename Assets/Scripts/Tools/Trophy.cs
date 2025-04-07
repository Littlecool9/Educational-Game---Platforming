using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trophy : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform targetSlotPosition;
    public float moveSpeed = 5f;
    public float stopDistance = 0.1f;

    private bool isFlying = false;
    private Coroutine flyCoroutine;
    private Collider2D co;

    public bool Collected { get; private set; }

    private void Start() {
        co = GetComponent<Collider2D>();
        Collected = false;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            StartFlyToSlot(targetSlotPosition);
        }
    }

    public void StartFlyToSlot(Transform targetSlot)
    {
        if (isFlying)
        {
            StopCoroutine(flyCoroutine);
        }
        flyCoroutine = StartCoroutine(FlyToTarget(targetSlot));
    }

    private IEnumerator FlyToTarget(Transform target)
    {
        isFlying = true;

        while (Vector3.Distance(transform.position, target.position) > stopDistance)
        {
            // 位置插值
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);

            // // 旋转插值
            // Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            yield return null;
        }

        isFlying = false;
        co.enabled = false;
        Collected = true;
    }


}
