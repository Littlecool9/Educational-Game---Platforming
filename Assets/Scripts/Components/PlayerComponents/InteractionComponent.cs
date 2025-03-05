using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class InteractionComponent : IComponent
    {
        // public bool IsInteractable { get; private set; }         // Find it in State Component

        // Record Interactables in Scene
        private List<InteractableComponent> Interactables = new List<InteractableComponent>();

        
        // Store Interaction result
        public void InitComponent()
        {
            
        }

        public void InitInteracables()
        {
            // interactableC.OnInteract += Interact;
            foreach (InteractableComponent interactable in Interactables) { 
                interactable.EnableInteraction += EnableInteraction;
                interactable.DisableInteraction += DisableInteraction;
            }
        }

        // Test Function
        public void Interact()
        {
            Debug.Log("Visiting Interaction Component");
        }

        public void AddInteractable(InteractableComponent interactableC)
        {
            Interactables.Add(interactableC);
        }

        private void EnableInteraction()
        {
            
        }

        private void DisableInteraction()
        {
            
        }

    }

}
