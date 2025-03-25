using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

public class NumberVibeComponent : IComponent
{
    // A link between the result number and the bit
    public int digital;         // digital of the bit
    public int resultSlot;

    public Color errorColor;
    public Color correctColor;

    public void InitComponent()
    {
        
    }

}
