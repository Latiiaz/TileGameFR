using UnityEngine;
using System.Collections;

public class RespawningSystem : MonoBehaviour
{
    public Transform plateA;
    public Transform plateB;
    private GameManager gameManager;

    private Coroutine activeTimer; // Only one active timer at a time
    private GameObject currentObjectOnPlate; // Tracks who is standing on the plate
    public float respawnTimer = 2.0f; // Time needed to trigger the respawn

    private SpriteRenderer spriteRenderer;
    private Coroutine pulseCoroutine;
    private Color originalColor;

    private void Awake()
    {
        plateA = GameObject.Find("RespawnPlateA")?.transform;
        plateB = GameObject.Find("RespawnPlateB")?.transform;
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (plateA == null || plateB == null)
        {
            Debug.LogError("RespawningSystem: One or both plates are missing! Assign them in the Inspector or check their names.");
        }

        if (gameManager == null)
        {
            Debug.LogError("RespawningSystem: GameManager not found in the scene!");
        }
    }

    private void Update()
    {
        bool shouldPulse = false;

        if (gameManager != null)
        {
            shouldPulse = !gameManager._player.activeInHierarchy || !gameManager._tractor.activeInHierarchy;
        }

        if (shouldPulse && pulseCoroutine == null)
        {
            pulseCoroutine = StartCoroutine(PulseColor());
        }
        else if (!shouldPulse && pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    private IEnumerator PulseColor()
    {
        float pulseSpeed = 2f;
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * pulseSpeed;
            float lerpValue = Mathf.PingPong(t, 1f);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.Lerp(originalColor, Color.white, lerpValue);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tractor") || other.CompareTag("Player"))
        {
            if (activeTimer == null) // Ensure only one timer runs at a time
            {
                currentObjectOnPlate = other.gameObject;
                activeTimer = StartCoroutine(RespawnTimer(other));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentObjectOnPlate) // Only cancel if the same object leaves
        {
            if (activeTimer != null)
            {
                StopCoroutine(activeTimer);
                activeTimer = null;
                currentObjectOnPlate = null;
                Debug.Log($"{other.name} left before respawning.");
            }
        }
    }

    private IEnumerator RespawnTimer(Collider2D other)
    {
        Debug.Log($"{other.name} started respawn countdown...");

        yield return new WaitForSeconds(respawnTimer); // Wait for the set time

        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing in RespawningSystem!");
            yield break;
        }

        // Determine target character and destination
        if (other.CompareTag("Tractor"))
        {
            Debug.Log("Tractor is on the plate. Attempting to respawn the player.");

            if (gameManager._player != null && !gameManager._player.activeInHierarchy)
            {
                Vector3 destination = (plateA.position == other.transform.position) ? plateB.position : plateA.position;
                gameManager.RespawnCharacter(gameManager._player, destination);
                Debug.Log("Player respawned successfully.");
            }
            else
            {
                Debug.Log("Player is not hidden. Respawn canceled.");
            }
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player is on the plate. Attempting to respawn the tractor.");

            if (gameManager._tractor != null && !gameManager._tractor.activeInHierarchy)
            {
                Vector3 destination = (plateA.position == other.transform.position) ? plateB.position : plateA.position;
                gameManager.RespawnCharacter(gameManager._tractor, destination);
                Debug.Log("Tractor respawned successfully.");
            }
            else
            {
                Debug.Log("Tractor is not hidden. Respawn canceled.");
            }
        }

        Debug.Log("Respawn sequence complete.");
        activeTimer = null;
        currentObjectOnPlate = null;
    }
}
