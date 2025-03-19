using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPPSystem : MonoBehaviour
{
    [SerializeField] private float totalWeight;
    [SerializeField] public AudioClip pressurePlateSound;
    private AudioSource audioSource;

    [SerializeField] private bool isLocked = false;
    private ObjectiveSystem _objectiveSystem;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite activeSprite; // Sprite when objective is enabled
    [SerializeField] private Sprite defaultSprite; // Default sprite

    [SerializeField] private float timerDuration = 3f; // Time before weight is added
    private float timer = 0f;
    private bool isTimerRunning = false;
    private bool weightAdded = false; // Ensures weight is added once per stay
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

        if (spriteRenderer != null)
        {
            defaultSprite = spriteRenderer.sprite; // Store the default sprite
        }
    }

    void Update()
    {
        if (_objectiveSystem._objectiveEnabled && isLocked)
        {
            isLocked = false;
            PlayPressurePlateSound();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = isLocked ? Color.gray : Color.magenta;
            spriteRenderer.sprite = _objectiveSystem._objectiveEnabled ? activeSprite : defaultSprite;
        }

        // Timer logic (moved to Update to ensure continuous countdown)
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Tractor")) && !weightAdded)
        {
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
            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null)
            {
                totalWeight = 0f;
                isTimerRunning = false;
                weightAdded = false; // Reset weight addition when the object leaves
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
            weightAdded = true;
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

    // **Restored Method**
    public float GetTotalWeight()
    {
        return totalWeight;
    }
}
