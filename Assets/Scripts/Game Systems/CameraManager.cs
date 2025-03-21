using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Tile Manager")]
    [SerializeField] private TileManager _tileManager;

    [Header("Camera Shake")]
    public bool IsShaking = false;
    public AnimationCurve animationCurve;
    public float ShakeDuration = 10f;
    public float ShakeMultiplier = 10f;

    [Header("Camera Lerping")]
    [SerializeField] public float LerpSpeed = 5f;
    private Transform _playerPos;
    private Transform _tractorPos;

    [SerializeField] private GameManager _gameManager;

    void Start()
    {
        FindCharacters();
    }

    void FixedUpdate()
    {
        CheckForRespawn();

        if (_playerPos != null && _tractorPos != null)
        {
            if (_gameManager.TurnStatus()) // Player Is the main target
            {
                CenteredOnMidpointBoth();
            }
            else // Robot is the main target
            {
                CenteredOnRobot();
            }
        }
        else if (_playerPos != null) // Only Player exists
        {
            CenteredOnPlayerInstant();
        }
        else if (_tractorPos != null) // Only Tractor exists
        {
            CenteredOnRobotInstant();
        }

        if (Input.GetKeyDown(KeyCode.G) && !IsShaking)
        {
            IsShaking = true;
            StartCoroutine(Shaking());
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
            FindCharacters(); // Check if a character has respawned
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

        Vector3 lerpPosition = Vector3.Lerp(transform.position, (playerPos + tractorPos) / 2, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(lerpPosition.x, lerpPosition.y, -15);
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
}
