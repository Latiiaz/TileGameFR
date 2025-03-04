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
    BreakableTile,
    Pylon
}

public class Tile : MonoBehaviour
{
    public TileType tileType = TileType.Normal;
    private Vector2Int _gridPosition;

    public bool IsWalkable { get; private set; } = true;
    public bool IsTraversable { get; private set; } = true;

    public bool IsShielded { get; private set; } = false;

    private bool isBroken = false;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _tileSprites;

    // Expose BoxCollider2D to be manually assigned in the Unity Editor
    //[SerializeField] private BoxCollider2D _boxCollider;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on tile prefab!");
            return;
        }

        // If the BoxCollider2D is not assigned in the editor, we can try to get it.
        //if (_boxCollider == null)
        //{
        //    _boxCollider = GetComponent<BoxCollider2D>();
        //}
    }

    public void SetIsShielded(bool shielded) // Currently the code runs often and the sprites keep changing
    {
        IsShielded = shielded;

        if (shielded)
        {
            //ColorAssigningShielded(); // add checks to only change once
        }
        else
        {
            //ColorAssigning();
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
        ColorAssigning(); // Will be replaced by SpriteAssign();
        ResetTileMovability();
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

            case TileType.BreakableTile:
                HandleBreakableTileInteraction(other);
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

          

            case TileType.Pylon:
                Debug.Log("Pylon interaction triggered.");
                break;

            default:
                //Debug.Log($"Unhandled tile type: {tileType}");
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
                IsTraversable = false; // Stun robot when inside
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

            case TileType.BreakableTile:
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

    void HandleNormalTileInteraction(Collider2D other) // basically runs on update
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered a normal tile.");
        }
    } // Normal tile logic

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
    } // River/Spike tile logic

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
    } // Mud tile logic
    void HandleBreakableTileInteraction(Collider2D other)
    {
        if (isBroken)
        {
            return;
        }

        if (other.CompareTag("Player") || other.CompareTag("Tractor"))
        {
            if (tileType == TileType.BreakableTile)
            {
                isBroken = true; 
         
                IsWalkable = false;
                IsTraversable = false;       
                
                _spriteRenderer.color = new Color(0.5f, 0.2f, 0.2f, 0.5f);
                Debug.Log("Tile Kaboom.");
            }
        }
    }

    void ColorAssigning()
    {
        Color assignedTileColor;
        switch (tileType)
        {
            case TileType.River:
                assignedTileColor = new Color(0.2f, 0.2f, Random.Range(0.3f, 0.6f), 0.55f);
                break;

            case TileType.Mud:
                assignedTileColor = new Color32(110, 38, 14, 25);
                break;

            case TileType.Tree:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f, 0.3f);
                break;

            case TileType.PlayerSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.TractorSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.BoulderSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f, 0.3f);
                break;

            case TileType.BushSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f, 0.3f);
                break;

            case TileType.BreakableTile:
                assignedTileColor = new Color(0.8f, Random.Range(0.3f, 0.4f), 0.2f, 0.3f);
                break;

            case TileType.Pylon:
                assignedTileColor = new Color(0, 0, 0, 0.3f);
                break;

            default:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;
        }

        _spriteRenderer.color = assignedTileColor;
    }

    void ColorAssigningShielded()
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
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.TractorSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.BoulderSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.BushSpawn:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f);
                break;

            case TileType.BreakableTile:
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

    void SpriteAssign()
    {
        Sprite assignedSprite = null;

        switch (tileType)
        {
            case TileType.River:
                assignedSprite = _tileSprites[1];
                break;

            case TileType.Mud:
                assignedSprite = _tileSprites[2];
                break;

            case TileType.Tree:
                assignedSprite = _tileSprites[3];
                break;

            case TileType.PlayerSpawn:
                assignedSprite = _tileSprites[4];
                break;

            case TileType.TractorSpawn:
                assignedSprite = _tileSprites[5];
                break;

            case TileType.BoulderSpawn:
                assignedSprite = _tileSprites[6];
                break;

            case TileType.BushSpawn:
                assignedSprite = _tileSprites[7];
                break;

            case TileType.BreakableTile:
                assignedSprite = _tileSprites[8];
                break;

            case TileType.Pylon:
                assignedSprite = _tileSprites[9];
                break;

            default:
                assignedSprite = _tileSprites[0];
                break;
        }

        _spriteRenderer.sprite = assignedSprite;
    }
}
