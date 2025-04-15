using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionsManager : MonoBehaviour
{
    [Header("Left Eye Variables")]
    [SerializeField] private GameObject _leftEyeObject;
    [SerializeField] private Sprite _defaultLeftEye;

    [Header("Right Eye Variables")]
    [SerializeField] private GameObject _rightEyeObject;
    [SerializeField] private Sprite _defaultRightEye;

    [Header("Shared Eye Variables")]
    [SerializeField] private Sprite _defaultSharedEye;

    [Header("Mouth Variables")]
    [SerializeField] private GameObject _mouthObject;
    [SerializeField] private Sprite _defaultMouth;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    private Vector3 _lastPosition;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform not assigned in ExpressionsManager.");
        }

        _lastPosition = playerTransform.position;
    }

    void Update()
    {
        if (playerTransform == null) return;

        Vector3 movement = playerTransform.position - _lastPosition;

        if (movement.magnitude > 0.001f)
        {
            Vector2 direction = movement.normalized;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    RotateEyesDirection(Vector2.right);
                }
                else
                {
                    RotateEyesDirection(Vector2.left);
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    RotateEyesDirection(Vector2.up);
                }
                else
                {
                    RotateEyesDirection(Vector2.down);
                }
            }
        }

        _lastPosition = playerTransform.position;
    }

    public void RotateEyesDirection(Vector2 direction)
    {
        float zRotation = 0f;

        if (direction == Vector2.right)
        {
            zRotation = -90f;
        }
        else if (direction == Vector2.left)
        {
            zRotation = 90f;
        }
        else if (direction == Vector2.down)
        {
            zRotation = 180f;
        }
        else if (direction == Vector2.up)
        {
            zRotation = 0f;
        }

        if (_leftEyeObject != null)
            _leftEyeObject.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);

        if (_rightEyeObject != null)
            _rightEyeObject.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
