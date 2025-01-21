using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour
{
    // Output strength on pressure plates to ensure that certain tiles can reach certain "checks"
    [SerializeField] public bool output;

    [SerializeField] private AudioClip PressurePlateSound;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }
    public bool GetOutputStatus()
    {
        return output;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ToggleOutput();
        audioSource.clip = PressurePlateSound;
        audioSource.Play();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        ToggleOutput();
    }

    private void ToggleOutput()
    {
        output = !output;
        Debug.Log($"Pressure Plate Output Changed: {output}");
    }
}