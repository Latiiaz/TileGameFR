using System.Collections;
using UnityEngine;

public class ExpressionsManager : MonoBehaviour
{
    [Header("Left Eye Variables")]
    [SerializeField] private SpriteRenderer _leftEyeRenderer;
    [SerializeField] private Sprite _defaultLeftEye;

    [Header("Right Eye Variables")]
    [SerializeField] private SpriteRenderer _rightEyeRenderer;
    [SerializeField] private Sprite _defaultRightEye;

    [Header("Shared Eye Variables")]
    [SerializeField] private Sprite _defaultSharedEye;
    [SerializeField] private Sprite _tearyEye;
    [SerializeField] private Sprite _cryEye;

    [Header("Mouth Variables")]
    [SerializeField] private GameObject _mouthObject;
    [SerializeField] private Sprite _defaultMouth;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    private Vector3 _lastPosition;
    private GameManager _gameManager;

    // New: cache the true original default eyes
    private Sprite _originalDefaultLeftEye;
    private Sprite _originalDefaultRightEye;

    void Start()
    {
        // Cache original defaults
        _originalDefaultLeftEye = _defaultLeftEye;
        _originalDefaultRightEye = _defaultRightEye;

        // Initialize expressions
        if (_leftEyeRenderer != null) _leftEyeRenderer.sprite = _defaultLeftEye;
        if (_rightEyeRenderer != null) _rightEyeRenderer.sprite = _defaultRightEye;

        if (playerTransform == null)
            Debug.LogWarning("Player Transform not assigned in ExpressionsManager.");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            Debug.LogWarning("GameManager not found by ExpressionsManager.");
    }

    void Update()
    {
        if (_gameManager == null) return;

        bool bothAlive = _gameManager._player.activeInHierarchy && _gameManager._tractor.activeInHierarchy;

        if (bothAlive)
        {
            // Restore original defaults if not already set
            if (_defaultLeftEye != _originalDefaultLeftEye || _defaultRightEye != _originalDefaultRightEye)
            {
                Debug.Log("Both alive again — restoring original default expressions.");
                _defaultLeftEye = _originalDefaultLeftEye;
                _defaultRightEye = _originalDefaultRightEye;
                ResetToDefaultExpression();
            }
        }
        else
        {
            // Switch default to teary if not already set
            if (_defaultLeftEye != _tearyEye || _defaultRightEye != _tearyEye)
            {
                Debug.Log("A character is dead — setting teary eyes as default.");
                _defaultLeftEye = _tearyEye;
                _defaultRightEye = _tearyEye;
                ResetToDefaultExpression();
            }
        }

        _lastPosition = playerTransform.position;
    }

    public void ExpressionPlayerDeath(float duration)
    {
        StartCoroutine(ExpressionPlayerDeathCoroutine(duration));
    }

    private IEnumerator ExpressionPlayerDeathCoroutine(float duration)
    {
        Debug.Log("Player death expression triggered.");

        if (_leftEyeRenderer != null)
        {
            _leftEyeRenderer.sprite = null;
            yield return null;
            _leftEyeRenderer.sprite = _cryEye;
        }

        if (_rightEyeRenderer != null)
        {
            _rightEyeRenderer.sprite = null;
            yield return null;
            _rightEyeRenderer.sprite = _cryEye;
        }

        if (_gameManager != null)
        {
            Debug.Log("Disabling input through GameManager.");
            _gameManager.DisableInputTemporarily(duration);
        }
        else
        {
            Debug.LogWarning("GameManager reference is null.");
        }

        yield return new WaitForSeconds(duration);

        // Manually reset to whatever the current default is (either teary or original)
        ResetToDefaultExpression();
    }



    public static void TriggerDeathExpressionAll(float duration)
    {
        ExpressionsManager[] all = FindObjectsOfType<ExpressionsManager>();
        foreach (ExpressionsManager em in all)
        {
            em.ExpressionPlayerDeath(duration);
        }
    }

    public void ResetToDefaultExpression()
    {
        if (_leftEyeRenderer != null) _leftEyeRenderer.sprite = _defaultLeftEye;
        if (_rightEyeRenderer != null) _rightEyeRenderer.sprite = _defaultRightEye;
        // You can add mouth or other parts here if needed
    }
}
