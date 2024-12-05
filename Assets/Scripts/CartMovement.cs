using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CartMovement : MonoBehaviour, IInteractable
{
    // Cart Movement
    // Maybe store the movement of the tractor and the cart is always one step behind it and the first move the cart does is to always move up? followed by the actions of the tractor
    // More than 1 cart can spawn maybe if wanna add more than 1 objective to beating the level

    private Transform tractor; 
    public Vector3 offsetBehindTractor = new Vector3(0, 0,0);  // -1 spawns it rigth behind the tractor
    private Vector3 lastTractorPosition;

    void Awake() // Changed to awake so the code runs when the cart spawns
    {
        // Find the tractor GameObject by tag
        GameObject tractorcartObject = GameObject.FindWithTag("TractorCartLocation");
        if (tractorcartObject != null)
        {
            tractor = tractorcartObject.transform;
            lastTractorPosition = tractor.position;
            FollowTractor();

        }
        else
        {
            Debug.LogError("CartMovement: No GameObject with the tag 'Tractor' was found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowTractor();
    }

    void FollowTractor()
    {
        // Ensure the tractor exists before proceeding
        if (tractor == null)
        {
            Debug.LogWarning("CartMovement: Tractor reference is missing.");
            return;
        }

        // Update the cart's position and rotation if the tractor has moved
        if (tractor.position != lastTractorPosition)
        {
            Vector3 targetCartPosition = tractor.position + offsetBehindTractor;
            transform.position = targetCartPosition;

            lastTractorPosition = tractor.position;
            transform.rotation = tractor.rotation;
        }
    }
    public void InteractE() //Enter Exit tractor
    {
        Debug.Log("EEEEEEEEEEE");
    }

}
