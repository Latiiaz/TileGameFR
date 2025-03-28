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


    void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            if (_objectiveSystem != null)
            {
                _objectiveSystem._objectiveCount++; // Increase objective count
                //KeyCollected();
                StartCoroutine(ScaleTo(ShadowObject, Vector3.zero , 1));
                ActualObject.SetActive(false);
                PlaySpawnEffect();

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

    void KeyCollected()
    {
        // Disable floating animation by stopping coroutines
        if (_moveSprite != null)
        {
            _moveSprite.StopAllCoroutines();
        }

        if (_scaleSprite != null)
        {
            _scaleSprite.StopAllCoroutines();
        }

        // Move the actual object
        if (ActualObject != null && _moveSprite != null)
        {
            StartCoroutine(MoveToPosition(ActualObject, ActualObject.transform.position + CollectedPos, _moveSprite._timeTaken));
        }

        // Scale the shadow object
        if (ShadowObject != null && _scaleSprite != null)
        {
            StartCoroutine(ScaleTo(ShadowObject, _minScale, _moveSprite._timeTaken/5));
        }
    }

    public IEnumerator MoveToPosition(GameObject targetObject, Vector3 targetPos, float duration)
    {
        if (targetObject == null)
            yield break;

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
        if (targetObject == null)
            yield break;

        Vector3 startScale = targetObject.transform.localScale;
        float elapsedTime = 0f;

        // First, scale to the target size
        while (elapsedTime < duration)
        {
            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.localScale = targetScale;

        // Now, scale down to (0,0,0)
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            targetObject.transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.localScale = Vector3.zero;

        // Stop any ongoing scaling
    }


    private void PlaySpawnEffect()
    {
        if (_keyPickUp != null)
        {
            ParticleSystem effectInstance = Instantiate(_keyPickUp, transform.position, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration + 0.5f); // Auto-destroy effect
        }
        else
        {
            Debug.LogWarning("No spawn effect assigned to the tile.");
        }
    }



}
