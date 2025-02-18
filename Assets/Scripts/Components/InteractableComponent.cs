using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using System;
using System.Linq.Expressions;

namespace EducationalGame.Component
{
    public class InteractableComponent : IComponent
    {
        // All interactable objects. Determine the state and the result of interaction result

        public Trigger triggerScript;
        public bool Interactable = false;   // true when player CAN interact with object

        public event Action OnInteract;     // Triggered when interacted


        public void InitComponent()
        {
            
        }

        public void SetTrigger(Trigger GameObject){
            triggerScript = GameObject;
            triggerScript.OnTriggerEnterEvent += EnableTrigger;
            triggerScript.OnTriggerExitEvent += DisableTrigger;
        }

        public void EnableTrigger(Collider2D other){
            Debug.Log("Enable Interactable");
            Interactable = true;
        }
        public void DisableTrigger(Collider2D other){
            Debug.Log("Disable Interactable");
            Interactable = false;
        }

        public void InvokeInteractionEvent() => OnInteract?.Invoke();
    }
}
