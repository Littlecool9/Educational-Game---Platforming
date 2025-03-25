using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EducationalGame;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public class EquationPuzzle : MonoBehaviour, IPuzzle
{

    // An instance of a algorithm puzzle

    [Serializable] public class GameObjectList
    {
        public List<GameObject> objects = new List<GameObject>(); // 这是实际的子列表
    }



    #region Binary Puzzle Objects and Entities
    [SerializeField] public bool isBinaryPuzzle;

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
    // [SerializeField] public List<List<GameObject>> EquationBitsPerNumber;       //
    [SerializeField] public List<GameObjectList> EquationBitsObjects;      // 

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

    [SerializeField] public List<GameObject> Gates;


    public bool triggered { get; set; }
    private Coroutine resetCoroutine;
    public float resetTime = 10f;


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
        }
        else
        {

        }


        return interactables;
    }

    private void InitSwitch(NumberSwitch switchEntity, GameObject switchObject)
    {
        RenderComponent switchRC = EntityManager.Instance.GetComponent<RenderComponent>(switchEntity);
        NumberSwitchComponent switchC = EntityManager.Instance.GetComponent<NumberSwitchComponent>(switchEntity);
        InteractableComponent switchIC = EntityManager.Instance.GetComponent<InteractableComponent>(switchEntity);
        switchRC?.SetGameObject(switchObject);
        switchC?.SetBridge(switchRC.switchBridge);
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
        slotC?.SetBridge(slotRC.numberBridge);
    }

    public event Action OnSolvePuzzle;

    public void SolvePuzzle()
    {
        OnSolvePuzzle?.Invoke();
        foreach (GameObject gate in Gates)
        {
            gate.SetActive(false);
        }
        foreach (Entity entity in Entities)
        {
            InteractableComponent interactableC = EntityManager.Instance.GetComponent<InteractableComponent>(entity.ID) ?? throw new Exception("Missing InteractableComponent in SolvePuzzle()");
            interactableC.DisableComponent();
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
