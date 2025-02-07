using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLerp : MonoBehaviour
{
    public float lerpSpeed = 0.25f; // Speed of the lerp
    private Vector3 startPosition;
    private Vector3 endPosition; // Ending position obtained from another script
    private float lerpTime = 0.0f; // Time variable for lerp

    // Reference to the other script that contains the end position
    public PressurePlateSystem pressurePlateSystem; // Assign this in the inspector

    void Start()
    {
        startPosition = pressurePlateSystem.transform.position;
        transform.position = startPosition;

        if (pressurePlateSystem != null)
        {
            endPosition = pressurePlateSystem.GetEndPosition();
        }
        else
        {
            Debug.LogError("OtherScript reference is not set!");
        }

        
    }

    void Update()
    {
        StartCoroutine(ParticleRespawn());
    }

    private void LerpingParticle()
    {
        lerpTime += Time.deltaTime * lerpSpeed;

        // Lerp the position
        transform.position = Vector3.Lerp(startPosition, endPosition, lerpTime);

        // Optional: Reset the lerp when it reaches the end position
        if (lerpTime >= 1.0f)
        {
            // Optionally reset or stop the lerp
            // lerpTime = 0.0f; // Uncomment to loop
            enabled = false; // Disable this script if you want to stop
        }
    }

    private IEnumerator ParticleRespawn()
    {
        LerpingParticle();
        yield return new WaitForSeconds(3f);
    }
}
