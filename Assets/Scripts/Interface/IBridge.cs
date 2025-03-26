using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;

/// <summary>
/// An Adapter from Unity Component to Custom Component
/// </summary>
public interface IBridge
{
    void LinkEntity(IComponent component);
}
