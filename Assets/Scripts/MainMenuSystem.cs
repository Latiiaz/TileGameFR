using System.Collections;
using System.Collections.Generic;
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

    [Header("Transform References for Particles")]
    [SerializeField] private Transform W_Transform;
    [SerializeField] private Transform A_Transform;
    [SerializeField] private Transform S_Transform;
    [SerializeField] private Transform D_Transform;

    [Header("Image Colors")]
    public Color defaultColor = Color.white;
    public Color pressedColor = Color.green;

    [Header("Key Press States")]
    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;
    private bool sceneLoadStarted = false;

    [Header("Scene Manager")]
    public LevelManager levelManager;
    [SerializeField] private string sceneName;

    [Header("Particle Growing")]
    [SerializeField] private ParticleSystem RingExpanding;

    void Start()
    {
        // Set all images and text to their default color
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
            Instantiate(RingExpanding, W_Transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.A) && !aPressed)
        {
            aPressed = true;
            A_Image.color = pressedColor;
            A_Text.color = pressedColor;
            Instantiate(RingExpanding, A_Transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.S) && !sPressed)
        {
            sPressed = true;
            S_Image.color = pressedColor;
            S_Text.color = pressedColor;
            Instantiate(RingExpanding, S_Transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.D) && !dPressed)
        {
            dPressed = true;
            D_Image.color = pressedColor;
            D_Text.color = pressedColor;
            Instantiate(RingExpanding, D_Transform.position, Quaternion.identity);
        }

        if (wPressed && aPressed && sPressed && dPressed && !sceneLoadStarted)
        {
            sceneLoadStarted = true;
            StartCoroutine(LoadSceneWithDelay(2.3f));
        }
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        levelManager.LoadSceneByName(sceneName);
    }
}
