using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Might need to add some sort of cooldown between each restart so holding R doesnt immediately refresh it over and over again only for testing

    public static LevelManager Instance;
    public float RestartTime = 1f;
    bool rKeyDown = false;
    float timeRKeyDown = 0f;
    public float RestartCooldown = 0f; // Add cooldown

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        RestartScene();

    }
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log($"Load {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Loading scene index: {nextIndex}");
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("Next scene index is out of range!");
            // Optional: Reload current scene or go back to main menu
        }
    }
    public void LoadVictoryScene()
    {

        SceneManager.LoadScene("VictoryScene");
    }

    public void LoadDefeatScene()
    {
        SceneManager.LoadScene("DefeatScene");
    }
    public void Quitgame() // remove Appliaction.Quit when the game is built otherwise just stick with Debug Log
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Replace with your actual scene name
    }

    public void ReloadCurrentScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartScene() // Can change to other keys but for now
    {
        if (Input.GetKey(KeyCode.R))
        {
            if(!rKeyDown)
            {
                rKeyDown = true;
                timeRKeyDown = 0f; 
            }
            if (rKeyDown)
            {
                timeRKeyDown += Time.deltaTime;

                if (timeRKeyDown >= RestartTime)
                {
                    ReloadCurrentScene();
                }
            }
            else // restart counter if doesnt reach req  time
            {
                rKeyDown = false;
                timeRKeyDown = 0f;
            }
        }
    }
}
