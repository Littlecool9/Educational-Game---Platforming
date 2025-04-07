using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameContentManager : MonoBehaviour
{
    public static bool isTextInputActive = false;
    public static bool isInputEnabled = true;

    public static bool isPaused = false;

    
    public static bool isTrapped = false;
    public static Vector3 transportTarget;


    public GameObject pauseMenuUI;
    public List<Trophy> trophies = new List<Trophy>();

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        bool success = false;
        foreach (Trophy trophy in trophies)
        {
            if (!trophy.Collected) break;
            success = true;
        }
        if (success) 
        {
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
        Debug.Log("1");
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        // 如果你之后需要显示暂停菜单，可以在这里调用 UI 的显隐
        pauseMenuUI.SetActive(isPaused);
    }

    private void GameEnd()
    {

    }
}
