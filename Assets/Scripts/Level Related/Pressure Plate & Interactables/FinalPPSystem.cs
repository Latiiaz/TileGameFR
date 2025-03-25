using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPPSystem : MonoBehaviour
{
    [SerializeField] private float totalWeight;
    [SerializeField] public AudioClip pressurePlateSound;
    private AudioSource audioSource;

    [SerializeField] private bool isLocked = true; // Locked by default
    private ObjectiveSystem _objectiveSystem;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Sprites for different states
    [SerializeField] private Sprite Locked;          // Sprite when Locked
    [SerializeField] private Sprite Unlocked;        // Sprite when Unlocked
    [SerializeField] private Sprite SteppedLocked;   // Sprite when Stepped + Locked
    [SerializeField] private Sprite SteppedUnlocked; // Sprite when Stepped + Unlocked

    [SerializeField] private float timerDuration = 3f; // Time before weight is added
    private float timer = 0f;
    private bool isTimerRunning = false;
    private bool objectOnPlate = false; // Tracks if an object is on the plate
    private IWeightedObject currentWeightedObject; // Store the object that triggered the timer

    private void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // **Instantly check and apply correct state when spawned**
        isLocked = !_objectiveSystem._objectiveEnabled; // Set locked state based on objective
        UpdateSprite();
    }

    void Update()
    {
        UpdateSprite();
        // Check if objective is enabled and unlock if necessary
        if (_objectiveSystem._objectiveEnabled && isLocked)
        {
            isLocked = false;
            PlayPressurePlateSound();
            UpdateSprite();
        }

        // Timer logic (runs while an object is on the plate)
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isTimerRunning = false;
                AddWeight();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            objectOnPlate = true;
            UpdateSprite(); // Update sprite when stepped on

            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null && !isTimerRunning)
            {
                // Start timer only once
                timer = timerDuration;
                isTimerRunning = true;
                currentWeightedObject = weightedObject; // Store the object for weight addition
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            objectOnPlate = false;
            UpdateSprite(); // Update sprite when stepped off

            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null)
            {
                totalWeight = 0f;
                isTimerRunning = false;
                currentWeightedObject = null;
            }
        }
    }

    private void AddWeight()
    {
        if (currentWeightedObject != null)
        {
            totalWeight += currentWeightedObject.GetWeight();
            PlayPressurePlateSound();
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

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        if (objectOnPlate)
        {
            // If stepped on, use the corresponding stepped sprite
            spriteRenderer.sprite = isLocked ? SteppedLocked : SteppedUnlocked;
        }
        else
        {
            // If not stepped on, use the normal locked/unlocked sprite
            spriteRenderer.sprite = isLocked ? Locked : Unlocked;
        }
    }

    public float GetTotalWeight()
    {
        return totalWeight;
    }
}
