using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trap : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("trigger");   
            other.gameObject.transform.position = respawnPoint.position;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.position = respawnPoint.position;
        }
    }
}
