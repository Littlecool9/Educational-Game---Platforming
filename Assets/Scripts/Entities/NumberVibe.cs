using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

public class NumberVibe : Entity
{
    public NumberVibe()
    {
        
    }

    public override void InitEntity()
    {
        entityManager.AddComponent(this, new NumberVibeComponent());
        entityManager.AddComponent(this, new RenderComponent());

        foreach (IComponent component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
    }
}
