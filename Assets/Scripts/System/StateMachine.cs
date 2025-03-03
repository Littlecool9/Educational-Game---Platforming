using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Component;
using EducationalGame.Core;

namespace EducationalGame
{
    public class StateMachine<T> : ISystem where T : System.Enum
    {
        public void Init()
        {
            
        }


        // State Machine
        public void Process()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            
        }
    }
}
