using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInChildren : MonoBehaviour
{
    public float fadeDuration = 1.5f; // Time it takes to fully fade in

    void Start()
    {
        StartCoroutine(FadeInAllChildren());
    }

    IEnumerator FadeInAllChildren()
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        GetAllChildSprites(transform, spriteRenderers);

        float elapsedTime = 0f;

        // Store the initial colors
        Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color newColor = sr.color;
            newColor.a = 0f; // Start completely transparent
            sr.color = newColor;
            originalColors[sr] = newColor;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Normalize 0  1

            foreach (var sr in spriteRenderers)
            {
                Color newColor = originalColors[sr];
                newColor.a = alpha;
                sr.color = newColor;
            }

            yield return null;
        }

        // Ensure full opacity at the end
        foreach (var sr in spriteRenderers)
        {
            Color newColor = originalColors[sr];
            newColor.a = 1f;
            sr.color = newColor;
        }
    }

    void GetAllChildSprites(Transform parent, List<SpriteRenderer> spriteList)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteList.Add(sr);
            }
            GetAllChildSprites(child, spriteList); // Recursively get deeper children
        }
    }
}
