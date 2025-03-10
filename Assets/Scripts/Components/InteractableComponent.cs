using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using System;

namespace EducationalGame.Component
{
    public class InteractableComponent : IComponent
    {
        // All interactable objects. Determine the state and the result of interaction result

        public Trigger triggerScript;
        public bool Interactable = false;   // true when player CAN interact with object
        // Activate when interacted
        // Comsumed in Judge System
        public bool InteractedBuffer = false;      
        public event Action OnInteract;     // Triggered when interacted
        public event Action EnableInteraction;
        public event Action DisableInteraction;
        public event Action OnStayTrigger;      // Triggered when stay on trigger


        public void InitComponent()
        {
            
        }

        public void SetTrigger(Trigger GameObject){
            triggerScript = GameObject;
            triggerScript.OnTriggerEnterEvent += EnterTrigger;
            triggerScript.OnTriggerExitEvent += ExitTrigger;
            triggerScript.OnTriggerStayEvent += OnStayTriggerEvent;
        }

        public void EnterTrigger(Collider2D other){
            Interactable = true;
            EnableInteraction?.Invoke();
        }
        public void ExitTrigger(Collider2D other){
            Interactable = false;
            DisableInteraction?.Invoke();
        }
        public void OnStayTriggerEvent(Collider2D other) 
        {
            OnStayTrigger?.Invoke();
        }

        // Activate when interacted or being interacted
        public void ActivateInteractionBuffer() => InteractedBuffer = true;
        // Deactivate when interacted or being interacted
        public void ComsumeInteractionBuffer() => InteractedBuffer = false;

        public void DisableComponent()
        {
            triggerScript.OnTriggerEnterEvent -= EnterTrigger;
            triggerScript.OnTriggerExitEvent -= ExitTrigger;
            triggerScript.OnTriggerStayEvent -= OnStayTriggerEvent;
            Interactable = false;
            InteractedBuffer = false;
        }

    }
}
