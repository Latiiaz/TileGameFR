using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    // Cart Movement
    // Maybe store the movement of the tractor and the cart is always one step behind it and the first move the cart does is to always move up? followed by the actions of the tractor

    public Transform tractor; 
    public Vector2 offsetBehindTractor = new Vector2(0, -1);  // -1 spawns it rigth behind the tractor
    private Vector3 lastTractorPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastTractorPosition = tractor.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (tractor.position != lastTractorPosition)
        {
            Vector3 targetCartPosition = tractor.position + (Vector3)offsetBehindTractor;

            transform.position = targetCartPosition;

            lastTractorPosition = tractor.position;
            transform.rotation = tractor.rotation;
        }
    }
}
