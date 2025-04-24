using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Added to listen for scene load events

public class OptionsMenuSystem : MonoBehaviour
{
    public GameObject optionsMenu;

    private bool isGamePaused = false;
    public static bool IsOptionsMenuOpen { get; private set; } = false;

    void Start()
    {
        // Reset the options menu state when a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[OptionsMenuSystem] Scene loaded. Resetting options menu state.");

        // Ensure static flag is reset when scene is loaded
        IsOptionsMenuOpen = false;

        // Reset the menu state after the scene is loaded
        optionsMenu.SetActive(false);  // Ensure the menu is hidden
        ResumeGame();
        Time.timeScale = 1f;           // Unpause the game if it was paused

        // Optionally, you could reset any other things tied to the menu state here
    }

    void Update()
    {
        // Make sure the toggle action works after the scene loads
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[OptionsMenuSystem] Escape key pressed");
            ToggleOptionsMenu();
        }
    }

    public void ToggleOptionsMenu()
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
        Debug.Log("[OptionsMenuSystem] Game paused.");
        isGamePaused = true;
        IsOptionsMenuOpen = true;
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        Debug.Log("[OptionsMenuSystem] Game resumed.");
        isGamePaused = false;
        IsOptionsMenuOpen = false;
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        // Unsubscribe from scene loaded events when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public static void ResetMenuFlag()
    {
        IsOptionsMenuOpen = false;
    }
}
