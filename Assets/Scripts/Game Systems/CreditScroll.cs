using UnityEngine;
using TMPro;
using System.Collections;

public class CreditScroll : MonoBehaviour
{
    [Header("Credits Settings")]
    [TextArea(5, 20)]
    public string creditsText;

    public TextMeshProUGUI creditTextObject;
    public float typingSpeed = 0.05f;
    public float displayDuration = 10f;

    private LevelManager _levelManager;

    private void Start()
    {
        _levelManager = LevelManager.Instance;

        if (creditTextObject != null)
        {
            creditTextObject.text = "";
            creditTextObject.alpha = 0f; // Make sure it's invisible initially
            StartCoroutine(TypeText());
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _levelManager.LoadMainMenu();
        }
    }

    private IEnumerator TypeText()
    {
        creditTextObject.text = "";
        creditTextObject.alpha = 1f; // Fully visible when typing starts
        yield return new WaitForSeconds(1f); // Optional small delay before typing

        foreach (char letter in creditsText)
        {
            creditTextObject.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeOutText());

        // Restart the process after fade
        StartCoroutine(TypeText());
    }

    private IEnumerator FadeOutText()
    {
        float timeElapsed = 0f;
        float duration = 1f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            creditTextObject.alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);
            yield return null;
        }

        creditTextObject.alpha = 0f;
    }
}
