using UnityEngine;

public class WallSystem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Random Color Clamp Range (0 to 1)")]
    [Range(0f, 1f)] public float minColorValue = 0.35f;
    [Range(0f, 1f)] public float maxColorValue = 0.55f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject.");

        }
        ApplyRandomClampedColor();
    }

    // Call this function to apply a clamped random color
    public void ApplyRandomClampedColor()
    {
        if (spriteRenderer == null) return;

        float r = Random.Range(minColorValue, maxColorValue);
        float g = Random.Range(minColorValue, maxColorValue);
        float b = Random.Range(minColorValue, maxColorValue);

        Color newColor = new Color(r, g, .65f);
        spriteRenderer.color = newColor;
    }
}
