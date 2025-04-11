using UnityEngine;

public class BGMSystem : MonoBehaviour
{
    private static BGMSystem instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate BGM objects
        }
    }
}
