using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContentManager : MonoBehaviour
{
    public static bool isTextInputActive = false;

    public static void UpdateParam(bool isTextInputActive) 
    { 
        GameContentManager.isTextInputActive = isTextInputActive; 
    }
}
