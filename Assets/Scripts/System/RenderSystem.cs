using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class RenderSystem : ISystem
    {
        public void Init()
        {
            
        }

        public void Process()
        {
            
        }

        public void Update()
        {
            // Handle Objects' Animation Logic
            foreach (Entity entity in EntityManager.Instance.GetAllEntities())
            {
                if (entity is Player){
                    // Player Logic
                    RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);


                    // Update Animator
                    StateComponent stateC = EntityManager.Instance.GetComponent<StateComponent>(entity);
                    if (renderC == null || stateC == null) throw new Exception("Missing RenderComponent or StateComponent in RenderSystem");

                    if (stateC.CurrentState == PlayerState.Walking) renderC.SetAnimatorBool("Walking", true);
                    else renderC.SetAnimatorBool("Walking", false);

                    // Update Transform
                    InputComponent inputC = EntityManager.Instance.GetComponent<InputComponent>(entity);
                    if (inputC == null) throw new Exception("Missing InputComponent in RenderSystem");
                    
                    if (inputC.Facing != inputC.PreviousFacing)
                    {
                        renderC.Flip();
                    }

                }
            }
        }

    }
}