using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class SortingBoxes : Entity
    {
        private EntityManager entityManager = EntityManager.Instance;
        public SortingBoxes() : base()
        {
            entityManager.AddComponent<InteractableComponent>(this, new InteractableComponent());
        }

        public override void InitComponents()
        {
            
        }
    }
}
