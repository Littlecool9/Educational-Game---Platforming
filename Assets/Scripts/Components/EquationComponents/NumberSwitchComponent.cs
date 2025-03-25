using System;
using EducationalGame.Core;
using TMPro;

namespace EducationalGame.Component
{
    [Serializable]
    public enum Binary{ Zero, One }
    public class NumberSwitchComponent : IComponent
    {
        public bool isSum;
        public bool isCarry;
        public Binary TargetBinary;

        public Binary CurrentBinary { get; private set; }

        public TextMeshPro textMeshPro;


        public SwitchBridge bridge;

        public void InitComponent()
        {
            OnChangeBinary += UpdateText;
        }

        public void ToggleBinary()
        {
            CurrentBinary = CurrentBinary == Binary.Zero ? Binary.One : Binary.Zero;
            // OnToggleBinary?.Invoke();
            OnChangeBinary?.Invoke();
        }
        public void SetCurrentBinaryXOR(Binary binary1, Binary binary2)
        {
            CurrentBinary = binary1 == binary2 ? Binary.Zero : Binary.One;
            OnChangeBinary?.Invoke();
        }  
        public void SetCurrentBinaryAND(Binary binary1, Binary binary2)
        {
            CurrentBinary = binary1 == Binary.One && binary2 == Binary.One ? Binary.One : Binary.Zero;
            OnChangeBinary?.Invoke();
        }
        public event Action OnChangeBinary;

        /// <summary>
        /// Set initial status according to inspector setting
        /// </summary>
        public void SetBridge(SwitchBridge bridge)
        {
            this.bridge = bridge;
            this.isCarry = bridge.isCarry;
            this.isSum = bridge.isSum;
            this.CurrentBinary = bridge.InitialBinary;
            this.textMeshPro = bridge.text;

            if (isCarry || isSum) 
            {
                TargetBinary = bridge.TargetBinary;
                string addingText = isSum ? "Sum" : "Carry";
                string number = CurrentBinary == Binary.Zero ? "0" : "1";
                textMeshPro.text = addingText + ": " + number;
            }
            else
            {
                textMeshPro.text = CurrentBinary == Binary.Zero ? "0" : "1";
            }
        }

        public void UpdateText()
        {   
            string addingText = "";
            if (isCarry || isSum) addingText = isSum ? "Sum: " : "Carry: ";
            string number = CurrentBinary == Binary.Zero ? "0" : "1";
            
            textMeshPro.text = addingText + number;
        }

        public void Reset() => SetBridge(bridge);
    }
}
