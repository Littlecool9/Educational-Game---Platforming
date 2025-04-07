using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameContentManager : MonoBehaviour
{
    public static bool isTextInputActive = false;
    public static bool isInputEnabled = true;

    public static bool isPaused = false;

    private static bool isFinished = false;

    
    public static bool isTrapped = false;
    public static Vector3 transportTarget;


    public GameObject pauseMenuUI;
    public TextMeshPro MainText;
    public List<Trophy> trophies = new List<Trophy>();

    private float startTime;
    private float endTime = 0f;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        foreach (Trophy trophy in trophies)
        {
            if (!trophy.Collected) break;
            isFinished = true;
        }
        if (isFinished)
        {
            if (endTime == 0f) 
            {
                endTime = Time.time;
            }
            GameEnd();
        }
    }

    public static void UpdateIsTrapped(Vector3 transportPoint)
    {
        isTrapped = true;
        transportTarget = transportPoint;
    }

    public static void UpdateParam(bool isTextInputActive) 
    { 
        GameContentManager.isTextInputActive = isTextInputActive; 
    }

    void SetTextInputActivew(bool isActive)
    {
        isTextInputActive = isActive;
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        // 如果你之后需要显示暂停菜单，可以在这里调用 UI 的显隐
        pauseMenuUI.SetActive(isPaused);
    }

    private void GameEnd()
    {
        MainText.text = "You Win!\n Time Taken: " + (endTime - startTime).ToString("F2") +"s";
    }
}
