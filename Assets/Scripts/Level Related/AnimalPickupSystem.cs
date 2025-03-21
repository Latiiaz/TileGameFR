using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPickupSystem : MonoBehaviour
{
    private ObjectiveSystem _objectiveSystem;

    void Start()
    {
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor")) // Check if the tag matches
        {
            if (_objectiveSystem != null)
            {
                _objectiveSystem._objectiveCount++; // Increase objective count
            }

            Destroy(gameObject); // Destroy this pickup object
        }
    }
}
