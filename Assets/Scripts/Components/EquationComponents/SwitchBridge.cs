using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

public class SwitchBridge : MonoBehaviour, IBridge
{
    public bool isSum;
    public bool isCarry;

    [Tooltip("Initial Binary of Sum/Carry is set manualy")]
    [SerializeField]
    public Binary InitialBinary;
    
    public Binary TargetBinary;
    public TextMeshPro text;

    private int previousCheck = -1;     // -1: 未勾选, 0: isSum, 1: isCarry

    public void LinkEntity(IComponent component)
    {
        if (component is NumberSwitchComponent) LinkEntity(component as NumberSwitchComponent);
        else throw new System.Exception("Invalid Component Type");
    }

    private void LinkEntity(NumberSwitchComponent component) => component.SetBridge(this);

    private void OnValidate() 
    {
        
        if (isSum && isCarry)
        {
            if (previousCheck == 0) // 如果 A 先被勾选
            {
                isSum = false; // 取消 A
            }
            else if (previousCheck == 1) // 如果 B 先被勾选
            {
                isCarry = false; // 取消 B
            }
        }
        if (isSum != isCarry)  // 只有一个为 true
        {
            previousCheck = isSum ? 0 : 1;
        }
    }

}
