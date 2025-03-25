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

    private void Awake()
    {
        plateA = GameObject.Find("RespawnPlateA")?.transform;
        plateB = GameObject.Find("RespawnPlateB")?.transform;
        gameManager = FindObjectOfType<GameManager>(); // Get the GameManager instance

        if (plateA == null || plateB == null)
        {
            Debug.LogError("RespawningSystem: One or both plates are missing! Assign them in the Inspector or check their names.");
        }

        if (gameManager == null)
        {
            Debug.LogError("RespawningSystem: GameManager not found in the scene!");
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

        if (other.CompareTag("Tractor"))
        {
            Debug.Log("Tractor is on the plate. Respawning the player.");
            gameManager.RespawnCharacter(gameManager._player, (plateA.position == other.transform.position) ? plateB.position : plateA.position);
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player is on the plate. Respawning the tractor.");
            gameManager.RespawnCharacter(gameManager._tractor, (plateA.position == other.transform.position) ? plateB.position : plateA.position);
        }

        Debug.Log("Respawn sequence complete.");
        activeTimer = null;
        currentObjectOnPlate = null;
    }
}