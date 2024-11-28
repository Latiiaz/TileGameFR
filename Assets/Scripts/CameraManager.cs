using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Script to handle Camera zoom and stuff
    // Camera Shake when the tractor moves maybe? (Juicing)

    [SerializeField] private TileManager _tileManager; 

    public bool StartShake = false;
    public AnimationCurve animationCurve;
    public float ShakeDuration = 10f;
    public float ShakeMultiplier = 10f;

    [SerializeField] public float LerpSpeed = 5f;

    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private Transform _playerPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        GameObject _newPlayer = GameObject.FindWithTag("Player");
        _playerPos = _newPlayer.transform;
        PlayerLerp();
        //CameraCentering();
        if (Input.GetKeyDown(KeyCode.Space) && StartShake == true)
        {
            StartShake = true;
            //StartShake = false;
            Debug.Log("shakey");
            StartCoroutine(Shaking());
        }
    }

    void CameraCentering()
    {
        Vector3 playerPosition = _playerPos.position;
        transform.position = new Vector3(Mathf.Clamp(playerPosition.x, _minX, _maxX), Mathf.Clamp(playerPosition.y, _minY, _maxY), -10f);

        Debug.Log(transform.position);

        //transform.position = new Vector3(_tileManager.GridWidth /2, _tileManager.GridHeight /2,-15);
    }

    void PlayerLerp()
    {
        Vector3 PlayerPos = _playerPos.position;
        Vector3 LerpLocation = Vector3.Lerp(transform.position, PlayerPos, LerpSpeed * Time.deltaTime);

        transform.position = new Vector3(LerpLocation.x,LerpLocation.y, -15);

    }

    // Referenced from previous project
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
    }
}
