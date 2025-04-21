using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCollection : MonoBehaviour
{
    [SerializeField] private Image _keyImage;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private ObjectiveSystem _objectiveSystem;
    private int _keyCount = 0;

    private void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        UpdateKeyDisplay();

    }


    public void KeyCollected()
    {
        UpdateKeyDisplay();
        StartCoroutine(AnimateKeyIcon());
    }

    private void UpdateKeyDisplay()
    {
        if (_textMeshProUGUI != null)
        {
            _textMeshProUGUI.text = $"{_objectiveSystem._objectiveCount} / {_objectiveSystem._neededObjectives}";
        }
    }

    private IEnumerator AnimateKeyIcon()
    {
        Vector3 originalScale = _keyImage.rectTransform.localScale;
        Vector3 targetScale = originalScale * 1.75f; // Punch size
        Vector3 squashScale = originalScale * 0.90f;

        float scaleUpDuration = 0.1f;
        float scaleDownDuration = 0.05f;

        Color originalColor = _keyImage.color;
        Color targetColor = Color.white;

        float elapsedTime = 0f;

        // Quick scale up + color shift to white
        while (elapsedTime < scaleUpDuration)
        {
            float t = elapsedTime / scaleUpDuration;
            _keyImage.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            _keyImage.color = Color.Lerp(originalColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _keyImage.rectTransform.localScale = targetScale;
        _keyImage.color = targetColor;

        // Scale down + color back to original
        elapsedTime = 0f;
        while (elapsedTime < scaleDownDuration)
        {
            float t = elapsedTime / scaleDownDuration;
            _keyImage.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            _keyImage.color = Color.Lerp(targetColor, originalColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _keyImage.rectTransform.localScale = squashScale;
        _keyImage.color = originalColor;

        // Tiny squash before reset for punchy feedback
        yield return new WaitForSeconds(0.05f);
        _keyImage.rectTransform.localScale = originalScale;
    }
}
