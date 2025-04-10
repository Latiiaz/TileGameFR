using UnityEngine;

public class BackgroundParticleSystem : MonoBehaviour
{
    [SerializeField] private Transform upPosition;
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform downPosition;
    [SerializeField] private Transform rightPosition;

    private TileManager _tileManager = FindObjectOfType<TileManager>();
    private Vector3 _levelMidpoint = Vector3.zero;

    void Update()
    {
        // Update level midpoint based on tile manager's grid size
        if (_tileManager != null)
        {
            float centerX = (_tileManager.GridWidth * _tileManager.TileSize) / 2f;
            float centerY = (_tileManager.GridHeight * _tileManager.TileSize) / 2f;
            _levelMidpoint = new Vector3(centerX, centerY, -15f);
        }

        // Constantly set the position to the level midpoint
        transform.position = _levelMidpoint;

        // Handle user input for movement (optional)
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
