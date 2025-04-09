using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuSystem : MonoBehaviour
{
    [Header("UI Key Images")]
    public Image W_Image;
    public Image A_Image;
    public Image S_Image;
    public Image D_Image;

    [Header("Text References (Child TMPs)")]
    private TextMeshProUGUI W_Text;
    private TextMeshProUGUI A_Text;
    private TextMeshProUGUI S_Text;
    private TextMeshProUGUI D_Text;

    [Header("Image Colors")]
    public Color defaultColor = Color.white;
    public Color pressedColor = Color.green;

    [Header("Shared Particle Effect")]
    public ParticleSystem sharedParticle;
    public Camera uiCamera; // Camera rendering the UI

    [Header("Key Press States")]
    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;

    [Header("Scene Manager")]
    public LevelManager levelManager;
    [SerializeField] string sceneName;

    void Start()
    {
        W_Image.color = defaultColor;
        A_Image.color = defaultColor;
        S_Image.color = defaultColor;
        D_Image.color = defaultColor;

        W_Text = W_Image.GetComponentInChildren<TextMeshProUGUI>();
        A_Text = A_Image.GetComponentInChildren<TextMeshProUGUI>();
        S_Text = S_Image.GetComponentInChildren<TextMeshProUGUI>();
        D_Text = D_Image.GetComponentInChildren<TextMeshProUGUI>();

        W_Text.color = defaultColor;
        A_Text.color = defaultColor;
        S_Text.color = defaultColor;
        D_Text.color = defaultColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !wPressed)
        {
            wPressed = true;
            W_Image.color = pressedColor;
            W_Text.color = pressedColor;
            PlayParticleAtUI(W_Image.rectTransform);
        }

        if (Input.GetKeyDown(KeyCode.A) && !aPressed)
        {
            aPressed = true;
            A_Image.color = pressedColor;
            A_Text.color = pressedColor;
            PlayParticleAtUI(A_Image.rectTransform);
        }

        if (Input.GetKeyDown(KeyCode.S) && !sPressed)
        {
            sPressed = true;
            S_Image.color = pressedColor;
            S_Text.color = pressedColor;
            PlayParticleAtUI(S_Image.rectTransform);
        }

        if (Input.GetKeyDown(KeyCode.D) && !dPressed)
        {
            dPressed = true;
            D_Image.color = pressedColor;
            D_Text.color = pressedColor;
            PlayParticleAtUI(D_Image.rectTransform);
        }

        if (wPressed && aPressed && sPressed && dPressed)
        {
            levelManager.LoadSceneByName(sceneName);
        }
    }

    void PlayParticleAtUI(RectTransform targetUI)
    {
        if (sharedParticle == null || uiCamera == null) return;

        // Convert UI position to world position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, targetUI.position);
        Ray ray = uiCamera.ScreenPointToRay(screenPoint);
        Vector3 spawnPosition = ray.origin + ray.direction * 5f; // Adjust depth as needed

        sharedParticle.transform.position = spawnPosition;
        sharedParticle.Play();
    }
}
