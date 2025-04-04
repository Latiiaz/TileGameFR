using UnityEngine;

public class BackgroundParticleSystem : MonoBehaviour
{
    [SerializeField] private Transform upPosition;
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform downPosition;
    [SerializeField] private Transform rightPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveAndRotate(upPosition);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveAndRotate(leftPosition);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveAndRotate(downPosition);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveAndRotate(rightPosition);
        }
    }

    private void MoveAndRotate(Transform target)
    {
        if (target == null) return;

        // Instantly move the position and apply rotation
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
