using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    // Adapter from Unity Collider to customed Collider Component
    public event Action<Collider2D> OnTriggerStayEvent;  // 触发器事件
    public event Action<Collider2D> OnTriggerEnterEvent;
    public event Action<Collider2D> OnTriggerExitEvent;

    private void OnTriggerStay2D(Collider2D other) {
        OnTriggerStayEvent?.Invoke(other);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log($"{gameObject.name} trigger {other.name}");
        OnTriggerEnterEvent?.Invoke(other);
    }
    private void OnTriggerExit2D(Collider2D other) {
        OnTriggerExitEvent?.Invoke(other);
    }
}
