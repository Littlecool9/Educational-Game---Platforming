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

        /// <summary>
        /// true when player enter the object's trigger
        /// </summary>
        public bool Interactable{ get; private set; }
        // Activate when interacted
        // Comsumed in PuzzleState System
        public bool InteractedBuffer;    
        public event Action OnInteract;     // Triggered when interacted
        public event Action EnableInteraction;
        public event Action DisableInteraction;
        public event Action OnStayTrigger;      // Triggered when stay on trigger


        public void InitComponent()
        {
            Interactable = false;
            InteractedBuffer = false;
        }

        public void SetTrigger(Trigger GameObject){
            if (GameObject == null) throw new Exception("Missing Trigger in SetTrigger()");
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
            Interactable = true;
            OnStayTrigger?.Invoke();
            Debug.Log("OnStayTriggerEvent");
        }

        // 这两个方法可以被改为使用事件传递信息
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
