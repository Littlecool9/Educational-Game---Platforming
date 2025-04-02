using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EducationalGame;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

public class LLMPuzzle : MonoBehaviour, IPuzzle
{
    public GameObject TypeConsoleObject;
    public SentenceObject sentenceObject;

    public Entity ConsoleEntity;
    public Entity SentenceEntity;

    public TMP_Text corpusField;
    public TMP_InputField inputField;
    public TMP_Text displayField;


    [SerializeField] private List<Gate> _gates;

    public List<Gate> Gates
    {
        get => _gates;
        set => _gates = value;
    }

    [SerializeField] private List<MaskTrigger> _mapMasks;
    public List<MaskTrigger> MapMasks 
    {
        get => _mapMasks;
        set => _mapMasks = value;
    }
    
    public List<Entity> GetEntities() => Entities;
    public List<Entity> Entities {
        get => new List<Entity> { ConsoleEntity, SentenceEntity };
        set => throw new NotSupportedException(); 
    }
    public bool triggered { get; set; }

    private Coroutine blinkingCursorCoroutine;
    public bool isInputEnabled { get; private set;}
    


    public List<InteractableComponent> Init()
    {
        List<InteractableComponent> interactables = new List<InteractableComponent>();
        
        
        ConsoleEntity = EntityManager.Instance.CreateEntity(EntityType.TypeConsole) as TypeConsole;

        InteractableComponent consoleIC = EntityManager.Instance.GetComponent<InteractableComponent>(ConsoleEntity);
        RenderComponent consoleRC = EntityManager.Instance.GetComponent<RenderComponent>(ConsoleEntity);

        consoleRC.SetGameObject(TypeConsoleObject);
        consoleIC.SetTrigger(consoleRC.trigger);

        consoleIC.EnableInteraction += RefreshTrigger;
        consoleIC.OnStayTrigger += RefreshTrigger;

        interactables.Add(consoleIC);
        

        SentenceEntity = EntityManager.Instance.CreateEntity(EntityType.Sentence) as IncompleteSentence;
        IncompleteSentenceComponent senC = EntityManager.Instance.GetComponent<IncompleteSentenceComponent>(SentenceEntity.ID);
        sentenceObject.LinkEntity(senC);

        foreach (string sen in sentenceObject.corpus)
        {
            corpusField.text += sen + "\n";
        }

        displayField.text = senC.incompleteSentence;


        return interactables;
    }

    private bool cursorVisible = true;

    // TODO: Update after abandon using TMP as playable
    # region Input Control
    public void EnableInput()
    {
        IncompleteSentenceComponent senC = EntityManager.Instance.GetComponent<IncompleteSentenceComponent>(SentenceEntity.ID);

        isInputEnabled = true;
        inputField.gameObject.SetActive(true);
        inputField.text = senC.incompleteSentence + " " + senC.currentInput;
        inputField.ActivateInputField();
        blinkingCursorCoroutine = StartCoroutine(BlinkingCursor());

    }
    public void UpdateInput() 
    {
        if (!inputField.gameObject.activeSelf) throw new Exception("Input field is not Enabled");
        IncompleteSentenceComponent senC = EntityManager.Instance.GetComponent<IncompleteSentenceComponent>(SentenceEntity.ID);

        senC.currentInput = inputField.text;
    }
    public void DisableInput()
    {
        if (!inputField.gameObject.activeSelf) throw new Exception("Input field is not Enabled");
        IncompleteSentenceComponent senC = EntityManager.Instance.GetComponent<IncompleteSentenceComponent>(SentenceEntity.ID);

        isInputEnabled = false;
        inputField.gameObject.SetActive(false);
        StopCoroutine(blinkingCursorCoroutine);
        displayField.text = senC.incompleteSentence + senC.currentInput;
    }
    # endregion

    IEnumerator BlinkingCursor()
    {
        IncompleteSentenceComponent senC = EntityManager.Instance.GetComponent<IncompleteSentenceComponent>(SentenceEntity.ID);

        while (true)
        {
            cursorVisible = !cursorVisible;
            string cursor = cursorVisible ? "_" : " ";
            displayField.text = senC.incompleteSentence + senC.currentInput + cursor;
            yield return new WaitForSeconds(0.5f);
        }
    }



    public void ResetPuzzle()
    {
        throw new NotImplementedException();
    }

    public void SolvePuzzle()
    {
        
    }


    public event Action OnDisableTrigger;
    public event Action OnEnableTrigger;
    public event Action OnSolvePuzzle;


    private Coroutine resetCoroutine;
    private float resetTime = 50f;

    public void DisableTrigger()
    {
        triggered = false;
        OnDisableTrigger?.Invoke();
    }

    public void RefreshTrigger()
    {
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
