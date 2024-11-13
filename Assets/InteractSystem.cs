using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    // Shoots a raycast infront of the player to see if a game object has the IInteractable script on it
    // Need to have UI prompt indicating it is indeed an interactable. Example: Remove Tree? [E] || Get In Tractor [E]
    // https://www.youtube.com/watch?v=B34iq4O5ZYI Raycast guide

    [SerializeField] private float _raycastRange = 2f; 
    private IInteractable _currentInteractable;

    // Start is called before the first frame update
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, _raycastRange); 

        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                _currentInteractable = interactable;
                //Debug.Log("Interact?"); NOOO STOP SPAMMING RAHHHHHHHHHHH
            }
            else
            {
                _currentInteractable = null;
            }
        }
        else
        {
            _currentInteractable = null;
        }

      
        if (_currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            _currentInteractable.Interact(); 
        }
    }
}