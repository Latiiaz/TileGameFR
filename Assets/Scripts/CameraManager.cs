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

    // Start is called before the first frame update
    void Start()
    {
        CameraCentering();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && StartShake == true)
        {
            StartShake = true;
            //StartShake = false;
            Debug.Log("shakey");
            StartCoroutine(Shaking());
        }
    }
    void CameraCentering()
    {
        transform.position = new Vector3(_tileManager.GridWidth /2, _tileManager.GridHeight /2,-15);
    }

    // Referenced from previous project
    public IEnumerator Shaking()
    {
        Vector3 startposition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < ShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = animationCurve.Evaluate(elapsedTime / ShakeDuration);
            transform.position = startposition + Random.insideUnitSphere * strength * ShakeMultiplier;
            yield return null;
        }

    }
}
