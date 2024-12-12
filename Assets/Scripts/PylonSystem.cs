using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PylonSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private TetherSystem _tetherSystem;

    private TileManager _tileManager;
    private Vector2Int _pylonPosition;

    void Start()
    {
       
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
