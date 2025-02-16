using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class InteractionSystem : ISystem
    {
        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            EntityManager entityManager = EntityManager.Instance;
            foreach (Entity entity in entityManager.GetAllEntities())
            {
                if (entity is Player){
                    
                }
                if (entityManager.HasComponent<StateComponent>(entity) &&
                    entityManager.HasComponent<InteractionComponent>(entity))
                {
                    var state = entityManager.GetComponent<StateComponent>(entity);
                    var interaction = entityManager.GetComponent<InteractionComponent>(entity);

                    // if (state.CurrentState == PlayerState.Interacting && interaction.CanInteract)
                    // {
                    //     // 触发交互逻辑
                    //     Debug.Log("Player interacting with object: " + interaction.InteractableObject);
                    // }
                }
            }
        }
    }

}
