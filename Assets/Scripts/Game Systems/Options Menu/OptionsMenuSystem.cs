using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuSystem : MonoBehaviour
{
    public GameObject optionsMenu;

    private bool isGamePaused = false;
    public static bool IsOptionsMenuOpen { get; private set; } = false;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("dwnuajid");

            ToggleOptionsMenu();
        }
    }

    void ToggleOptionsMenu()
    {
        Debug.Log("dwnuajiddn23qhyijuod231n");

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
        isGamePaused = true;
        IsOptionsMenuOpen = true;
        Time.timeScale = 0f; 

    }

    void ResumeGame()
    {
        isGamePaused = false;
        IsOptionsMenuOpen = false;
        Time.timeScale = 1f; 
       
    }
}
