using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EducationalGame.Core
{
    public interface ISystem
    {
        // The logic executed per frame
        void Update();  
        void Process();
    }
}
