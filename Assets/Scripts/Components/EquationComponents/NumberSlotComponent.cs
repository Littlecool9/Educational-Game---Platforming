using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class NumberSlotComponent : IComponent
    {
        public int CurrentNumber { get; private set; }

        public bool IsResult = false;

        // Assigned from bridge
        private int _result;

        public int Result
        {
            get
            {
                if (!IsResult)
                {
                    throw new System.Exception("Not a result slot!");
                }

                return _result;
            }
            set => _result = value;
        }
        public int BinaryGroupNumber;
        // Dict structure: digital: 位数, value: binary
        private Dictionary<int, NumberSwitchComponent> Binaries = new Dictionary<int, NumberSwitchComponent>();
        private List<NumberVibeComponent> Vibes = new List<NumberVibeComponent>();
        

        private int digital = 0;

        
        public void InitComponent()
        {
            
        }

        public void AddBinary(NumberSwitchComponent binary)
        {
            Binaries.Add(digital++, binary);
        }

        private void UpdateNumber()
        {
            int decimalNumber = 0;

            foreach (var digit in Binaries)
            {
                if (digit.Value.CurrentBinary == Binary.One)
                {
                    decimalNumber += (int)Mathf.Pow(2, digit.Key);
                }
            }
            CurrentNumber = decimalNumber;
        }

        public void Init()
        {
            foreach (var binary in Binaries)
            {
                binary.Value.OnToggleBinary += UpdateNumber;
            }
        }

    }
}
