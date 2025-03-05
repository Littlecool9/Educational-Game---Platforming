using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace EducationalGame.Component
{
    [Serializable]
    public class SortingBoxComponent : IComponent        // Dangerous Code Warning
    {
        // Store Information about the Sorting Box


        public int index;     // Order of the Sorting Box, Adjusted in Inspector
        public bool CorrectlyPlaced { get; set; }

        // Render Related
        public float distance = 1.5f;
        public float followSpeed = 5f;

        public void InitComponent()
        {
            
        }

        public void SetOrder(int order) => this.index = order;


        public bool ResultCorrect(int placedIndex)
        {
            return this.index == placedIndex;
        }
        
        
    }
}


