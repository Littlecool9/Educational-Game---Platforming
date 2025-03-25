using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationPuzzle : MonoBehaviour
{
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

    
}
