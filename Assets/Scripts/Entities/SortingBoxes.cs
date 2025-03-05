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
        }

        public override void InitEntity()
        {
            // entityManager.AddComponent(this, new StateComponent());
            entityManager.AddComponent(this, new InteractableComponent());
            // entityManager.AddComponent(this, new InteractionComponent());

            foreach (IComponent component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
        }
    }
}
