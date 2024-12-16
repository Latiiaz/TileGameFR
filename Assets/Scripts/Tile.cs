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

    // Obstacles Tiles
    Tree,

    // SpawnPoint Tiles
    PlayerSpawn,
    TractorSpawn,
    BoulderSpawn,
    BushSpawn,
    ObjectiveSpawn,
    Pylon
}

public class Tile : MonoBehaviour
{
    public TileType tileType = TileType.Normal;
    private Vector2Int _gridPosition;

    public bool IsWalkable { get; private set; } = true; 
    public bool IsTraversable { get; private set; } = true; 

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on tile prefab!");
            return;
        }
    }

    public void Initialize(Vector2Int position, TileType type)
    {
        _gridPosition = position;
        tileType = type;
        ColorAssigning();
        ResetTileMovability();
    }

    void ColorAssigning()
    {
        Color assignedTileColor;
        switch (tileType)
        {
            case TileType.River:
                assignedTileColor = new Color(0.2f, 0.2f, Random.Range(0.3f, 0.6f));
                break;
            case TileType.Mud:
                assignedTileColor = new Color32(110, 38, 14, 255);
                break;

            case TileType.Tree:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.PlayerSpawn:
                assignedTileColor = new Color(Random.Range(0.3f, 0.6f), Random.Range(0.3f, 0.4f), Random.Range(0.3f, 0.6f));
                break;
            case TileType.TractorSpawn:
                assignedTileColor = new Color(1f, 0f, 0f);
                break;
            case TileType.BoulderSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;
            case TileType.BushSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;
            case TileType.ObjectiveSpawn:
                assignedTileColor = new Color(0.8f, Random.Range(0.3f, 0.4f), 0.2f);
                break;
            case TileType.Pylon:
                assignedTileColor = new Color(0, 0, 0);
                break;

            default:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;
        }

        _spriteRenderer.color = assignedTileColor;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        SetTileMovability(false, false);

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
                    Debug.Log("Player has entered the PlayerSpawn tile.");
                }
                break;

            case TileType.TractorSpawn:
                if (other.CompareTag("Tractor"))
                {
                    Debug.Log("Tractor has entered the TractorSpawn tile.");
                }
                break;

            case TileType.Pylon:
                Debug.Log("Pylon interaction triggered.");
                break;

            default:
                Debug.Log($"Unhandled tile type: {tileType}");
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ResetTileMovability();
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
                IsTraversable = true; // Stun robot when inside
                break;

            case TileType.Tree:
                IsWalkable = true; 
                IsTraversable = true;
                break;

            case TileType.PlayerSpawn:
                IsWalkable = true;
                IsTraversable = true;
                break;
            case TileType.TractorSpawn:
                IsWalkable = true;
                IsTraversable = true;
                break;
            case TileType.ObjectiveSpawn:
                IsWalkable = true;
                IsTraversable = true;
                break;

            case TileType.BoulderSpawn:
                IsWalkable = true;
                IsTraversable = true;
                break;
            case TileType.BushSpawn:
                IsWalkable = true; 
                IsTraversable = true;
                break;

            case TileType.Pylon:
                IsWalkable = true; 
                IsTraversable = true;
                break;

            default:
                IsWalkable = true;
                IsTraversable = true;
                break;
        }
    }


    void HandleNormalTileInteraction(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered a normal tile.");
        }
    }

    void HandleRiverTileInteraction(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered a river tile and is swimming!");
        }
        else if (other.CompareTag("Tractor"))
        {
            Debug.Log("Tractor cannot cross the river tile!");
        }
    }

    void HandleMudTileInteraction(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player slowed down by mud!");
        }
        else if (other.CompareTag("Tractor"))
        {
            Debug.Log("Tractor is unaffected by mud.");
        }
    }
}
