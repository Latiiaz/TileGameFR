using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


public class CameraManager : MonoBehaviour
{
    // Script to handle Camera zoom and stuff
    // Camera Shake when the tractor moves maybe? (Juicing)
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


    [SerializeField] Image _greenFilter;
    [SerializeField] Image _transparentMiddle;

    [SerializeField] GameManager _gameManager;


    void FixedUpdate()
    {
        GameObject _newPlayer = GameObject.FindWithTag("Player");
        _playerPos = _newPlayer.transform;

        GameObject _newTractor = GameObject.FindWithTag("Tractor");
        _tractorPos = _newTractor.transform;
        if (_gameManager.TurnStatus()) // Player Is the main target
        {
            CenteredOnMidpointBoth();
        }
        else // Robot is the main target
        {
            CenteredOnRobot();
        }
        if (Input.GetKeyDown(KeyCode.G) && IsShaking == false)
        {
            IsShaking = true;
            StartCoroutine(Shaking());
        }
    }
    void CenteredOnPlayer()
    {
        Vector3 Player = (_playerPos.position);
        Vector3 LerpLocation = Vector3.Lerp(transform.position, Player, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(LerpLocation.x,LerpLocation.y, -15);

        //Additional Stuff goes here
        _greenFilter.gameObject.SetActive(false);
        _transparentMiddle.gameObject.SetActive(true);

    }
    void CenteredOnRobot()
    {
        Vector3 Robot = (_tractorPos.position);
        Vector3 LerpLocation = Vector3.Lerp(transform.position, Robot, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(LerpLocation.x, LerpLocation.y, -15);

        _greenFilter.gameObject.SetActive(true);
        _transparentMiddle.gameObject.SetActive(false);

    }
    void CenteredOnMidpointBoth()
    {
        Vector3 Robot = (_tractorPos.position);
        Vector3 Player = (_playerPos.position);

        Vector3 LerpLocation = Vector3.Lerp(transform.position, (Robot + Player + Player) /3, LerpSpeed * Time.deltaTime);
        transform.position = new Vector3(_tileManager.GridWidth/2, LerpLocation.y, -15);

        _greenFilter.gameObject.SetActive(false);
        _transparentMiddle.gameObject.SetActive(true);

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
