using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class BoxSlotComponent : IComponent
    {
        public int index { get; private set; }
        public bool isPlaced;       // Updated in Interaction system
        public bool correctlyPlaced;        // Updated in Judgement system
        public Color incorrectColor = new Color(255f / 255f, 129f / 255f, 129f / 255f, 1f);
        public Color correctColor = new Color(1f,1f,1f);
        // public bool existBox { get; private set; }

        public void InitComponent()
        {
            
        }
        public void SetBridge(SlotBridge bridge)
        {
            isPlaced = bridge.isPlaced;
            index = bridge.index;
            correctlyPlaced = bridge.correctlyPlaced;
        }
        // public void SetExistBox(bool existBox) => this.existBox = existBox;
    }
}
