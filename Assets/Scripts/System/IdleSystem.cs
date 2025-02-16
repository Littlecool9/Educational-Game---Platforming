using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class IdleSystem : MonoBehaviour, ISystem
    {
        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            var entityManager = EntityManager.Instance;
            foreach (Entity entity in entityManager.GetAllEntities())
            {
                if (entity is Player){
                    // Handle Player idle logic
                    
                }
                if (entityManager.HasComponent<StateComponent>(entity))
                {
                    
                    var state = entityManager.GetComponent<StateComponent>(entity);
                    // if (state.currentState == )
                    // {
                    //     // 处理 Idle 逻辑，如播放动画
                    // }
                }
            }
        }

    }

}
