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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip _tileCracking;
    [SerializeField] private AudioClip _tileBreaking;

    [Header("Camera")]
    [SerializeField] private CameraManager cameraManager;


    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();  // Get CameraManager reference

        currentTriggerCount = maxTriggers;
        UpdateTriggerAmountText();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            currentTriggerCount--;
            UpdateTriggerAmountText();

            if (currentTriggerCount <= 0)
            {
                FindObjectOfType<GameManager>().DisableInputTemporarily(0.5f);

                PlaySound(_tileBreaking);
                MoveBreakableTile();
                RemoveText();
                if (cameraManager != null)
                {
                    StartCoroutine(cameraManager.ShakeCamera(0, 1f)) ;  // Start camera shake before respawn
                }
            }
            else
            {
                PlaySound(_tileCracking);
                if (cameraManager != null)
                {
                    StartCoroutine(cameraManager.ShakeCamera(0,0.01f));  // Start camera shake before respawn
                }
            }

            // Uncomment if you want the sprite to change per step
            // UpdateTileSprite();
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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
