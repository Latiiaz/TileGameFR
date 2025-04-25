using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditSceneEffectScript : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image blackOverlay;         // The Image UI component you want to fade (should be full black initially)
    public float fadeDelay = 2f;       // Seconds to wait before fading starts
    public float fadeDuration = 1f;    // Seconds it takes to fade out

    private void Start()
    {
        if (blackOverlay != null)
        {
            StartCoroutine(FadeOutBlackOverlay());
        }
    }

    private IEnumerator FadeOutBlackOverlay()
    {
        // Optional: Wait before starting fade
        yield return new WaitForSeconds(fadeDelay);

        Color startColor = blackOverlay.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            blackOverlay.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        blackOverlay.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
