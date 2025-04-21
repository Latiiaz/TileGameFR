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

    [SerializeField] private GameObject _leftEyeObject;
    [SerializeField] private GameObject _rightEyeObject;

    private Vector3 _lastPosition;
    private GameManager _gameManager;

    private Sprite _originalDefaultLeftEye;
    private Sprite _originalDefaultRightEye;

    private Vector3 _leftEyeDefaultPosition;
    private Vector3 _rightEyeDefaultPosition;
    private Quaternion _leftEyeDefaultRotation;
    private Quaternion _rightEyeDefaultRotation;

    private Vector3 _leftEyeTargetPos;
    private Vector3 _rightEyeTargetPos;
    private Quaternion _leftEyeTargetRot;
    private Quaternion _rightEyeTargetRot;

    [SerializeField] private float _eyeLerpSpeed = 10f;
    private bool _isMoving = false;

    void Start()
    {
        if (_leftEyeObject != null)
        {
            _leftEyeDefaultPosition = _leftEyeObject.transform.localPosition;
            _leftEyeDefaultRotation = _leftEyeObject.transform.localRotation;
        }

        if (_rightEyeObject != null)
        {
            _rightEyeDefaultPosition = _rightEyeObject.transform.localPosition;
            _rightEyeDefaultRotation = _rightEyeObject.transform.localRotation;
        }

        _leftEyeTargetPos = _leftEyeDefaultPosition;
        _rightEyeTargetPos = _rightEyeDefaultPosition;
        _leftEyeTargetRot = _leftEyeDefaultRotation;
        _rightEyeTargetRot = _rightEyeDefaultRotation;

        _originalDefaultLeftEye = _defaultLeftEye;
        _originalDefaultRightEye = _defaultRightEye;

        if (_leftEyeRenderer != null)
        {
            _leftEyeRenderer.sprite = _defaultLeftEye;
        }

        if (_rightEyeRenderer != null)
        {
            _rightEyeRenderer.sprite = _defaultRightEye;
        }

        if (playerTransform == null)
            Debug.LogWarning("Player Transform not assigned in ExpressionsManager.");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            Debug.LogWarning("GameManager not found by ExpressionsManager.");
    }

    void Update()
    {
        if (_gameManager == null || playerTransform == null) return;

        bool bothAlive = _gameManager._player.activeInHierarchy && _gameManager._tractor.activeInHierarchy;

        if (bothAlive)
        {
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
            if (_defaultLeftEye != _tearyEye || _defaultRightEye != _tearyEye)
            {
                Debug.Log("A character is dead — setting teary eyes as default.");
                _defaultLeftEye = _tearyEye;
                _defaultRightEye = _tearyEye;
                ResetToDefaultExpression();
            }
        }

        Vector3 direction = playerTransform.position - _lastPosition;
        _isMoving = direction.magnitude > 0.01f;

        if (_isMoving)
        {
            TiltEyes(direction);
        }
        else
        {
            ResetEyeTargetsToDefault();
        }

        LerpEyes();
        _lastPosition = playerTransform.position;
    }

    private void TiltEyes(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // LEFT or RIGHT — rotate Z
            float tilt = direction.x > 0 ? -40f : 40f;
            _leftEyeTargetRot = Quaternion.Euler(0, 0, tilt);
            _rightEyeTargetRot = Quaternion.Euler(0, 0, tilt);
            _leftEyeTargetPos = _leftEyeDefaultPosition;
            _rightEyeTargetPos = _rightEyeDefaultPosition;
        }
        else
        {
            // UP or DOWN — vertical shift
            float offset = direction.y > 0 ? 0.1f : -0.1f;
            Quaternion verticalRotation = direction.y > 0 ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

            _leftEyeTargetRot = verticalRotation;
            _rightEyeTargetRot = verticalRotation;
            _leftEyeTargetPos = _leftEyeDefaultPosition + new Vector3(0, offset, 0);
            _rightEyeTargetPos = _rightEyeDefaultPosition + new Vector3(0, offset, 0);
        }
    }

    private void ResetEyeTargetsToDefault()
    {
        _leftEyeTargetPos = _leftEyeDefaultPosition;
        _rightEyeTargetPos = _rightEyeDefaultPosition;
        _leftEyeTargetRot = _leftEyeDefaultRotation;
        _rightEyeTargetRot = _rightEyeDefaultRotation;
    }

    private void LerpEyes()
    {
        if (_leftEyeObject != null)
        {
            _leftEyeObject.transform.localPosition = Vector3.Lerp(_leftEyeObject.transform.localPosition, _leftEyeTargetPos, Time.deltaTime * _eyeLerpSpeed);
            _leftEyeObject.transform.localRotation = Quaternion.Lerp(_leftEyeObject.transform.localRotation, _leftEyeTargetRot, Time.deltaTime * _eyeLerpSpeed);
        }

        if (_rightEyeObject != null)
        {
            _rightEyeObject.transform.localPosition = Vector3.Lerp(_rightEyeObject.transform.localPosition, _rightEyeTargetPos, Time.deltaTime * _eyeLerpSpeed);
            _rightEyeObject.transform.localRotation = Quaternion.Lerp(_rightEyeObject.transform.localRotation, _rightEyeTargetRot, Time.deltaTime * _eyeLerpSpeed);
        }
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
        if (_leftEyeRenderer != null)
        {
            _leftEyeRenderer.sprite = _defaultLeftEye;
        }

        if (_rightEyeRenderer != null)
        {
            _rightEyeRenderer.sprite = _defaultRightEye;
        }

        ResetEyeTargetsToDefault();

        if (_defaultLeftEye == _tearyEye || _defaultLeftEye == _cryEye ||
            _defaultRightEye == _tearyEye || _defaultRightEye == _cryEye)
        {
            FlipMouth();
        }
        else
        {
            ResetMouth();
        }
    }

    public void FlipMouth()
    {
        if (_mouthObject != null)
        {
            _mouthObject.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else
        {
            Debug.LogWarning("Mouth object not assigned in ExpressionsManager.");
        }
    }

    public void ResetMouth()
    {
        if (_mouthObject != null)
        {
            _mouthObject.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Mouth object not assigned in ExpressionsManager.");
        }
    }
}
