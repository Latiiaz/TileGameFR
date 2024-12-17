using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InteractSystem : MonoBehaviour
{
    // Shoots a raycast infront of the player to see if a game object has the IInteractable script on it
    // Need to have UI prompt indicating it is indeed an interactable. Example: Remove Tree? [E] || Get In Tractor [F]
    //Uses placer facing direction in the playermovement script

    //Update to add in F to leave tractor and E to interact with any object in/out of tractor
    // https://www.youtube.com/watch?v=B34iq4O5ZYI Raycast guide

    [SerializeField] private float _raycastRange = 1f;
    [SerializeField] private LayerMask _layerMask;
    private PlayerMovement _player;

    private UIManager _uiManager;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _uiManager = FindObjectOfType<UIManager>();

    }
    void Update()
    {
        Vector2 rayDirection = transform.up; // Raycasting
        Color rayColor = Color.green;

        bool isInteracting = false;


        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, _raycastRange, _layerMask);

        if (hit.collider != null)

        {

            // Check for Interactables
            IInteractable iinteractable = hit.collider.GetComponent<IInteractable>();
            if (iinteractable != null)
            {
                rayColor = Color.blue;
                _uiManager.ShowEInteract(true);
                isInteracting = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Debug.Log("dwhajda");
                    iinteractable.InteractE();
                }
            }

        }
        if (!isInteracting)
        {
            _uiManager.ShowEInteract(false);
            _uiManager.ShowFInteract(false);
        }
        Debug.DrawRay(transform.position, rayDirection * _raycastRange, rayColor);
    }
}
