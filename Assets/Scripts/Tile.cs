using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType // Might need different types of tiles and need to pair with tilemanager to ensure it doesnt break spawning again
{
    Normal,
    SpawnPoint,
    River,
    Tree
}


public class Tile : MonoBehaviour
{
    // Script to handle individual tile behavior
    // https://discussions.unity.com/t/how-do-you-change-a-color-in-spriterenderer/520132 Color changing tiles for now

    public TileType tileType = TileType.Normal;
    private Vector2Int _gridPosition;
    public bool IsWalkable = true;
    //public bool PlayerCheck = false;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ColorRandomization();
    }

    public void Initialize(Vector2Int position, TileType type)
    {
        _gridPosition = position;
        tileType = type;
    }

    public void SetWalkable(bool walkable)
    {
        IsWalkable = walkable;
    }

    private void ColorRandomization()
    {
        Color randomizedColor;

        if (Random.value > 0.5f)
        {
            randomizedColor = new Color(0.2f, 0.4f, 0.2f);
        }
        else
        {
            randomizedColor = new Color(0.2f, 0.3f, 0.2f);
        }
        _spriteRenderer.color = randomizedColor;
    }

    public void DifferentTiles()
    {
        switch (tileType)
        {
            case TileType.SpawnPoint:
                Debug.Log("Is le spawn");
                break;

            case TileType.River:
                Debug.Log("Woosh water");
                break;

            case TileType.Tree:
                Debug.Log("Twee");
                break;

            case TileType.Normal:
                Debug.Log("normel");
                break;
        }
    }

    public void PerformTileAction()
    {
        Debug.Log("Performing action on tile: " + this.name);
    }

}