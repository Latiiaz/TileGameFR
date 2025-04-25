using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private GameObject _EInteract;
    private GameObject _FInteract;

    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Restart Visual Feedback")]
    [SerializeField] private Image restartVisualOverlay; // This is your Image UI element
    private float restartOverlayOriginalAlpha = 0f;

    public int SceneLoadCount { get; private set; } = 0;
    private float _timeElapsed = 0f;

    // Glow effect settings
    private float nextGlowTime = 20f;
    private Coroutine glowCoroutine;
    [SerializeField] private float glowFadeInDuration = 0.3f;
    [SerializeField] private float glowHoldDuration = 1f;
    [SerializeField] private float glowFadeOutDuration = 0.2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (restartVisualOverlay == null)
        {
            GameObject found = GameObject.Find("RestartVisualOverlay");
            if (found != null)
                restartVisualOverlay = found.GetComponent<Image>();
        }

        if (restartVisualOverlay != null)
            restartOverlayOriginalAlpha = restartVisualOverlay.color.a;
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(_timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(_timeElapsed % 60f);

        if (_timerText != null)
            _timerText.text = $"{minutes:00}:{seconds:00}";

        UpdateRestartVisual();

        // Trigger glow every 10 seconds after 20 seconds
        if (_timeElapsed >= nextGlowTime)
        {
            if (glowCoroutine != null)
                StopCoroutine(glowCoroutine);

            glowCoroutine = StartCoroutine(GlowEffect());
            nextGlowTime += 10f;
        }
    }

    private void UpdateRestartVisual()
    {
        if (restartVisualOverlay == null) return;

        bool isRestarting = LevelManager.Instance != null && LevelManager.Instance.IsRestarting;

        Color currentColor = restartVisualOverlay.color;
        float targetAlpha = isRestarting ? 1f : restartOverlayOriginalAlpha;
        float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * 10f);

        restartVisualOverlay.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }

    private IEnumerator GlowEffect()
    {
        if (restartVisualOverlay == null) yield break;

        Color color = restartVisualOverlay.color;

        // Fade in
        float t = 0f;
        while (t < glowFadeInDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(restartOverlayOriginalAlpha, 1f, t / glowFadeInDuration);
            restartVisualOverlay.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Hold full glow
        restartVisualOverlay.color = new Color(color.r, color.g, color.b, 1f);
        yield return new WaitForSeconds(glowHoldDuration);

        // Fade out
        t = 0f;
        while (t < glowFadeOutDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, restartOverlayOriginalAlpha, t / glowFadeOutDuration);
            restartVisualOverlay.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        restartVisualOverlay.color = new Color(color.r, color.g, color.b, restartOverlayOriginalAlpha);
    }

    public void ShowEInteract(bool activateE) => _EInteract?.SetActive(activateE);
    public void ShowFInteract(bool activateF) => _FInteract?.SetActive(activateF);
}
