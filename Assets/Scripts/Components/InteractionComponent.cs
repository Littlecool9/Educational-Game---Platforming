using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class InteractionComponent : IComponent
    {
        // Store Interaction result
        public void InitComponent()
        {
            
        }

        // Test Function
        public void Interact(){
            Debug.Log("Visiting Interaction Component");
        }
    }

}
