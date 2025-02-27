using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTileSystem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private GameObject replacementObject; // The object to place when max passes are reached
    [SerializeField] private int maxPasses = 3; // Maximum number of times the tile can be stepped on

    [SerializeField]    private int timesPassed = 0;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Robot"))
        {
            timesPassed++;
            Debug.Log("Dwa");
            if (timesPassed >= maxPasses)
            {
                
                _boxCollider2D.isTrigger = false;
                if (replacementObject != null)
                {
                    Instantiate(replacementObject, transform.position, Quaternion.identity);
                }
            }
        }
    }
}