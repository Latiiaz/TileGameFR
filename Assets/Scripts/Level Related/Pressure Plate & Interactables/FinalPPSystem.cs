using System.Collections;
using UnityEngine;

public class FinalPPSystem : MonoBehaviour
{
    [SerializeField] private float totalWeight;
    [SerializeField] public AudioClip pressurePlateSound;
    private AudioSource audioSource;

    [SerializeField] private bool isLocked = true;
    private ObjectiveSystem _objectiveSystem;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite Locked;
    [SerializeField] private Sprite Unlocked;
    [SerializeField] private Sprite SteppedLocked;
    [SerializeField] private Sprite SteppedUnlocked;

    [SerializeField] private float timerDuration = 3f;
    private float timer = 0f;
    private bool isTimerRunning = false;
    private bool objectOnPlate = false;
    private IWeightedObject currentWeightedObject;

    [Header("Particles")]
    [SerializeField] private GameObject completionParticlesPrefab; // Prefab reference

    private void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        isLocked = !_objectiveSystem._objectiveEnabled;
        UpdateSprite();
    }

    void Update()
    {
        UpdateSprite();

        if (_objectiveSystem._objectiveEnabled && isLocked)
        {
            isLocked = false;
            PlayPressurePlateSound();
            PlayCompletionParticles(); // Trigger particles here
            UpdateSprite();
            return;
        }

        if (isLocked) return;

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
        if (isLocked) return;

        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            objectOnPlate = true;
            UpdateSprite();
            PlayPressurePlateSound();

            IWeightedObject weightedObject = other.GetComponent<IWeightedObject>();
            if (weightedObject != null && !isTimerRunning)
            {
                timer = timerDuration;
                isTimerRunning = true;
                currentWeightedObject = weightedObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            objectOnPlate = false;
            UpdateSprite();
            PlayPressurePlateSound();

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
        if (isLocked) return;

        if (currentWeightedObject != null)
        {
            totalWeight += currentWeightedObject.GetWeight();
        }
    }

    private void PlayPressurePlateSound()
    {
        if (pressurePlateSound != null)
        {
            audioSource.clip = pressurePlateSound;
            audioSource.PlayOneShot(pressurePlateSound);
        }
        else
        {
            Debug.LogError("Pressure Plate Sound not assigned!");
        }
    }

    private void PlayCompletionParticles()
    {
        if (completionParticlesPrefab != null)
        {
            GameObject particles = Instantiate(completionParticlesPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax); // clean up after playing
            }
            else
            {
                Debug.LogWarning("The prefab does not contain a ParticleSystem component.");
            }
        }
        else
        {
            Debug.LogWarning("Completion particle prefab not assigned!");
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        if (objectOnPlate)
        {
            spriteRenderer.sprite = isLocked ? SteppedLocked : SteppedUnlocked;
        }
        else
        {
            spriteRenderer.sprite = isLocked ? Locked : Unlocked;
        }
    }

    public float GetTotalWeight()
    {
        return totalWeight;
    }

    public Vector3 GetEndPosition()
    {
        return transform.position;
    }
}
