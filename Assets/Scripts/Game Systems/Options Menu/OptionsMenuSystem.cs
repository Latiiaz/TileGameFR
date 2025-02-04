using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuSystem : MonoBehaviour
{
    public GameObject optionsMenu;

    private bool isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsMenu();
        }
    }

    void ToggleOptionsMenu()
    {
        bool isActive = !optionsMenu.activeSelf;
        optionsMenu.SetActive(isActive);

        if (isActive)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; 
        isGamePaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; 
        isGamePaused = false;
    }
}
