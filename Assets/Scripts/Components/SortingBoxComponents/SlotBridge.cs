using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public class SlotBridge : MonoBehaviour, IBridge
{
    // A bridge to receive set parameter from inspector
    
    public int index;
    public bool isPlaced;
    public bool isTempSlot;
    public bool correctlyPlaced;

    public void LinkEntity(IComponent component)
    {
        LinkEntity(component as BoxSlotComponent);
    }
    private void LinkEntity(BoxSlotComponent component)
    {
        component.SetBridge(this);
    }
}
