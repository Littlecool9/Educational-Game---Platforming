using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public interface IPuzzle
{
    List<Entity> Entities { get; set; }

    List<Gate> Gates { get; set; }
    List<MaskTrigger> MapMasks { get; set; }

    List<InteractableComponent> Init();

    bool triggered { get; set; }

    void SolvePuzzle();
    void ResetPuzzle();
    List<Entity> GetEntities();

    event Action OnDisableTrigger;
    event Action OnEnableTrigger;
    event Action OnSolvePuzzle;
}
