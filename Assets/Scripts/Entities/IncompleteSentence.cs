using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class IncompleteSentence : Entity
    {
        public string sentence;

        public override void InitEntity()
        {
            entityManager.AddComponent(this, new RenderComponent());
            entityManager.AddComponent(this, new IncompleteSentenceComponent());

            foreach (IComponent component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
        }
    }
}
