using UnityEngine;
using System.Collections;

public class RespawningSystem : MonoBehaviour
{
    public Transform plateA;
    public Transform plateB;

    private Coroutine activeTimer; // Only one active timer at a time
    private GameObject currentObjectOnPlate; // Tracks who is standing on the plate
    public float respawnTimer = 2.0f; // Time needed to trigger the respawn

    private void Awake()
    {
        plateA = GameObject.Find("RespawnPlateA")?.transform;
        plateB = GameObject.Find("RespawnPlateB")?.transform;

        if (plateA == null || plateB == null)
        {
            Debug.LogError("RespawningSystem: One or both plates are missing! Assign them in the Inspector or check their names.");
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

        if (other.CompareTag("Tractor"))
        {
            TractorMovement tractor = other.GetComponent<TractorMovement>();
            tractor?.CheckRespawn(plateA, plateB);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player?.CheckRespawn(plateA, plateB);
        }

        Debug.Log($"{other.name} has been respawned!");

        // Reset after respawn
        activeTimer = null;
        currentObjectOnPlate = null;
    }
}
