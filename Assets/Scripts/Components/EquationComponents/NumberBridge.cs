using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

public class NumberBridge : MonoBehaviour, IBridge
{
    public int InitialNumber;
    public TextMeshPro text;
    public bool isFixedNumber;

    public void LinkEntity(IComponent component)
    {
        if (component is NumberSlotComponent) LinkEntity(component as NumberSlotComponent);
        else throw new System.Exception("Invalid Component Type");
    }

    private void LinkEntity(NumberSlotComponent component)
    {
        component.SetBridge(this);
    }
}
