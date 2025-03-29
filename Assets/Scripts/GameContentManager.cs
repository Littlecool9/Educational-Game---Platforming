using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContentManager : MonoBehaviour
{
    public static bool isTextInputActive = false;

    void Awake()
    {
        // 保证只有一个 InputManager 实例
        if (FindObjectsOfType<GameContentManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public static void UpdateParam(bool isTextInputActive) 
    { 
        GameContentManager.isTextInputActive = isTextInputActive; 
    }
}
