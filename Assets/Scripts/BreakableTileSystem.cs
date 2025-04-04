using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakableTileSystem : MonoBehaviour
{
    [SerializeField] private GameObject BreakableTileLocation;
    [SerializeField] private GameObject BreakableTileRigidBody;
    [SerializeField] private Sprite[] tileSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

   
    [SerializeField] private int maxTriggers = 4;

    [SerializeField] private int currentTriggerCount;

    [SerializeField] private TextMeshProUGUI triggerText;


    private void Start()
    {
        currentTriggerCount = maxTriggers;
        UpdateTriggerAmountText();

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            currentTriggerCount--;
            UpdateTriggerAmountText();
            //UpdateTileSprite();
            if (currentTriggerCount <= 0)
            {
                MoveBreakableTile();
                RemoveText();
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
    private void UpdateTriggerAmountText()
    {
        if (triggerText != null)
        {
            triggerText.text = $"{currentTriggerCount}";
        }
        else
        {
            Debug.LogWarning("Weight TextMeshPro is not assigned in the Inspector!");
        }
    }
    private void RemoveText()
    {
        triggerText.gameObject.SetActive(false);
    }
}
