using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private float totalWeight = 0f;
    [SerializeField] public AudioClip pressurePlateSound;
    private AudioSource audioSource;

    [SerializeField] private List<DoorSystem> controlledDoors; // List of doors this plate controls
    private int currentDoorIndex = 0;

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
            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null)
            {
                float weight = weightedObject.GetWeight();
                totalWeight += weight;
                UpdateCurrentDoor();

                if (pressurePlateSound != null)
                {
                    audioSource.clip = pressurePlateSound;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogError("Pressure Plate Sound not assigned!");
                }
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
                UpdateCurrentDoor();
            }
        }
    }

    public void InteractE()
    {
        Debug.Log("Pressure plate interacted with. Cycling to next door.");
        CycleToNextDoor();
    }

    private void CycleToNextDoor()
    {
        if (controlledDoors.Count == 0)
        {
            Debug.LogWarning("No doors assigned to this pressure plate!");
            return;
        }

        currentDoorIndex = (currentDoorIndex + 1) % controlledDoors.Count;
        Debug.Log($"Cycled to door: {controlledDoors[currentDoorIndex].name}");
    }

    private void UpdateCurrentDoor()
    {
        if (controlledDoors.Count > 0)
        {
            controlledDoors[currentDoorIndex].UpdateDoorState(totalWeight);
        }
    }
}
