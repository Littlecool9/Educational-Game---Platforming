using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public class BoxBridge : MonoBehaviour, IBridge
{
    // A bridge to receive set parameter from inspector
    
    [SerializeField]
    public int boxIndex;
    public int slotIndex;

    public void LinkEntity(IComponent component)
    {
        LinkEntity(component as SortingBoxComponent);
    }

    private void LinkEntity(SortingBoxComponent component)
    {
        component.SetBridge(this);
    }
}
