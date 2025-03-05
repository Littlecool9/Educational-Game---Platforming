using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class BoxSlotComponent : IComponent
    {
        public int index { get; private set; }
        

        public void InitComponent()
        {
            
        }
        public void SetIndex(int index) => this.index = index;
    }
}
