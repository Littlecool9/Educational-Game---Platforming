using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using EducationalGame.Component;

namespace EducationalGame
{
    public class InteractSystem : ISystem
    {
        Player player;
        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
        }

        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            
            InputComponent inputC = EntityManager.Instance.GetComponent<InputComponent>(player.ID);
            StateComponent stateC = EntityManager.Instance.GetComponent<StateComponent>(player.ID);
            if (inputC.InteractInput && (stateC.CurrentState == PlayerState.Idle || stateC.CurrentState == PlayerState.Walking))
            {
                bool isInteracting = false;
                foreach(Entity entity in EntityManager.Instance.GetAllEntities())
                {
                    if (EntityManager.Instance.HasComponent<InteractableComponent>(entity)
                    && EntityManager.Instance.HasComponent<InteractionComponent>(entity))
                    {
                        InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                        InteractionComponent interactionC = EntityManager.Instance.GetComponent<InteractionComponent>(entity);
                        if (interactableC.Interactable){
                            interactableC.InvokeInteractionEvent();
                            interactionC.Interact();
                            Debug.Log("Interacting");


                            isInteracting = true;
                            break;
                        }
                    }
                }
                if (!isInteracting) Debug.Log("Not Interacting");
            }
            
        }
    }
}
