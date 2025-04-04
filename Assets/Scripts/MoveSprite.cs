using UnityEngine;
using System.Collections;

public class MoveSprite : MonoBehaviour
{
    [SerializeField] private float _floatingAmount = 1f;
    [SerializeField] public float _timeTaken = 2f;
    private Vector3 _originalPosition;
    private Coroutine _floatRoutine;

    void Start()
    {
        _originalPosition = transform.position;
        _floatRoutine = StartCoroutine(FloatRoutine());
    }

    private IEnumerator FloatRoutine()
    {
        while (true)
        {
            yield return MoveToPosition(_originalPosition + Vector3.up * _floatingAmount, _timeTaken / 2);
            yield return MoveToPosition(_originalPosition, _timeTaken / 2);
        }
    }

    public IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }

    public void StopFloating()
    {
        if (_floatRoutine != null)
        {
            StopCoroutine(_floatRoutine);
        }
    }
}
