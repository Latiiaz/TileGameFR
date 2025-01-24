using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    // Shoots a raycast in front of the player or tractor to detect interactable objects

    [SerializeField] private float _raycastRange = 1f;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Vector2 _raycastOffset;

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
        Vector2 playerPosition = _player.transform.position;

        // Perform raycast from the player's position
        RaycastHit2D playerHit = Physics2D.Raycast((playerPosition + _raycastOffset), playerFacingDirection, _raycastRange, _layerMask);

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
        Vector2 tractorPosition = _tractor.transform.position;

        // Perform raycast from the tractor's position
        RaycastHit2D tractorHit = Physics2D.Raycast(tractorPosition+ _raycastOffset, tractorFacingDirection, _raycastRange, _layerMask);

        // Process the raycast result
        if (tractorHit.collider != null)
        {
            ITractor itractor = tractorHit.collider.GetComponent<ITractor>();
            if (itractor != null)
            {
                _uiManager.ShowFInteract(true);
                if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
                {
                    itractor.InteractF(); // uses itractor but still still uses shift + e to control.
                }
                return; 
            }
        }

        _uiManager.ShowFInteract(false);
    }
}
