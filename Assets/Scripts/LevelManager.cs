using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Might need to add some sort of cooldown between each restart so holding R doesnt immediately refresh it over and over again only for testing

    private static LevelManager _instance;

    public float RestartTime = 1f;
    bool rKeyDown = false;
    float timeRKeyDown = 0f;
    public float RestartCooldown = 0f; // Add cooldown

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        RestartScene();

    }
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadVictoryScene()
    {
        SceneManager.LoadScene("VictoryScene");
    }

    public void Quitgame() // remove Appliaction.Quit when the game is built otherwise just stick with Debug Log
    {
        Debug.Log("Quit game");
        //Application.Quit();
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
