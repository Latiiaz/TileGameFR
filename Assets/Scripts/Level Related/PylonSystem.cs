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
}
