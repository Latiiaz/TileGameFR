using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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

    void FixedUpdate()
    {
        GameObject _newPlayer = GameObject.FindWithTag("Player");
        _playerPos = _newPlayer.transform;

        GameObject _newTractor = GameObject.FindWithTag("Tractor");
        _tractorPos = _newTractor.transform;

        PlayerTractorLerp();
        if (Input.GetKeyDown(KeyCode.Space) && IsShaking == false)
        {
            IsShaking = true;
            //Debug.Log("shakey");
            StartCoroutine(Shaking());
        }
    }
    void PlayerTractorLerp()
    {
        Vector3 midPointTractorPlayer = (_playerPos.position + _tractorPos.position) / 2;
        Vector3 LerpLocation = Vector3.Lerp(transform.position, midPointTractorPlayer, LerpSpeed * Time.deltaTime);

        transform.position = new Vector3(LerpLocation.x,LerpLocation.y, -15);

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
