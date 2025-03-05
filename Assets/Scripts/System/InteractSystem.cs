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
            StateComponent stateC = EntityManager.Instance.GetComponent<StateComponent>(player.ID);
            InteractionComponent interactionC = EntityManager.Instance.GetComponent<InteractionComponent>(player.ID);

            if (stateC.CurrentState == PlayerState.Interacting)
            {
                bool isInteracting = false;
                foreach(Entity entity in EntityManager.Instance.GetAllEntities())
                {
                    // if (EntityManager.Instance.HasComponent<InteractableComponent>(entity))
                    if (entity is SortingBoxes)     // TODO: Edit when more interactable objects added
                    {
                        InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity);
                        if (interactableC.Interactable){
                            interactableC.InvokeInteractionEvent();
                            interactionC.Interact();

                            Debug.Log("Interacting");

                            
                            interactableC.Interactable = false;
                            isInteracting = true;
                            break;
                        }
                    }
                }
                if (!isInteracting) 
                {
                    // return to previous state
                    stateC.SetCurrentState(stateC.GetPreviousState());
                    Debug.Log("Not Interacting");
                }
            }
            
        }
    }
}
