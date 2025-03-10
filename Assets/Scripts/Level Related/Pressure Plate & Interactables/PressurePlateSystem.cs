using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressurePlateSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private float totalWeight;
    [SerializeField] public AudioClip pressurePlateSound;
    private AudioSource audioSource;

    [SerializeField] private List<DoorSystem> controlledDoors; // List of doors this plate controls
    private int currentDoorIndex = 0;

    [SerializeField] private TextMeshProUGUI weightText;

    [SerializeField] private GameObject highlightBox;
    [SerializeField] private GameObject targetHighlight;


    public Vector3 endPosition;

    [SerializeField] private bool isLocked = false;

    private ObjectiveSystem _objectiveSystem;
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        UpdateCurrentDoor();
        UpdateWeightText();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        }

        highlightBox.transform.position = controlledDoors[currentDoorIndex].transform.position;
    }

    void Update()
    {
        if (_objectiveSystem._objectiveEnabled && isLocked)
        {
            isLocked = false;
            audioSource.clip = pressurePlateSound;
            audioSource.Play();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = isLocked ? Color.gray : Color.magenta;
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
                float weight = weightedObject.GetWeight();
                totalWeight += weight;
                UpdateCurrentDoor();
                UpdateWeightText();

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
                UpdateWeightText();
            }
        }
    }

    public void InteractE()
    {
        Debug.Log("Pressure plate interacted with. Cycling to next door.");
        CheckDoors();
    }
    private void CheckDoors()
    {
        if (controlledDoors != null)
        {
            CycleToNextDoor();
            return;
        }
        else
        {
            currentDoorIndex = (currentDoorIndex + 1) % controlledDoors.Count;
            CycleToNextDoor();
        }
        
    }
    private void CycleToNextDoor()
    {
        if (controlledDoors.Count == 0)
        {
            Debug.LogWarning("No doors assigned to this pressure plate!");
            return;
        }

        currentDoorIndex = (currentDoorIndex + 1) % controlledDoors.Count;
        //Debug.Log($"Cycled to door: {controlledDoors[currentDoorIndex].name} at position {controlledDoors[currentDoorIndex].transform.position}");
    }


    private void UpdateCurrentDoor()
    {
        if (controlledDoors.Count > 0)
        {
            controlledDoors[currentDoorIndex].UpdateDoorState(totalWeight);
        }
    }
    private void UpdateWeightText()
    {
        if (weightText != null)
        {
            weightText.text = $"{totalWeight%100}";
        }
        else
        {
            Debug.LogWarning("Weight TextMeshPro is not assigned in the Inspector!");
        }
    }

    //void ShowTargetHighlight(bool highlightActive)
    //{
    //    if (true)
    //    {
    //        targetHighlight.SetActive(highlightActive);

    //    }
    //}


    public Vector3 GetEndPosition()
    {
        if (controlledDoors.Count > 0)
        {
            return controlledDoors[currentDoorIndex].transform.position;
        }
        return transform.position;
    }

    public DoorSystem GetCurrentDoor()
    {
        if (controlledDoors.Count > 0)
        {
            return controlledDoors[currentDoorIndex]; // Return the currently active door
        }
        return null;
    }
}

