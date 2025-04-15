using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TileType
{
    // Base Tiles
    Normal,
    River,
    Mud,

    // SpawnPoint Tiles
    PlayerSpawn,
    TractorSpawn,

}

public class Tile : MonoBehaviour
{
    public TileType tileType = TileType.Normal;
    private Vector2Int _gridPosition;

    public bool IsWalkable { get; private set; } = true;
    public bool IsTraversable { get; private set; } = true;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _tileSprites;

    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private ParticleSystem _spawnEffectPrefab; // Prefab, NOT an attached effect

    [SerializeField] private ParticleSystem _stepOnEffectPrefab; // Effect when player/tractor steps on tile


    private Color _originalColor;
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on tile prefab!");
            return;
        }

        if (_boxCollider == null)
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }
    }

    public Vector2Int GetGridPosition()
    {
        return _gridPosition;
    }

    public void Initialize(Vector2Int position, TileType type)
    {
        _gridPosition = position;
        tileType = type;

        ColorAssigning();
        _originalColor = _spriteRenderer.color;
        ResetTileMovability();

        PlaySpawnEffect();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            // In here, Apply method to change tile's sprite renderer from white and lerp it slowly to the original sprite renderer hexcode assigned in Initialize method.
            //StopAllCoroutines(); // Stop any ongoing color transition
            //_spriteRenderer.color = Color.white; // Instantly turn white
            //StartCoroutine(LerpColorBack(_originalColor, 0.5f)); // Lerp back
        }

    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Physical") || other.CompareTag("Player") || other.CompareTag("Tractor") || other.CompareTag("Pylon") || other.CompareTag("Door"))

        {
            SetTileMovability(false, false);

        }
        // Handle tile-specific logic based on tile type
        switch (tileType)
        {
            case TileType.Normal:
                HandleNormalTileInteraction(other);
                break;

            case TileType.River:
                HandleRiverTileInteraction(other);
                break;

            case TileType.Mud:
                HandleMudTileInteraction(other);
                break;

            case TileType.PlayerSpawn:
                if (other.CompareTag("Player"))
                {
                    //Debug.Log("Player has entered the PlayerSpawn tile.");
                }
                break;

            case TileType.TractorSpawn:
                if (other.CompareTag("Tractor"))
                {
                    //Debug.Log("Tractor has entered the TractorSpawn tile.");
                }
                break;

            default:
                //Debug.Log($"Unhandled tile type: {tileType}");
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        ResetTileMovability();

        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            PlayStepOnEffect();
            //StopAllCoroutines(); // Stop any ongoing color transition
            //_spriteRenderer.color = new Color(_originalColor.r * 2f, _originalColor.g * 2f, _originalColor.b * 2f);
            //StartCoroutine(LerpColorBack(_originalColor, 0.5f)); // Lerp back
        }
    }

    private void SetTileMovability(bool walkable, bool traversable)
    {
        IsWalkable = walkable;
        IsTraversable = traversable;
    }

    private void ResetTileMovability()
    {
        switch (tileType)
        {
            case TileType.Normal:
                IsWalkable = true;
                IsTraversable = true;
                break;

            case TileType.River:
                IsWalkable = false;
                IsTraversable = true;
                break;

            case TileType.Mud:
                IsWalkable = true;
                IsTraversable = false;
                break;

            case TileType.PlayerSpawn:
                IsWalkable = true;
                IsTraversable = true;
                break;

            case TileType.TractorSpawn:
                IsWalkable = true;
                IsTraversable = true;
                break;

            default:
                IsWalkable = true;
                IsTraversable = true;
                break;
        }
    }

    void HandleNormalTileInteraction(Collider2D other) // Normal tile logic
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered a normal tile.");
        }
    }

    void HandleRiverTileInteraction(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered a river tile and is swimming!");
        }
        else if (other.CompareTag("Tractor"))
        {
            //Debug.Log("Tractor cannot cross the river tile!");
        }
    }

    void HandleMudTileInteraction(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player slowed down by mud!")
        }
        else if (other.CompareTag("Tractor"))
        {
            //Debug.Log("Tractor is unaffected by mud.");
        }
    }

    void ColorAssigning()
    {
        Color assignedTileColor;
        switch (tileType)
        {
            case TileType.River:
                assignedTileColor = new Color(0.2f, 0.2f, Random.Range(0.3f, 0.6f), 1f);
                break;

            case TileType.Mud:
                assignedTileColor = new Color32(110, 38, 14, 25);
                break;

            case TileType.PlayerSpawn:
                assignedTileColor = new Color(0.8024783f, Random.Range(0.445194f, 0.545194f), 0.9811321f, 1f);
                break;

            case TileType.TractorSpawn:
                assignedTileColor = new Color(0.8024783f, Random.Range(0.445194f, 0.545194f), 0.9811321f, 1f);
                break;

            default:
                assignedTileColor = new Color(0.8024783f, Random.Range(0.445194f, 0.545194f), 0.9811321f, 1f);
                break;
        }

        _spriteRenderer.color = assignedTileColor;
    }


    private IEnumerator LerpColorBack(Color targetColor, float duration)
    {
        float time = 0;
        Color startColor = _spriteRenderer.color;

        while (time < duration)
        {
            _spriteRenderer.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = targetColor; // Ensure final color is set
    }


    private void PlaySpawnEffect()
    {
        if (_spawnEffectPrefab != null)
        {
            ParticleSystem effectInstance = Instantiate(_spawnEffectPrefab, transform.position, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration + 0.5f); // Auto-destroy effect
        }
        else
        {
            Debug.LogWarning("No spawn effect assigned to the tile.");
        }
    }
    private void PlayStepOnEffect()
    {
        if (_stepOnEffectPrefab != null)
        {
            ParticleSystem effectInstance = Instantiate(_stepOnEffectPrefab, transform.position, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration + 0.5f);
        }
    }

}