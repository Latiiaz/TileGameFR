using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;
    [SerializeField] private GameManager _gameManager;

    [Header("Camera Targets")]
    [SerializeField] private float lerpSpeed = 5f;
    private Transform _playerTransform;
    private Transform _tractorTransform;

    [Header("Shake Settings")]
    public AnimationCurve shakeCurve;
    public float shakeDuration = 0.3f;
    public float shakeStrength = 1.5f;
    private bool _isShaking = false;

    [Header("Zoom Settings")]
    private bool _hasStartedPPULerp = false;
    [SerializeField] private float finalFOV = 60f; // Perspective camera FOV instead

    private Vector3 _levelMidpoint = Vector3.zero;

    void Start()
    {
        _tileManager = FindObjectOfType<TileManager>();
        CenterOnLevelMidpoint();
        FindCharacters();
    }

    void Update()
    {
        CheckForRespawn();

        if (_playerTransform != null && _tractorTransform != null)
        {
            CenterOnBothCharacters();
        }
        else if (_playerTransform != null)
        {
            // Lerp to midpoint between level center and player if only player exists
            LerpToMidpointAndCharacter(_playerTransform);
        }
        else if (_tractorTransform != null)
        {
            // Lerp to midpoint between level center and tractor if only tractor exists
            LerpToMidpointAndCharacter(_tractorTransform);
        }

        if (Input.GetKeyDown(KeyCode.G) && !_isShaking)
        {
            StartCoroutine(ShakeCamera());
        }
    }

    private void CenterOnLevelMidpoint()
    {
        if (_tileManager != null)
        {
            float centerX = (_tileManager.GridWidth * _tileManager.TileSize) / 2f;
            float centerY = (_tileManager.GridHeight * _tileManager.TileSize) / 2f;
            _levelMidpoint = new Vector3(centerX, centerY, -15f);
            transform.position = _levelMidpoint;
        }
        else
        {
            Debug.LogWarning("TileManager is missing!");
        }
    }

    private void FindCharacters()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) _playerTransform = player.transform;

        GameObject tractor = GameObject.FindWithTag("Tractor");
        if (tractor != null) _tractorTransform = tractor.transform;
    }

    private void CheckForRespawn()
    {
        if (_playerTransform == null || _tractorTransform == null)
        {
            FindCharacters();
        }
    }

    private void CenterOnBothCharacters()
    {
        Vector3 midCharacterPos = (_playerTransform.position + _tractorTransform.position) / 2f;
        Vector3 target = (_levelMidpoint + midCharacterPos) / 2f;
        Vector3 lerped = Vector3.Lerp(transform.position, target, lerpSpeed * Time.deltaTime);
        transform.position = SnapToPixelGrid(new Vector3(lerped.x, lerped.y, -15f));

        if (!_hasStartedPPULerp)
        {
            StartCoroutine(LerpFOV(finalFOV, 1f));
            _hasStartedPPULerp = true;
        }
    }

    private void LerpToMidpointAndCharacter(Transform target)
    {
        // Lerp between the level midpoint and the character's position
        Vector3 targetPosition = (_levelMidpoint + target.position) / 2f;
        Vector3 lerpedPosition = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        transform.position = SnapToPixelGrid(new Vector3(lerpedPosition.x, lerpedPosition.y, -15f));
    }

    public IEnumerator ShakeCamera()
    {
        _isShaking = true;
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float strength = shakeCurve.Evaluate(elapsed / shakeDuration);
            transform.position = originalPos + Random.insideUnitSphere * strength * shakeStrength;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        _isShaking = false;
    }

    private Vector3 SnapToPixelGrid(Vector3 worldPosition)
    {
        float unitsPerPixel = 1f / _pixelPerfectCamera.assetsPPU;

        float snappedX = Mathf.Round(worldPosition.x / unitsPerPixel) * unitsPerPixel;
        float snappedY = Mathf.Round(worldPosition.y / unitsPerPixel) * unitsPerPixel;

        return new Vector3(snappedX, snappedY, worldPosition.z);
    }

    public IEnumerator LerpZoom(float targetSize, float duration)
    {
        float startSize = Camera.main.orthographicSize;
        float elapsed = 0f;

        _pixelPerfectCamera.pixelSnapping = false;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        Camera.main.orthographicSize = targetSize;
        _pixelPerfectCamera.pixelSnapping = true;
    }

    public IEnumerator LerpPPU(int targetPPU, float duration)
    {
        float startPPU = _pixelPerfectCamera.assetsPPU;
        float elapsed = 0f;

        _pixelPerfectCamera.pixelSnapping = false;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float interpolated = Mathf.Lerp(startPPU, targetPPU, t);
            _pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(interpolated);
            yield return null;
        }

        _pixelPerfectCamera.assetsPPU = targetPPU;
        _pixelPerfectCamera.pixelSnapping = true;
    }

    public IEnumerator LerpFOV(float targetFOV, float duration)
    {
        float startFOV = Camera.main.fieldOfView;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            Camera.main.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        Camera.main.fieldOfView = targetFOV;
    }
}
