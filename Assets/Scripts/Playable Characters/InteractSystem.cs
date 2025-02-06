using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    // Shoots a raycast in front of the player or tractor to detect interactable objects

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float raycastStartOffset = 0.5f; 


    private PlayerMovement _player;
    private TractorMovement _tractor;
    private UIManager _uiManager;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _tractor = FindObjectOfType<TractorMovement>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        HandlePlayerInteraction();
        HandleTractorInteraction();
    }

    private void HandlePlayerInteraction()
    {
        // Get the player's facing direction
        Vector2 playerFacingDirection = _player.GetFacingDirection();
        Vector2 playerPosition = (Vector2)_player.transform.position;

        // Move raycast start point slightly forward in the facing direction
        Vector2 raycastStart = playerPosition + playerFacingDirection * raycastStartOffset;

        // Perform raycast
        RaycastHit2D playerHit = Physics2D.Raycast(raycastStart, playerFacingDirection, 0.5f, _layerMask);

        // Debug visualization
        if (playerHit.collider != null)
        {
            Debug.DrawRay(raycastStart, playerFacingDirection * playerHit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(raycastStart, playerFacingDirection * 0.5f, Color.red);
        }

        // Process the raycast result
        if (playerHit.collider != null)
        {
            IInteractable iinteractable = playerHit.collider.GetComponent<IInteractable>();
            if (iinteractable != null)
            {
                _uiManager.ShowEInteract(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    iinteractable.InteractE();
                }
                return;
            }
        }

        _uiManager.ShowEInteract(false);
    }


    private void HandleTractorInteraction()
    {
        // Get the tractor's facing direction
        Vector2 tractorFacingDirection = _tractor.GetFacingDirection();
        Vector2 tractorPosition = (Vector2)_tractor.transform.position;

        // Move raycast start point slightly forward in the facing direction
        Vector2 raycastStart = tractorPosition + tractorFacingDirection * raycastStartOffset;

        // Perform raycast
        RaycastHit2D tractorHit = Physics2D.Raycast(raycastStart, tractorFacingDirection, 0.5f, _layerMask);

        // Debug visualization
        //Color debugColor = (tractorHit.collider != null) ? Color.blue : Color.red;
        //Debug.DrawRay(raycastStart, tractorFacingDirection * 0.5f, debugColor);

        // Process the raycast result
        if (tractorHit.collider != null)
        {
            ITractor itractor = tractorHit.collider.GetComponent<ITractor>();
            if (itractor != null)
            {
                _uiManager.ShowFInteract(true);
                if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
                {
                    itractor.InteractF();
                }
                return;
            }
        }

        _uiManager.ShowFInteract(false);
    }

}
