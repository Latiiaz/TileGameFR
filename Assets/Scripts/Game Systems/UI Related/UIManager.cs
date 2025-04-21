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

    public void ShowEInteract(bool activateE) => _EInteract?.SetActive(activateE);
    public void ShowFInteract(bool activateF) => _FInteract?.SetActive(activateF);
}
