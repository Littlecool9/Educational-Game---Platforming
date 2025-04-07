using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public abstract class PuzzleBase : MonoBehaviour
{
    public virtual List<Entity> Entities { get; set; }

    public virtual List<Gate> Gates { get; set; }
    public virtual List<MaskTrigger> MapMasks { get; set; }

    public abstract List<InteractableComponent> Init();

    // Trigger the puzzle, signs this puzzle is on
    [HideInInspector] public bool triggered;

    private Coroutine resetCoroutine;
    private float resetTime = 10f;

    public virtual void SolvePuzzle()
    {
        OnSolvePuzzle?.Invoke();

        // Remove objects
        foreach (Gate gate in Gates)
        {
            // gate.gameObject.SetActive(false);       // TODO: gate.StartDisappearing();
            gate.StartDisappearing();
        }
        foreach (MaskTrigger mask in MapMasks)
        {
            mask.RemoveMask();
        }

        // Disable interaction
        foreach (Entity entity in Entities)
        {
            InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity.ID);
            interactableC?.DisableComponent();
        }
        DisableTrigger();       // Unsubscribe objects
    }

    public virtual void ResetPuzzle()
    {
        
    }
    public List<Entity> GetEntities() => Entities;

    public event Action OnDisableTrigger;
    public event Action OnEnableTrigger;
    public event Action OnSolvePuzzle;

    protected void DisableTrigger()
    {
        triggered = false;
        OnDisableTrigger?.Invoke();
    }

    protected void RefreshTrigger()
    {
        Debug.Log("refresh trigger");
        triggered = true;
        OnEnableTrigger?.Invoke();

        // 如果之前有倒计时协程在运行，先停止它
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        // 启动新的倒计时协程
        resetCoroutine = StartCoroutine(ResetTriggeredAfterDelay());
    }

    private IEnumerator ResetTriggeredAfterDelay()
    {
        yield return new WaitForSeconds(resetTime);
        DisableTrigger();
    }

}
