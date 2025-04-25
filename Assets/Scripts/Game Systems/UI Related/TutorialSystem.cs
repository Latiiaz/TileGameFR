using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialSystem : MonoBehaviour
{
    [Header("Settings")]
    public bool playOnStart = true;
    public GameObject tutorialUI;
    public GameObject spaceToCloseHint; // The hint UI element
    public TextMeshProUGUI tutorialText; // Reference to the TextMeshProUGUI component

    [TextArea(3, 10)]
    public string fullText = "Welcome to the mission! Let's get started..."; // Text to display

    [Header("Timing")]
    [SerializeField] private float tutorialStartDelay = 3f; // Delay before the tutorial appears
    [SerializeField] private float typeSpeed = 0.02f;       // Speed of text typing effect

    private bool isTutorialActive = false;
    private CanvasGroup hintCanvasGroup;

    void Start()
    {
        if (spaceToCloseHint != null)
        {
            spaceToCloseHint.SetActive(false);
            hintCanvasGroup = spaceToCloseHint.GetComponent<CanvasGroup>();
            if (hintCanvasGroup == null)
            {
                hintCanvasGroup = spaceToCloseHint.AddComponent<CanvasGroup>();
            }
        }

        if (playOnStart)
        {
            StartCoroutine(ShowTutorialWithDelay(tutorialStartDelay));
        }
    }

    void Update()
    {
        if (isTutorialActive)
        {
            PulseHint();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CloseTutorial();
            }
        }
    }

    public void TriggerTutorial()
    {
        StartCoroutine(ShowTutorialWithDelay(tutorialStartDelay));
    }

    private IEnumerator ShowTutorialWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowTutorial();
    }

    private void ShowTutorial()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(true);

        if (tutorialText != null)
        {
            StartCoroutine(TypeText());
        }

        isTutorialActive = true;
    }

    private IEnumerator TypeText()
    {
        tutorialText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            tutorialText.text += fullText[i];
            yield return new WaitForSeconds(typeSpeed);
        }

        // Once the text is fully typed, show the space-to-close hint
        StartCoroutine(ShowHintWithDelay(0.3f));
    }

    private IEnumerator ShowHintWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spaceToCloseHint != null)
            spaceToCloseHint.SetActive(true);
    }

    private void PulseHint()
    {
        if (spaceToCloseHint != null && spaceToCloseHint.activeSelf && hintCanvasGroup != null)
        {
            float alpha = Mathf.Lerp(0.2f, 1f, Mathf.PingPong(Time.time, 1f));
            hintCanvasGroup.alpha = alpha;
        }
    }

    private void CloseTutorial()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        if (spaceToCloseHint != null)
            spaceToCloseHint.SetActive(false);

        isTutorialActive = false;
        playOnStart = false;
    }
}
