using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;

namespace EducationalGame.Component
{
    public class SortingBoxComponent : IComponent        // Dangerous Code Warning
    {
        // Store Information about the Sorting Box


        public int order;     // Order of the Sorting Box, Adjusted in Inspector
        public bool CorrectlyPlaced { get; set; }

        // Render Related
        public float distance = 1.5f;
        public float followSpeed = 5f;

        public void InitComponent()
        {
            
        }

        public void SetCorrectlyPlaced(bool correctlyPlaced) => CorrectlyPlaced = correctlyPlaced;
        
    }
}


