using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTileSystem : MonoBehaviour
{
    [SerializeField] private GameObject BreakableTileLocation;
    [SerializeField] private GameObject BreakableTileRigidBody;
    [SerializeField] private Sprite[] tileSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private int currentTriggerCount = 1;
    [SerializeField] private const int triggerThreshold = 4;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            currentTriggerCount++;
            UpdateTileSprite();
            if (currentTriggerCount >= triggerThreshold)
            {
                MoveBreakableTile();
            }
        }
    }

    private void UpdateTileSprite()
    {
        if (tileSprites.Length > 0 && currentTriggerCount - 1 < tileSprites.Length)
        {
            spriteRenderer.sprite = tileSprites[currentTriggerCount - 1];
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
