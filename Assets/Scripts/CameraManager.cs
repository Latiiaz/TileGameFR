using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Script to handle Camera zoom and stuff
    // Camera Shake when the tractor moves maybe? (Juicing)

    [SerializeField] private TileManager _tileManager;

    // Start is called before the first frame update
    void Start()
    {
        CameraCentering();
    }

    void CameraCentering()
    {
        transform.position = new Vector3(_tileManager.GridWidth /2, _tileManager.GridHeight /2,-15);
    }
}
