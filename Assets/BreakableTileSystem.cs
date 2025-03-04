using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTileSystem : MonoBehaviour
{
    [SerializeField] private GameObject BreakableTileLocation;
    [SerializeField] private GameObject BreakableTileRigidBody;

    [SerializeField] private int currentTriggerCount = 0;
    [SerializeField] private const int triggerThreshold = 3;

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("dwhaj");
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            currentTriggerCount++;
            Debug.Log("dwnujia");
            if (currentTriggerCount >= triggerThreshold)
            {
                MoveBreakableTile();
            }
        }
    }

    private void MoveBreakableTile()
    {
        if (BreakableTileRigidBody != null && BreakableTileLocation != null)
        {
            BreakableTileRigidBody.transform.position = BreakableTileLocation.transform.position;
        }
    }
}
