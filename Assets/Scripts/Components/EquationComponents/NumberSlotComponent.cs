using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

namespace EducationalGame.Component
{
    public class NumberSlotComponent : IComponent
    {
        // A number in the decimal equation
        public int CurrentNumber { get; private set; }

        public TextMeshPro text;
        public bool isFixed;

        // Link the set of binary representation
        // Dict structure: digital: 位数, value: binary
        private Dictionary<int, NumberSwitchComponent> Binaries = new Dictionary<int, NumberSwitchComponent>();
        private int digital = 0;

        private NumberBridge bridge;

        
        public void InitComponent()
        {
            
        }

        public void AddBitToBinaryList(NumberSwitchComponent binary)
        {
            Binaries.Add(digital++, binary);
        }

        private void UpdateNumber()
        {
            if (isFixed) throw new System.Exception("Fixed number cannot be changed.");
            int decimalNumber = 0;

            foreach (var digit in Binaries)
            {
                if (digit.Value.CurrentBinary == Binary.One)
                {
                    decimalNumber += (int)Mathf.Pow(2, digit.Key);
                }
            }
            CurrentNumber = decimalNumber;
            text.text = CurrentNumber.ToString();
        }

        public void Init()
        {
            foreach (var binary in Binaries) { binary.Value.OnChangeBinary += UpdateNumber; }
            UpdateNumber();
        }

        public void SetBridge(NumberBridge bridge)
        {
            this.bridge = bridge;
            this.CurrentNumber = bridge.InitialNumber;
            this.text = bridge.text;
            this.isFixed = bridge.isFixedNumber;
        }
    }
}
