using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

public class EquationPuzzle : MonoBehaviour, IPuzzle
{

    // An instance of a algorithm puzzle
    [Serializable]
    public class GameObjectList
    {
        public List<GameObject> objects = new List<GameObject>(); // 这是实际的子列表
    }

    [SerializeField] public bool isBinaryPuzzle;

    [SerializeField] public List<GameObject> Bits;
    [SerializeField] public GameObject Carry;
    [SerializeField] public GameObject Sum;

    [SerializeField] public List<GameObject> EquationNumbers;
    // [SerializeField] public List<List<GameObject>> EquationBitsPerNumber;       //
    [SerializeField] public List<GameObjectList> EquationBits;      // 

    public event Action OnDisableTrigger;
    public event Action OnEnableTrigger;
    public event Action OnSolvePuzzle;

    public List<Entity> Entities { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool triggered { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public List<InteractableComponent> Init()
    {
        throw new NotImplementedException();
    }

    public void SolvePuzzle()
    {
        throw new NotImplementedException();
    }

    public void ResetPuzzle()
    {
        throw new NotImplementedException();
    }

    public List<Entity> GetEntities() => Entities;
}
