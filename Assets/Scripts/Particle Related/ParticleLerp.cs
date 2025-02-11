using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLerp : MonoBehaviour
{
    public float moveDuration = 1.0f; // Time in seconds to reach the destination
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsedTime = 0.0f;
    private bool isLerping = false;

    public PressurePlateSystem pressurePlateSystem; // Assign this in the inspector

    void Start()
    {
        if (pressurePlateSystem == null)
        {
            Debug.LogError("PressurePlateSystem reference is not set!");
            return;
        }

        startPosition = pressurePlateSystem.transform.position;
        endPosition = pressurePlateSystem.GetEndPosition();
        transform.position = startPosition;
        StartCoroutine(MoveParticle());
    }

    void Update()
    {
        if (pressurePlateSystem != null)
        {
            endPosition = pressurePlateSystem.GetEndPosition();
        }
    }

    private IEnumerator MoveParticle()
    {
        isLerping = true;
        elapsedTime = 0.0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float timetomove = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, timetomove);
            yield return null;
        }

        transform.position = endPosition;
        //yield return new WaitForSeconds(1f); // Optional delay before teleporting back
        transform.position = startPosition;
        StartCoroutine(MoveParticle()); // Restart the process
    }
}
