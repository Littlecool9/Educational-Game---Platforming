using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common.GameUI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquationPuzzle))]
public class EquationPuzzleEditor : Editor
{

    #region SerializableCustomProperties
    SerializedProperty Bits;
    SerializedProperty Carry;
    SerializedProperty Sum;

    SerializedProperty EquationNumbers;
    // SerializedProperty EquationBitsPerNumber;
    SerializedProperty EquationBits;
    #endregion

    SerializedProperty TextObject;
    SerializedProperty Gates;
    SerializedProperty Masks;

    private void OnEnable() 
    {
        Bits = serializedObject.FindProperty("BitsObjects");
        Carry = serializedObject.FindProperty("CarryObject");
        Sum = serializedObject.FindProperty("SumObject");

        EquationNumbers = serializedObject.FindProperty("EquationNumbersObjects");
        EquationBits = serializedObject.FindProperty("EquationBitsObjects");

        TextObject = serializedObject.FindProperty("text");
        Gates = serializedObject.FindProperty("_gates");
        Masks = serializedObject.FindProperty("_mapMasks");
    }

    public override void OnInspectorGUI()
    {
        
        EquationPuzzle script = (EquationPuzzle)target;     // 获取目标对象
        serializedObject.Update();                          // 更新 Inspector 数据

        // 获取TextMeshPro
        EditorGUILayout.PropertyField(TextObject, new GUIContent("TextMeshPro"));


        // 创建一个勾选框
        script.isBinaryPuzzle = EditorGUILayout.Toggle("isBinaryPuzzle", script.isBinaryPuzzle);

        // 根据勾选框的状态显示不同的 Puzzle
        if (script.isBinaryPuzzle)
        {
            EditorGUILayout.PropertyField(Bits, new GUIContent("Bits"), true);
            EditorGUILayout.PropertyField(Carry, new GUIContent("Carry"), true);
            EditorGUILayout.PropertyField(Sum, new GUIContent("Sum"), true);
        }
        else
        {
            EditorGUILayout.PropertyField(EquationNumbers, new GUIContent("EquationNumbers"), true);

            int currentSize = EquationNumbers.arraySize;
            while (EquationBits.arraySize < currentSize)
            {
                EquationBits.InsertArrayElementAtIndex(EquationBits.arraySize);
            }
            while (EquationBits.arraySize > currentSize)
            {
                EquationBits.DeleteArrayElementAtIndex(EquationBits.arraySize - 1);
            }

            // 显示动态生成的子列表
            for (int i = 0; i < EquationBits.arraySize; i++)
            {
                SerializedProperty subList = EquationBits.GetArrayElementAtIndex(i).FindPropertyRelative("objects");
                EditorGUILayout.PropertyField(subList, new GUIContent($"Binaries of Element {i}"), true);
            }
            // EditorGUILayout.PropertyField(EquationBits, new GUIContent("EquationBits"), true);
        }
        
        // 获取Gate
        EditorGUILayout.PropertyField(Gates, new GUIContent("Gates"));

        // 获取Mask
        EditorGUILayout.PropertyField(Masks, new GUIContent("Masks"));

        serializedObject.ApplyModifiedProperties(); // 保存修改
    }
}
