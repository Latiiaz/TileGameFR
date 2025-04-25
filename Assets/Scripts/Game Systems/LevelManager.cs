using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float RestartTime = 1f;
    public float FadeInDuration = 0.3f;

    private bool rKeyDown = false;
    private float timeRKeyHeld = 0f;

    private Coroutine fadeCoroutine;
    private Coroutine timedGlowCoroutine;

    private float elapsedTime = 0f;
    private float nextGlowTime = 20f; // start glow at 20 seconds

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
        elapsedTime += Time.deltaTime;

        // Trigger timed glow at 20s, 30s, 40s, etc.
        if (elapsedTime >= nextGlowTime)
        {
            Debug.Log($"[GlowTrigger] Glow scheduled at {Time.time:F2}s (nextGlowTime = {nextGlowTime})");

            if (!rKeyDown)
            {
                Debug.Log("[GlowTrigger] Starting timed glow coroutine");

                if (timedGlowCoroutine != null) StopCoroutine(timedGlowCoroutine);
                timedGlowCoroutine = StartCoroutine(GlowEffect());
            }

            nextGlowTime += 10f;
        }

        RestartScene();

        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
    }

    public void LoadVictoryScene() => SceneManager.LoadScene("VictoryScene");
    public void LoadDefeatScene() => SceneManager.LoadScene("DefeatScene");
    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    public void Quitgame() => Application.Quit();

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

                CanvasGroup restartOverlay = GetRestartOverlay();  // Store the reference here
                if (restartOverlay != null)
                {
                    if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                    fadeCoroutine = StartCoroutine(FadeCanvasGroup(restartOverlay, 0f, 1f, FadeInDuration));  // Use the stored reference
                }
                else
                {
                    Debug.LogWarning("[RestartScene] Restart overlay not found.");
                }
            }

            timeRKeyHeld += Time.deltaTime;

            if (timeRKeyHeld >= FadeInDuration + RestartTime)
            {
                ReloadCurrentScene();
                rKeyDown = false;
                timeRKeyHeld = 0f;

                CanvasGroup restartOverlay = GetRestartOverlay();  // Store the reference here
                if (restartOverlay != null)
                {
                    if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                    fadeCoroutine = StartCoroutine(FadeCanvasGroup(restartOverlay, 1f, 0f, 0.2f));  // Use the stored reference
                }
                else
                {
                    Debug.LogWarning("[RestartScene] Restart overlay not found.");
                }
            }
        }
        else
        {
            if (rKeyDown)
            {
                rKeyDown = false;
                timeRKeyHeld = 0f;

                CanvasGroup restartOverlay = GetRestartOverlay();  // Store the reference here
                if (restartOverlay != null)
                {
                    if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                    fadeCoroutine = StartCoroutine(FadeCanvasGroup(restartOverlay, 1f, 0f, 0.2f));  // Use the stored reference
                }
                else
                {
                    Debug.LogWarning("[RestartScene] Restart overlay not found.");
                }
            }
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        Debug.Log($"[FadeCanvasGroup] Fading from {start:F2} to {end:F2} over {duration}s");

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        cg.alpha = end;
    }

    private IEnumerator GlowEffect()
    {
        Debug.Log($"[GlowEffect] Triggered at {Time.time:F2} seconds");

        CanvasGroup restartOverlay = GetRestartOverlay();
        if (restartOverlay == null)
        {
            Debug.LogWarning("[GlowEffect] Restart overlay not found.");
            yield break; // Exit if restartOverlay is not found
        }

        // Fade in (same as R key)
        yield return FadeCanvasGroup(restartOverlay, restartOverlay.alpha, 1f, FadeInDuration);

        // Hold full glow
        yield return new WaitForSeconds(RestartTime);

        // Fade out
        yield return FadeCanvasGroup(restartOverlay, restartOverlay.alpha, 0f, 0.2f);

        Debug.Log("[GlowEffect] Glow complete");
    }

    private CanvasGroup GetRestartOverlay()
    {
        GameObject restartButtonObj = GameObject.Find("Restart Button");
        if (restartButtonObj == null)
        {
            Debug.LogWarning("[GetRestartOverlay] GameObject 'Restart Button' not found.");
            return null;
        }

        CanvasGroup restartOverlay = restartButtonObj.GetComponent<CanvasGroup>();
        if (restartOverlay == null)
        {
            Debug.LogWarning("[GetRestartOverlay] CanvasGroup component missing on 'Restart Button'.");
            return null;
        }

        return restartOverlay;
    }

}
