using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressurePlateSystem : MonoBehaviour
{
    [SerializeField] private float totalWeight;
    [SerializeField] public AudioClip pressurePlateSound;
    private AudioSource audioSource;

    [SerializeField] private List<DoorSystem> controlledDoors; // List of doors this plate controls
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private GameObject highlightBox;
    [SerializeField] private GameObject targetHighlight;
    public Vector3 endPosition;

    private ObjectiveSystem _objectiveSystem;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        UpdateAllDoors();
        UpdateWeightText();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (controlledDoors.Count > 0)
        {
            highlightBox.transform.position = controlledDoors[0].transform.position; // Use first door position for highlight
        }
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
        }
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
                totalWeight += weightedObject.GetWeight();
                UpdateAllDoors();
                UpdateWeightText();
                PlayPressurePlateSound();
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
                totalWeight -= weightedObject.GetWeight();
                UpdateAllDoors();
                UpdateWeightText();
            }
        }
    }

    private void PlayPressurePlateSound()
    {
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

    private void UpdateAllDoors()
    {
        foreach (DoorSystem door in controlledDoors)
        {
            door.UpdateDoorState(totalWeight);
        }
    }

    private void UpdateWeightText()
    {
        if (weightText != null)
        {
            weightText.text = $"{totalWeight % 100}";
        }
        else
        {
            //Debug.LogWarning("Weight TextMeshPro is not assigned in the Inspector!");
        }
    }

    public Vector3 GetEndPosition()
    {
        if (controlledDoors.Count > 0)
        {
            return controlledDoors[0].transform.position; // Return the first door's position
        }
        return transform.position;
    }

    public List<DoorSystem> GetControlledDoors()
    {
        return controlledDoors; // Return all doors instead of just one
    }
}
