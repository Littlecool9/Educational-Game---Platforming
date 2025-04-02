using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EducationalGame;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

public class EquationPuzzle : MonoBehaviour, IPuzzle
{

    // An instance of a algorithm puzzle

    [Serializable] public class GameObjectList
    {
        public List<GameObject> objects = new List<GameObject>(); // 这是实际的子列表
    }


    public TextMeshPro text;
    [SerializeField] public bool isBinaryPuzzle;

    #region Binary Puzzle Objects and Entities

    [Tooltip("Only the first 2 bits are valid")]
    [SerializeField] public List<GameObject> BitsObjects;   
    [SerializeField] public GameObject SumObject;
    [SerializeField] public GameObject CarryObject;

    public List<Entity> bitsEntities = new List<Entity>(2);
    public Entity carryEntity;
    public Entity sumEntity;
    #endregion

    public bool hasSum = false;
    public bool hasCarry = false;

    #region Equation Puzzle Objects and Entities
    
    [SerializeField] public List<GameObject> EquationNumbersObjects;
    [SerializeField] public List<GameObjectList> EquationBitsObjects;      
    // BitsObjects列表中每一项严格对应NumbersObjects列表中每一项的二进制表示（多位数），下面同理

    public List<Entity> equationNumbersEntities = new List<Entity>();
    public List<List<Entity>> equationBitsEntities = new List<List<Entity>>();
    #endregion


    public List<Entity> Entities { 
        get{
            if (isBinaryPuzzle) 
            {
                return bitsEntities.Concat(new List<Entity> { carryEntity, sumEntity }).ToList();
            } 
            else
            {
                List<Entity> entities = new List<Entity>();
                foreach (List<Entity> equationBits in equationBitsEntities)
                {
                    entities.AddRange(equationBits);
                }
                return equationNumbersEntities.Concat(entities).ToList();
            }
        }
        set => throw new NotSupportedException(); 
    }
    public List<Entity> GetEntities() => Entities;

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


    public bool triggered { get; set; }
    private Coroutine resetCoroutine;
    private float resetTime = 10f;


    public List<InteractableComponent> Init()
    {
        List<InteractableComponent> interactables = new List<InteractableComponent>();
        if (isBinaryPuzzle)
        {
            if (SumObject != null)
            {
                sumEntity = EntityManager.Instance.CreateEntity(EntityType.NumberSwitch);
                InitSwitch(sumEntity as NumberSwitch, SumObject);
                hasSum = true;
            }
            
            if (CarryObject != null)
            {
                carryEntity = EntityManager.Instance.CreateEntity(EntityType.NumberSwitch);
                InitSwitch(carryEntity as NumberSwitch, CarryObject);
                hasCarry = true;
            }

            // foreach (GameObject bit in BitsObjects)
            for (int i = 0; i < 2; i++)
            {
                GameObject bit = BitsObjects[i];
                NumberSwitch bitEntity = EntityManager.Instance.CreateEntity(EntityType.NumberSwitch) as NumberSwitch;
                InitSwitch(bitEntity, bit);
                bitsEntities.Add(bitEntity);
            }

            foreach (Entity entity in Entities)
            {
                InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity.ID);
                if (interactableC != null) interactables.Add(interactableC);
            }
        }
        else
        {
            if (EquationNumbersObjects.Count != EquationBitsObjects.Count) throw new Exception("EquationNumbersObjects and EquationBitsObjects must have the same length.");

            for(int i = 0; i < EquationNumbersObjects.Count; i++)
            {
                GameObject number = EquationNumbersObjects[i];
                NumberSlot numberEntity = EntityManager.Instance.CreateEntity(EntityType.NumberSlot) as NumberSlot;
                InitNumber(numberEntity, number);
                equationNumbersEntities.Add(numberEntity);

                NumberSlotComponent numberC = EntityManager.Instance.GetComponent<NumberSlotComponent>(numberEntity);

                List<Entity> bitsOfDecimal = new List<Entity>();

                if (!numberC.isFixed)
                {
                    foreach (GameObject bit in EquationBitsObjects[i].objects)
                    {
                        NumberSwitch bitEntity = EntityManager.Instance.CreateEntity(EntityType.NumberSwitch) as NumberSwitch;
                        InitSwitch(bitEntity, bit);
                        bitsOfDecimal.Add(bitEntity);

                        numberC.AddBitToBinaryList(EntityManager.Instance.GetComponent<NumberSwitchComponent>(bitEntity));

                        InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(bitEntity.ID);
                        if (interactableC != null) interactables.Add(interactableC);
                    }
                }
                equationBitsEntities.Add(bitsOfDecimal);
                numberC.Init();
            }
        }


        return interactables;
    }

    private void InitSwitch(NumberSwitch switchEntity, GameObject switchObject)
    {
        RenderComponent switchRC = EntityManager.Instance.GetComponent<RenderComponent>(switchEntity);
        NumberSwitchComponent switchC = EntityManager.Instance.GetComponent<NumberSwitchComponent>(switchEntity);
        InteractableComponent switchIC = EntityManager.Instance.GetComponent<InteractableComponent>(switchEntity);
        switchRC?.SetGameObject(switchObject);
        switchRC.bridge?.LinkEntity(switchC);
        switchIC?.SetTrigger(switchRC.trigger);

        if (switchC.isCarry || switchC.isSum)
        {
            // Disable interaction function
            switchIC.DisableComponent();
        }
        else
        {
            switchIC.EnableInteraction += RefreshTrigger;
            switchIC.OnStayTrigger += RefreshTrigger;
        }

    }

    private void InitNumber(NumberSlot slotEntity, GameObject slotObject)
    {
        RenderComponent slotRC = EntityManager.Instance.GetComponent<RenderComponent>(slotEntity);
        NumberSlotComponent slotC = EntityManager.Instance.GetComponent<NumberSlotComponent>(slotEntity);
        slotRC?.SetGameObject(slotObject);
        slotRC.bridge?.LinkEntity(slotC);
    }

    public event Action OnSolvePuzzle;

    public void SolvePuzzle()
    {
        OnSolvePuzzle?.Invoke();
        foreach (Gate gate in Gates)
        {
            gate.gameObject.SetActive(false);       // TODO: gate.StartDisappearing();
        }
        foreach (Entity entity in Entities)
        {
            InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity.ID);
            interactableC?.DisableComponent();
        }
        DisableTrigger();       // Unsubscribe objects
    }

    public void ResetPuzzle()
    {
        throw new NotImplementedException();
    }

    public event Action OnDisableTrigger;
    public event Action OnEnableTrigger;

    

    public void DisableTrigger()
    {
        triggered = false;
        OnDisableTrigger?.Invoke();
    }

    public void RefreshTrigger()
    {
        triggered = true;
        OnEnableTrigger?.Invoke();;

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
