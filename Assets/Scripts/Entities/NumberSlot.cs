using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class NumberSlot : Entity
    {
        // Number for Equation puzzles

        public override void InitEntity()
        {
            entityManager.AddComponent(this, new NumberSlotComponent());
            entityManager.AddComponent(this, new RenderComponent()); 

            foreach (IComponent component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
        }
    }
}
