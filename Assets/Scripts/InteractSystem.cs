using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    // Shoots a raycast infront of the player to see if a game object has the IInteractable script on it
    // Need to have UI prompt indicating it is indeed an interactable. Example: Remove Tree? [E] || Get In Tractor [F]
    //Uses placer facing direction in the playermovement script

    //Update to add in F to leave tractor and E to interact with any object in/out of tractor
    // https://www.youtube.com/watch?v=B34iq4O5ZYI Raycast guide

    [SerializeField] private float _raycastRange = 2f;
    private IInteractable _currentIInteractable;
    private ITractor _currentITractor;
    // Start is called before the first frame update
    void Update()
    {
        RaycastHit2D hitE = Physics2D.Raycast(transform.position, Vector2.up, _raycastRange);
        if (hitE.collider != null)
        {
            IInteractable interactable = hitE.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                _currentIInteractable = interactable;
                //Debug.Log("Interact?"); NOOO STOP SPAMMING RAHHHHHHHHHHH
            }
            else
            {
                _currentIInteractable = null;
            }
        }
        else
        {
            _currentIInteractable = null;
        }

      
        if (_currentIInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            _currentIInteractable.InteractE(); 


        }



        RaycastHit2D hitF = Physics2D.Raycast(transform.position, Vector2.up, _raycastRange);
        if (hitF.collider != null)
        {
            ITractor interactable = hitF.collider.GetComponent<ITractor>();

            if (interactable != null)
            {
                _currentITractor = interactable;
                //Debug.Log("Interact?"); NOOO STOP SPAMMING RAHHHHHHHHHHH
            }
            else
            {
                _currentITractor = null;
            }
        }
        else
        {
            _currentITractor = null;
        }


        if (_currentITractor != null && Input.GetKeyDown(KeyCode.F))
        {
            _currentITractor.InteractF();
        }
    }
}