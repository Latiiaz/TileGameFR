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
    private CameraManager cameraManager;
    private OptionsMenuSystem _optionsMenuSystem;

    [SerializeField] private float _scaleTime = 0.5f;

    [Header("Sound Related")]
    public AudioClip respawnSound;
    public AudioSource audioSource;

    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        _optionsMenuSystem =  FindObjectOfType<OptionsMenuSystem>();
    }
    void OnLevelLoaded()
    {
        ResetState();
        StartCoroutine(SetupGame());
    }

    private IEnumerator SetupGame()
    {
        //Debug.Log("[SetupGame] Setup started");

        DisableInputTemporarily(2);
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(tileManager.GenerateGridCoroutine());
        yield return new WaitForSeconds(0.3f);

        while (!tileManager.isGridGenerated)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //Debug.Log("[SetupGame] Spawning characters...");
        SpawnTractor();
        SpawnPlayer();

        if (_player != null) _playerMovement = _player.GetComponentInChildren<PlayerMovement>();
        if (_tractor != null) _tractorMovement = _tractor.GetComponentInChildren<TractorMovement>();

        //Debug.Log("[SetupGame] Input is now allowed again.");
        _isHandlingMovement = true;
    }



    void Update()
    {
        //Debug.Log($"[Update] IsHandlingMovement: {_isHandlingMovement}");
        //Debug.Log($"Player active: {_player?.activeSelf}, Tractor active: {_tractor?.activeSelf}");
        //Debug.Log($"Player movement null? {_playerMovement == null}, Tractor movement null? {_tractorMovement == null}");
        //Debug.Log($"Player isMoving: {_playerMovement?.isMoving}, Tractor isMoving: {_tractorMovement?.isMoving}");
        //Debug.Log($"Options Menu Open: {OptionsMenuSystem.IsOptionsMenuOpen}");

        if (OptionsMenuSystem.IsOptionsMenuOpen) return;

        if (_player != null || _tractor != null)
        {
            CheckIfBothCharactersIdle();
            CheckIfBothCharactersHidden();
        }

        if (_bothCharactersHidden)
        {
            //Debug.Log("[Update] Both characters are hidden, restarting level...");
            StartCoroutine(LevelRestart(1.5f));
        }

        if (_isHandlingMovement)
        {
            HandleInput();
        }
        else
        {
            //Debug.Log("[Update] Movement input is currently disabled.");
        }
    }


    void CheckIfBothCharactersIdle()
    {
        bool playerIdle = _playerMovement == null || !_player.activeSelf || !_playerMovement.isMoving;
        bool tractorIdle = _tractorMovement == null || !_tractor.activeSelf || !_tractorMovement.isMoving;
        _bothCharactersIdle = playerIdle && tractorIdle;
    }

    void CheckIfBothCharactersHidden()
    {
        _bothCharactersHidden = !_player.activeInHierarchy && !_tractor.activeInHierarchy;
    }

    void HandleInput()
    {
        if (!_bothCharactersIdle)  //  Movement won't work until both characters are idle!
        {
            //Debug.Log("[HandleInput] Characters are not idle");
            return;
        }

        Vector2Int inputDirection = Vector2Int.zero;

        if (Input.GetKey(KeyCode.W)) inputDirection = Vector2Int.up;
        else if (Input.GetKey(KeyCode.A)) inputDirection = Vector2Int.left;
        else if (Input.GetKey(KeyCode.S)) inputDirection = Vector2Int.down;
        else if (Input.GetKey(KeyCode.D)) inputDirection = Vector2Int.right;

        if (inputDirection == Vector2Int.zero)
        {
            return;  //  If no key is held, nothing happens — this is expected
        }

        //Debug.Log($"[HandleInput] Direction pressed: {inputDirection}");

        if (_playerMovement != null && _player.activeSelf)
        {
            if (CanMoveToTile(_playerMovement, inputDirection))
            {
                //Debug.Log("[HandleInput] Moving player...");
                _playerMovement.MoveInDirection(inputDirection);
            }
        }

        if (_tractorMovement != null && _tractor.activeSelf)
        {
            if (CanMoveToTile(_tractorMovement, inputDirection))
            {
                //Debug.Log("[HandleInput] Moving tractor...");
                _tractorMovement.MoveInDirection(inputDirection);
            }
        }
    }


    bool CanMoveToTile(Movement character, Vector2Int direction)
    {
        Vector2Int targetPosition = character.GetGridPosition() + direction;
        Tile targetTile = TileManager.Instance.GetTileAtPosition(targetPosition);

        if (targetTile == null) return false;

        return targetTile.IsWalkable;
    }

    public void MoveCharacter(Movement character, Vector2Int direction)
    {
        if (character == null || !character.gameObject.activeSelf) return;
        character.MoveOrTurn(direction);
    }

    void SpawnPlayer()
    {
        if (tileManager.IsTileAvailable(_playerStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_playerStartPosition.x * tileManager.TileSize, _playerStartPosition.y * tileManager.TileSize);
            _player = Instantiate(PlayerPrefab, spawnPosition, Quaternion.identity);

            _player.transform.localScale = Vector3.one * 0.1f;
            StartCoroutine(LerpScale(_player.transform, Vector3.one, _scaleTime));

            if (_player != null) _playerMovement = _player.GetComponent<PlayerMovement>();
        }
        else
        {
        }
    }

    void SpawnTractor()
    {
        if (tileManager.IsTileAvailable(_tractorStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_tractorStartPosition.x * tileManager.TileSize, _tractorStartPosition.y * tileManager.TileSize);
            _tractor = Instantiate(TractorPrefab, spawnPosition, Quaternion.identity);

            _tractor.transform.localScale = Vector3.one * 0.1f;
            StartCoroutine(LerpScale(_tractor.transform, Vector3.one, _scaleTime));

            if (_tractor != null) _tractorMovement = _tractor.GetComponent<TractorMovement>();
        }
        else
        {
        }
    }

    public void RespawnCharacter(GameObject character, Vector3 targetPosition)
    {
        if (character != null)
        {
            character.transform.localScale = Vector3.one;

            if (cameraManager != null)
            {
                StartCoroutine(cameraManager.ShakeCamera(0.5f, 1.5f));
            }

            if (PPManager.Instance != null)
            {
                PPManager.Instance.PulseBloom(15f, 1f);
            }

            StartCoroutine(CharacterSpawning(character));

            // Reset movement state
            Movement movementScript = character.GetComponent<Movement>();
            if (movementScript != null)
            {
                movementScript.isMoving = false;
                movementScript.isActionOnCooldown = false;
            }

            // Move character to new position
            character.transform.position = targetPosition;

            // Update movement script's position tracking
            if (movementScript != null)
            {
                movementScript.currentPosition = new Vector2Int((int)targetPosition.x, (int)targetPosition.y);
            }

            ResetCharacterExpression(character); // Reset expression to default

        }
        else
        {
        }
    }

    private void ResetCharacterExpression(GameObject character)
    {
        var expressionManager = character.GetComponentInChildren<ExpressionsManager>();
        if (expressionManager != null)
        {
            expressionManager.ResetToDefaultExpression(); // Assumes the default sprite is set in the editor
        }
        else
        {
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

    private IEnumerator CharacterSpawning(GameObject character)
    {
        //Debug.Log($"[Respawn] Coroutine started for {character.name}");

        yield return new WaitForSeconds(0.5f);

        //Debug.Log($"[Respawn] Activating {character.name}");
        character.SetActive(true);

        if (audioSource != null && respawnSound != null)
        {
            //Debug.Log($"[Respawn] Playing respawn sound for {character.name}");
            audioSource.PlayOneShot(respawnSound);
        }
        else
        {
            //Debug.LogWarning("[Respawn] Missing audio source or respawn clip!");
        }

        //Debug.Log($"[Respawn] Coroutine completed for {character.name}");

        yield return null;
    }

    public void DisableInputTemporarily(float duration)
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(DisableInputCoroutine(duration));
    }

    private IEnumerator DisableInputCoroutine(float duration)
    {
        _isHandlingMovement = false;
        yield return new WaitForSeconds(duration);
        _isHandlingMovement = true;
    }
    public void ResetState()
    {
        _player = null;
        _tractor = null;
        _playerMovement = null;
        _tractorMovement = null;
        _isHandlingMovement = false;
        _bothCharactersIdle = false;
        _bothCharactersHidden = false;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        //Debug.Log("[GameManager] Scene loaded, reinitializing.");
        StartCoroutine(SetupGame());
        if (OptionsMenuSystem.IsOptionsMenuOpen)
        {
            OptionsMenuSystem.ResetMenuFlag();
        }
    }

}
