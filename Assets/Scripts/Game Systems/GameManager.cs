using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool _bothCharactersHidden = false;

    private LevelManager _levelManager;
    private CameraManager cameraManager; // Reference to CameraManager

    [SerializeField] private float _scaleTime = 0.5f;


    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();  // Get LevelManager reference
        cameraManager = FindObjectOfType<CameraManager>();  // Get CameraManager reference
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
        yield return new WaitForSeconds(0.5f);
        // Now that the grid is ready, spawn characters
        SpawnTractor();
        SpawnPlayer();

        if (_player != null) _playerMovement = _player.GetComponentInChildren<PlayerMovement>();
        if (_tractor != null) _tractorMovement = _tractor.GetComponentInChildren<TractorMovement>();
    }

    void Update()
    {
        CheckIfBothCharactersIdle();
        CheckIfBothCharactersHidden();

        if (_bothCharactersHidden)
        {
            // Play some action before the level is reset, some animation that indicates that both players are dead etc :3
            StartCoroutine(LevelRestart(1.5f));
        }

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

    // Check if both characters are hidden (inactive in hierarchy)
    void CheckIfBothCharactersHidden()
    {
        _bothCharactersHidden = !_player.activeInHierarchy && !_tractor.activeInHierarchy;
    }

    void HandleInput()
    {
        if (!_bothCharactersIdle) return;

        Vector2Int movementDirection = Vector2Int.zero;

        if (Input.GetKey(KeyCode.W)) movementDirection = Vector2Int.up;
        else if (Input.GetKey(KeyCode.A)) movementDirection = Vector2Int.left;
        else if (Input.GetKey(KeyCode.S)) movementDirection = Vector2Int.down;
        else if (Input.GetKey(KeyCode.D)) movementDirection = Vector2Int.right;

        if (movementDirection != Vector2Int.zero)
        {
            if (_playerMovement != null && _player.activeSelf && CanMoveToTile(_playerMovement, movementDirection))
            {
                _playerMovement.MoveInDirection(movementDirection);
            }

            if (_tractorMovement != null && _tractor.activeSelf && CanMoveToTile(_tractorMovement, movementDirection))
            {
                _tractorMovement.MoveInDirection(movementDirection);
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

            _player.transform.localScale = Vector3.one * 0.1f; // Start small
            StartCoroutine(LerpScale(_player.transform, Vector3.one, _scaleTime)); // Grow over time

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

            _tractor.transform.localScale = Vector3.one * 0.1f; // Start small
            StartCoroutine(LerpScale(_tractor.transform, Vector3.one, _scaleTime)); // Scale up

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
            //Debug.Log($"{character.name} BEFORE respawn: Active={character.activeSelf}, Position={character.transform.position}");

            // Trigger camera shake before respawn
            if (cameraManager != null)
            {
                StartCoroutine(cameraManager.ShakeCamera());  // Start camera shake before respawn
            }

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
    IEnumerator LevelRestart(float BasicCooldown)
    {
        yield return new WaitForSeconds(BasicCooldown);
        _levelManager.ReloadCurrentScene();
    }
    private IEnumerator LerpScale(Transform target, Vector3 endScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = endScale;
    }

}
