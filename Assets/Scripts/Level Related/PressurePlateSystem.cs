using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour, IInteractable
{
    // Total weight on the pressure plate
    [SerializeField] private float totalWeight = 0f;

    [SerializeField] private AudioClip pressurePlateSound;
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

    public float GetTotalWeight()
    {
        return totalWeight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Objective") || other.CompareTag("Tractor"))
        {
            Debug.Log($"Detected collision with: {other.name}, Tag: {other.tag}");

            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null)
            {
                float weight = weightedObject.GetWeight();
                totalWeight += weight;
                Debug.Log($"Object entered: {other.name}, Weight: {weight}, Total Weight: {totalWeight}");

                audioSource.clip = pressurePlateSound;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"{other.name} does not have IWeightedObject implemented!");
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Objective") || other.CompareTag("Tractor"))
        {
            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null)
            {
                float weight = weightedObject.GetWeight();
                totalWeight -= weight;
                Debug.Log($"Object exited: {other.name}, Weight: {weight}, Total Weight: {totalWeight}");
            }
        }
    }

    public void InteractE()
    {
        Debug.Log("Diggy Diggy hole, Changing target door");
        
    }
}
