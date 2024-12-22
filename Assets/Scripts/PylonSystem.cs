using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PylonSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private TetherSystem _tetherSystem;

    private TileManager _tileManager;
    private Vector2Int _pylonPosition;

    [SerializeField] private AudioClip PylonTurningOn;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _tileManager = FindObjectOfType<TileManager>();
        if (_pylonPosition == null)
        {
            SetPylonSpawnPosition();
        }
        else
        {
            return;
        }

    }
    public void InteractE() //Enter Exit tractor
    {
        _tetherSystem.IsCurrentlyActive = true;
        Debug.Log("Interacted With Pylon");
        if (PylonTurningOn != null && audioSource != null)
        {
            audioSource.clip = PylonTurningOn;
            audioSource.Play();
        }
    }
    void SetPylonSpawnPosition()
    {
        foreach (var tileKey in _tileManager.tileDictionary)
        {
            if (tileKey.Value.tileType == TileType.Pylon)
            {
                _pylonPosition = tileKey.Key;
                Vector2 worldPosition = new Vector2(_pylonPosition.x * _tileManager.TileSize, _pylonPosition.y * _tileManager.TileSize);
                transform.position = worldPosition;
                Debug.Log($"Pylon spawned at: {worldPosition}");
            }
        }
    }

}
