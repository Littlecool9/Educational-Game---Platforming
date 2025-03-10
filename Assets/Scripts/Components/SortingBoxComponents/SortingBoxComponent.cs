using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EducationalGame.Core;
using System;

namespace EducationalGame.Component
{
    [Serializable]
    public class SortingBoxComponent : IComponent        // Dangerous Code Warning
    {
        // Store Information about the Sorting Box
        public int index;     // Order of the Sorting Box, Adjusted in Inspector
        private int _slotIndex;
        public int slotIndex            // negative refers to temp slot, current slot
        {
            get { return _slotIndex; } // 访问私有字段
            set
            {
                if (_slotIndex != -1) previousSlotIndex = _slotIndex;
                _slotIndex = value; // 设置私有字段，避免无限递归
            }
        }
        public int previousSlotIndex;

        public bool swapable = false;

        // Render Related
        public Color initColor = new Color(1f,1f,1f);
        public Color enabledSwapColor = Color.green;
        public float distance = 1.5f;
        public float followSpeed = 5f;

        private BoxBridge boxBridge;

        public void InitComponent()
        {
            
        }

        public void SetOrder(int order) => this.index = order;
        public void SetBridge(BoxBridge bridge)
        {
            boxBridge = bridge;
            slotIndex = bridge.slotIndex;
            index = bridge.boxIndex;
            previousSlotIndex = bridge.slotIndex;
        }

        public void Reset() => SetBridge(boxBridge);

        public bool ResultCorrect(int placedIndex)
        {
            return this.index == placedIndex;
        }
    }
}


