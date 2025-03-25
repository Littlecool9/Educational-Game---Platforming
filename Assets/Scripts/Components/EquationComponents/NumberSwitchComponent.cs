using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public enum Binary{ Zero, One }
    public class NumberSwitchComponent : IComponent
    {
        public Binary CurrentBinary { get; private set; }
        public event Action OnToggleBinary;

        public void InitComponent()
        {
            
        }

        public void ToggleBinary()
        {
            CurrentBinary = CurrentBinary == Binary.Zero ? Binary.One : Binary.Zero;
            OnToggleBinary?.Invoke();
        }
    }
}
