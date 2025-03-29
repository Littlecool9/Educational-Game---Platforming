using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public class LLMPuzzle : MonoBehaviour, IPuzzle
{
    public List<Entity> Entities { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool triggered { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event Action OnDisableTrigger;
    public event Action OnEnableTrigger;
    public event Action OnSolvePuzzle;

    public List<Entity> GetEntities()
    {
        throw new NotImplementedException();
    }

    public List<InteractableComponent> Init()
    {
        throw new NotImplementedException();
    }

    public void ResetPuzzle()
    {
        throw new NotImplementedException();
    }

    public void SolvePuzzle()
    {
        throw new NotImplementedException();
    }
}
