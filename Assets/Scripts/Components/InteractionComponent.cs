using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class InteractionComponent : IComponent
    {
        // 
        public object InteractableObject = null;
        public bool Isinteractable { get; private set;} = true;
        public void SetInteractable(bool interactable) => Isinteractable = interactable;
    }

}
