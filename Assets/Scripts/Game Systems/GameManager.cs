using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject TractorPrefab;

    public GameObject _player;
    public GameObject _tractor;

    private PlayerMovement _playerMovement;
    private TractorMovement _tractorMovement;

    private Vector2Int _playerStartPosition = new Vector2Int(0, 0);
    private Vector2Int _tractorStartPosition = new Vector2Int(0, 0);

    public TileManager tileManager;
    public SpawnChildren spawnChildren;

    private bool _isHandlingMovement = true;
    private bool _bothCharactersIdle = false;

    void Start()
    {
        StartCoroutine(SetupGame());
    }

    private IEnumerator SetupGame()
    {
        yield return new WaitForSeconds(0.3f);
        // Start grid generation
        yield return StartCoroutine(tileManager.GenerateGridCoroutine());
        yield return new WaitForSeconds(0.3f);
        // Wait until the grid is completely generated
        while (!tileManager.isGridGenerated)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        // Now that the grid is ready, spawn characters
        SpawnTractor();
        SpawnPlayer();

        if (_player != null) _playerMovement = _player.GetComponent<PlayerMovement>();
        if (_tractor != null) _tractorMovement = _tractor.GetComponent<TractorMovement>();
    }

    void Update()
    {
        CheckIfBothCharactersIdle();
        if (_isHandlingMovement) HandleInput();
    }

    void CheckIfBothCharactersIdle()
    {
        bool playerIdle = false;
        bool tractorIdle = false;

        if (_playerMovement == null || !_player.activeSelf || !_playerMovement.isMoving)
        {
            playerIdle = true;
        }

        if (_tractorMovement == null || !_tractor.activeSelf || !_tractorMovement.isMoving)
        {
            tractorIdle = true;
        }

        _bothCharactersIdle = playerIdle && tractorIdle;
    }

    void HandleInput()
    {
        if (!_bothCharactersIdle) return; // Prevent input if characters are moving

        Vector2Int movementDirection = Vector2Int.zero;

        if (Input.GetKey(KeyCode.W)) movementDirection = Vector2Int.up;
        else if (Input.GetKey(KeyCode.A)) movementDirection = Vector2Int.left;
        else if (Input.GetKey(KeyCode.S)) movementDirection = Vector2Int.down;
        else if (Input.GetKey(KeyCode.D)) movementDirection = Vector2Int.right;

        if (movementDirection != Vector2Int.zero)
        {
            // Rotate first (even if movement is blocked)
            if (_playerMovement != null && _player.activeSelf)
            {
                _playerMovement.RotateToDirection(movementDirection);
            }

            if (_tractorMovement != null && _tractor.activeSelf)
            {
                _tractorMovement.RotateToDirection(movementDirection);
            }

            // After rotating, check if movement is possible
            if (_playerMovement != null && _player.activeSelf && CanMoveToTile(_playerMovement, movementDirection))
            {
                MoveCharacter(_playerMovement, movementDirection);
            }

            if (_tractorMovement != null && _tractor.activeSelf && CanMoveToTile(_tractorMovement, movementDirection))
            {
                MoveCharacter(_tractorMovement, movementDirection);
            }
        }
    }

    bool CanMoveToTile(Movement character, Vector2Int direction)
    {
        Vector2Int targetPosition = character.GetGridPosition() + direction;

        Tile targetTile = TileManager.Instance.GetTileAtPosition(targetPosition); // Assuming TileManager has this method

        if (targetTile == null) return false; // Prevent moving into non-existent tiles

        return targetTile.IsWalkable; // Only allow movement if walkable

    }



    public void MoveCharacter(Movement character, Vector2Int direction)
    {
        if (character == null || !character.gameObject.activeSelf) return;
        character.MoveOrTurn(direction);
    }

    public void ToggleGameManagerControl(bool state)
    {
        _isHandlingMovement = state;
        Debug.Log($"GameManager control over movement is now: {_isHandlingMovement}");
    }

    void SpawnPlayer()
    {
        if (tileManager.IsTileAvailable(_playerStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_playerStartPosition.x * tileManager.TileSize, _playerStartPosition.y * tileManager.TileSize);
            _player = Instantiate(PlayerPrefab, spawnPosition, Quaternion.identity);

            if (_player != null) _playerMovement = _player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for player.");
        }
    }

    void SpawnTractor()
    {
        if (tileManager.IsTileAvailable(_tractorStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_tractorStartPosition.x * tileManager.TileSize, _tractorStartPosition.y * tileManager.TileSize);
            _tractor = Instantiate(TractorPrefab, spawnPosition, Quaternion.identity);

            if (_tractor != null) _tractorMovement = _tractor.GetComponent<TractorMovement>();
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for tractor.");
        }
    }

    public void RespawnCharacter(GameObject character, Vector3 targetPosition)
    {
        if (character != null)
        {
            Debug.Log($"{character.name} BEFORE respawn: Active={character.activeSelf}, Position={character.transform.position}");

            character.SetActive(true); // Ensure the character is active

            // Reset movement state (prevents movement lock issues)
            Movement movementScript = character.GetComponent<Movement>();
            if (movementScript != null)
            {
                movementScript.isMoving = false;
                movementScript.isActionOnCooldown = false;
            }

            // Move character to the new position
            character.transform.position = targetPosition;

            // Update movement script's position tracking
            if (movementScript != null)
            {
                movementScript.currentPosition = new Vector2Int((int)targetPosition.x, (int)targetPosition.y);
            }

            Debug.Log($"{character.name} AFTER respawn: Active={character.activeSelf}, Position={character.transform.position}");
        }
        else
        {
            Debug.LogError("RespawnCharacter: Character reference is null!");
        }
    }
}
