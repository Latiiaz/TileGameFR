using System.Collections;
using UnityEngine;

public class KeyPickUpSystem : MonoBehaviour
{
    private ObjectiveSystem _objectiveSystem;

    [SerializeField] private GameObject ShadowObject;
    [SerializeField] private GameObject ActualObject;

    [SerializeField] private MoveSprite _moveSprite;
    [SerializeField] private ScaleSprite _scaleSprite;

    [SerializeField] public Vector3 CollectedPos = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] public Vector3 _minScale = new Vector3(1.5f, 1.5f, 1f);

    [SerializeField] private ParticleSystem _keyPickUp;

    //  Add AudioClip or AudioSource reference
    [SerializeField] private AudioClip _pickUpSound;
    private AudioSource _audioSource;


    public KeyCollection _keyCollection;


    void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        _audioSource = GetComponent<AudioSource>();
        _keyCollection = FindObjectOfType<KeyCollection>();
        if (_audioSource == null && _pickUpSound != null)
        {
            // If no AudioSource is attached, create one on the fly
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            if (_objectiveSystem != null)
            {
                _objectiveSystem._objectiveCount++; // Increase objective count

                // Play pickup animation/effects
                StartCoroutine(ScaleTo(ShadowObject, Vector3.zero, 1));
                ActualObject.SetActive(false);
                _keyCollection.KeyCollected();

                PlaySpawnEffect();
                PlayPickUpSound(); // ? Play sound when key is picked up
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }


    public IEnumerator MoveToPosition(GameObject targetObject, Vector3 targetPos, float duration)
    {
        if (targetObject == null) yield break;

        Vector3 startPos = targetObject.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetObject.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.position = targetPos;
        Destroy(gameObject);
    }

    public IEnumerator ScaleTo(GameObject targetObject, Vector3 targetScale, float duration)
    {
        _scaleSprite.StopScaling();
        if (targetObject == null) yield break;

        Vector3 startScale = targetObject.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.localScale = targetScale;

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            targetObject.transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.localScale = Vector3.zero;
    }

    private void PlaySpawnEffect()
    {
        if (_keyPickUp != null)
        {
            ParticleSystem effectInstance = Instantiate(_keyPickUp, transform.position, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration + 0.5f);
        }
        else
        {
            Debug.LogWarning("No spawn effect assigned to the tile.");
        }
    }

    //  Method to play sound
    private void PlayPickUpSound()
    {
        if (_audioSource != null && _pickUpSound != null)
        {
            _audioSource.pitch = 20f; 
            _audioSource.PlayOneShot(_pickUpSound);
        }
        else
        {
            Debug.LogWarning("Missing audio source or pickup sound.");
        }
    }

}
