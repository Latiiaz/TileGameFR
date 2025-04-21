using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float RestartTime = 1f; // Time after full fade-in to restart
    public float FadeInDuration = 0.3f;

    private bool rKeyDown = false;
    private float timeRKeyHeld = 0f;

    public CanvasGroup restartOverlay;
    private Coroutine fadeCoroutine;

    public bool IsRestarting => rKeyDown;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Fallback: Try to find overlay if not already assigned
        if (restartOverlay == null)
        {
            GameObject obj = GameObject.Find("Restart Button ");
            if (obj != null)
            {
                restartOverlay = obj.GetComponent<CanvasGroup>();
            }
        }

        RestartScene();

        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadMainMenu();
        }
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
        }
    }

    public void LoadVictoryScene() => SceneManager.LoadScene("VictoryScene");
    public void LoadDefeatScene() => SceneManager.LoadScene("DefeatScene");
    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    public void Quitgame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartScene()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (!rKeyDown)
            {
                rKeyDown = true;
                timeRKeyHeld = 0f;

                if (restartOverlay != null)
                {
                    if (fadeCoroutine != null)
                        StopCoroutine(fadeCoroutine);

                    fadeCoroutine = StartCoroutine(FadeCanvasGroup(restartOverlay, restartOverlay.alpha, 1f, FadeInDuration));
                }
            }

            timeRKeyHeld += Time.deltaTime;

            if (timeRKeyHeld >= FadeInDuration + RestartTime)
            {
                ReloadCurrentScene();
                rKeyDown = false;
                timeRKeyHeld = 0f;

                if (restartOverlay != null)
                {
                    if (fadeCoroutine != null)
                        StopCoroutine(fadeCoroutine);

                    fadeCoroutine = StartCoroutine(FadeCanvasGroup(restartOverlay, restartOverlay.alpha, 0f, 0.2f));
                }
            }
        }
        else
        {
            if (rKeyDown)
            {
                rKeyDown = false;
                timeRKeyHeld = 0f;

                if (restartOverlay != null)
                {
                    if (fadeCoroutine != null)
                        StopCoroutine(fadeCoroutine);

                    fadeCoroutine = StartCoroutine(FadeCanvasGroup(restartOverlay, restartOverlay.alpha, 0f, 0.2f));
                }
            }
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        cg.alpha = end;
    }
}
