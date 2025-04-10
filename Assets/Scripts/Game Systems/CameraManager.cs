using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    [Header("Tile Manager")]
    [SerializeField] private TileManager _tileManager;

    [Header("Camera Shake")]
    public bool IsShaking = false;
    public AnimationCurve animationCurve;
    public float ShakeDuration = 0.3f;
    public float ShakeMultiplier = 1.5f;

    [Header("Camera Lerping")]
    [SerializeField] public float LerpSpeed = 5f;
    private Transform _playerPos;
    private Transform _tractorPos;

    [Header("Pixel Perfect Camera Stuff")]
    [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;
    [SerializeField] private GameManager _gameManager;

    private Vector3 _levelMidpoint = Vector3.zero;
    private bool _hasStartedPPULerp = false;

    void Start()
    {
        _tileManager = FindObjectOfType<TileManager>();
        Debug.Log("CameraManager: Start");

        CenterOnSpawnPositions();
        FindCharacters();

        // Optional: simulate a higher starting value for visual effect
        //_pixelPerfectCamera.assetsPPU = 128;
        //StartCoroutine(LerpPPU(64, 2f));
    }

    void FixedUpdate()
    {
        CheckForRespawn();

        if (_playerPos != null && _tractorPos != null)
        {
            CenteredOnMidpointBoth();
        }
        else if (_playerPos != null)
        {
            CenteredOnPlayerInstant();
        }
        else if (_tractorPos != null)
        {
            CenteredOnRobotInstant();
        }

        if (Input.GetKeyDown(KeyCode.G) && !IsShaking)
        {
            IsShaking = true;
            StartCoroutine(Shaking());
        }
    }

    void CenterOnSpawnPositions()
    {
        if (_tileManager != null)
        {
            float levelCenterX = (_tileManager.GridWidth * _tileManager.TileSize) / 2f;
            float levelCenterY = (_tileManager.GridHeight * _tileManager.TileSize) / 2f;
            _levelMidpoint = new Vector3(levelCenterX, levelCenterY, -15f);

            transform.position = _levelMidpoint;
            Debug.Log("Camera centered on level midpoint.");
        }
        else
        {
            Debug.LogWarning("TileManager reference is missing in CameraManager!");
        }
    }

    void FindCharacters()
    {
        GameObject _newPlayer = GameObject.FindWithTag("Player");
        if (_newPlayer != null) _playerPos = _newPlayer.transform;

        GameObject _newTractor = GameObject.FindWithTag("Tractor");
        if (_newTractor != null) _tractorPos = _newTractor.transform;
    }

    void CheckForRespawn()
    {
        if (_playerPos == null || _tractorPos == null)
        {
            FindCharacters();
        }
    }

    void CenteredOnPlayer()
    {
        Vector3 targetPosition = _playerPos.position;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, targetPosition, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(lerpPosition.x, lerpPosition.y, -15);
    }

    void CenteredOnRobot()
    {
        Vector3 targetPosition = _tractorPos.position;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, targetPosition, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(lerpPosition.x, lerpPosition.y, -15);
    }

    void CenteredOnMidpointBoth()
    {
        Vector3 playerPos = _playerPos.position;
        Vector3 tractorPos = _tractorPos.position;

        Vector3 charMid = (playerPos + tractorPos) / 2f;
        Vector3 trueMidpoint = (_levelMidpoint + charMid) / 2f;

        Vector3 lerpPosition = Vector3.Lerp(transform.position, trueMidpoint, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(lerpPosition.x, lerpPosition.y, -15f);

        if (!_hasStartedPPULerp)
        {
            StartCoroutine(LerpPPU(64, 1.5f)); // Lerp PPU to 64 over 1.5 seconds
            _hasStartedPPULerp = true;
        }
    }

    void CenteredOnPlayerInstant()
    {
        transform.position = new Vector3(_playerPos.position.x, _playerPos.position.y, -15);
    }

    void CenteredOnRobotInstant()
    {
        transform.position = new Vector3(_tractorPos.position.x, _tractorPos.position.y, -15);
    }

    public IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < ShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = animationCurve.Evaluate(elapsedTime / ShakeDuration);
            transform.position = startPosition + Random.insideUnitSphere * strength * ShakeMultiplier;
            yield return null;
        }

        transform.position = startPosition;
        IsShaking = false;
    }

    public IEnumerator LerpPPU(int targetPPU, float duration)
    {
        float timeElapsed = 0f;
        float startPPU = _pixelPerfectCamera.assetsPPU;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            float newPPU = Mathf.Lerp(startPPU, targetPPU, t);
            _pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(newPPU);

            yield return null;
        }

        _pixelPerfectCamera.assetsPPU = targetPPU;
    }
}
