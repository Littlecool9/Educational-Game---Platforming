using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class SortingBoxSlot : Entity
    {
        public SortingBoxSlot() :base()
        {
        }

        public override void InitEntity()
        {
            entityManager.AddComponent(this, new BoxSlotComponent());
            entityManager.AddComponent(this, new InteractableComponent());
            entityManager.AddComponent(this, new RenderComponent());

            foreach (IComponent component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
        }
    }
}
