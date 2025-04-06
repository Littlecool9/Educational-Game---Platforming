using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trap : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (respawnPoint == null) throw new System.Exception("Missing Respawn Point");
            GameContentManager.UpdateIsTrapped(respawnPoint.position);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (respawnPoint == null) throw new System.Exception("Missing Respawn Point");
            other.gameObject.transform.position = respawnPoint.position;
        }
    }
}
