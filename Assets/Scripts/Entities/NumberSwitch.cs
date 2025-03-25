using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class NumberSwitch : Entity
    {
        // Number for XOR and AND puzzles

        public override void InitEntity()
        {
            entityManager.AddComponent(this, new NumberSwitchComponent());
            entityManager.AddComponent(this, new InteractableComponent());

            foreach (IComponent component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
        }
    }
}
