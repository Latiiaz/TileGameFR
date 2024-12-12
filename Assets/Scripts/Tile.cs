using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType // Might need different types of tiles and need to pair with tilemanager to ensure it doesnt break spawning again
{
    //Base Tiles
    Normal,
    River,
    Mud,

    //Obstacles TIles
    Tree,

    //SpawnPoint Tiles
    PlayerSpawn, //Can remove, the player will spawn on the same spawnpoint as the tractor now
    TractorSpawn,
    BoulderSpawn,
    BushSpawn,
    ObjectiveSpawn,
    Pylon
}


public class Tile : MonoBehaviour
{
    // Script to handle individual tile behavior
    // https://discussions.unity.com/t/how-do-you-change-a-color-in-spriterenderer/520132 Color changing tiles for now

    public TileType tileType = TileType.Normal;
    private Vector2Int _gridPosition;
    public bool IsWalkable => TilesWalkabilityBehavior(); // get walkable from method
    public bool IsTraversable => TilesTraversabilityBehavior(); // Tractor movement will use Traversable instead of Walkable to allow tractors to be blocked by river blocks but the player can go past them.
    //public bool PlayerCheck = false;

    public SpriteRenderer _spriteRenderer;

    // AWAKE AWAKE AWAKE THIS THING TOOK 2 HOURS TO FIGURE OUT AS TO WHY I COULD NOT GET SPRITE RENDERER AND I JUST NEEDED IT TO BE ON AWAKE I AM GOING TO CRY AND DIE OH MY GODDDDDDDDDDDDDD
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on tile prefab!");
            return;
        }
    }
    public void Initialize(Vector2Int position, TileType type, bool walkable, bool traversable)
    {
        _gridPosition = position;
        tileType = type;
        //IsTraversable = traversable;
        ColorAssigning();
       
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
                assignedTileColor = new Color32(110,38,14,255);
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
                assignedTileColor = new Color(0, 0, 0);
                break;
            case TileType.Pylon:
                assignedTileColor = new Color(0, 0, 0);
                break;

            default:
                assignedTileColor = new Color(0.2f, Random.Range(0.3f, 0.4f), 0.2f); // This is Green
                break;

        }
        
        _spriteRenderer.color = assignedTileColor;
    }

    public bool TilesWalkabilityBehavior()
    {
        switch (tileType)
        {
            case TileType.River:      // Only River is Walkable to cross, tractor cannot cross
                //Debug.Log("River Tiles Spawn Here");
                return false;
            case TileType.Mud:   // Rename to Mud, Tractor can go across, player cannot
                //Debug.Log("Unstable Grounds Here");
                return true;


            case TileType.Tree:
                //Debug.Log("NonMovable Tree Spawns Here");
                return true;


            case TileType.PlayerSpawn:
                //Debug.Log("Player Spawns Here");
                return true;
            case TileType.TractorSpawn:
                //Debug.Log("Tractor Spawns Here");
                return true;

            case TileType.BoulderSpawn:
                //Debug.Log("Boulder Spawns here");
                return true;

            case TileType.BushSpawn:
                //Debug.Log("Cuttable Bushes Spawns Here");
                return true;

            case TileType.ObjectiveSpawn:
                //Debug.Log("Objective Spawns here");
                return true;
            case TileType.Pylon:
                //Debug.Log("Objective Spawns here");
                return false;

            default:
                //Debug.Log("Normal Tile Spawns Here");
                return true;

        }
    }

    public bool TilesTraversabilityBehavior()
    {
        switch (tileType)
        {
            case TileType.River:      // Only River is Walkable to cross, tractor cannot cross
                //Debug.Log("River Tiles Spawn Here");
                return true;
            case TileType.Mud:   // Rename to Mud, Tractor can go across, player cannot
                //Debug.Log("Unstable Grounds Here");
                return false;


            case TileType.Tree:
                //Debug.Log("NonMovable Tree Spawns Here");
                return true;


            case TileType.PlayerSpawn:
                //Debug.Log("Player Spawns Here");
                return true;
            case TileType.TractorSpawn:
                //Debug.Log("Tractor Spawns Here");
                return true;

            case TileType.BoulderSpawn:
                //Debug.Log("Boulder Spawns here");
                return true;

            case TileType.BushSpawn:
                //Debug.Log("Cuttable Bushes Spawns Here");
                return true;

            case TileType.ObjectiveSpawn:
                //Debug.Log("Objective Spawns here");
                return true;
            case TileType.Pylon:
                //Debug.Log("Objective Spawns here");
                return false;

            default:
                //Debug.Log("Normal Tile Spawns Here");
                return true;

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (this._gridPosition.x == 9 && this._gridPosition.y == 5 && other.CompareTag("Player")) // Theres a pretty good chance that the colliders are too big, reduce size of box colliders to 0.95 when seperating image and game objects
        {
            Debug.Log("Player is on 9,5");

            if (this._gridPosition.x == 18 && this._gridPosition.y == 10 && other.CompareTag("Tractor")) // Theres a pretty good chance that the colliders are too big, reduce size of box colliders to 0.95 when seperating image and game objects
            {
                Debug.Log("Tractor is on 18,10");

                Debug.Log("Victory");
            }
        }
    }



}