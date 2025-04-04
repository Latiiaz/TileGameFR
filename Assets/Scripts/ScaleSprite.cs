using UnityEngine;
using System.Collections;

public class ScaleSprite : MonoBehaviour
{
    [SerializeField] private float _scaleAmount = 0.1f;
    [SerializeField] private float _timeTaken = 2f;

    [SerializeField] private Vector3 _maxScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] private Vector3 _minScale = new Vector3(1f, 1f, 1f);

    private Vector3 _originalScale;
    private Coroutine _scaleRoutine;

    void Start()
    {
        _originalScale = transform.localScale;
        _scaleRoutine = StartCoroutine(ScaleRoutine());
    }

    private IEnumerator ScaleRoutine()
    {
        while (true)
        {
            yield return ScaleTo(_maxScale, _timeTaken / 2);
            yield return ScaleTo(_minScale, _timeTaken / 2);
        }
    }

    public IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void StopScaling()
    {
        if (_scaleRoutine != null)
        {
            StopCoroutine(_scaleRoutine);
            _scaleRoutine = null;
        }
    }
}
