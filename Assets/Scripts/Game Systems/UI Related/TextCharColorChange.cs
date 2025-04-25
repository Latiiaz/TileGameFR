using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextCharColorChange : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    public float speed = 0.1f;
    [Range(0f, 1f)]
    public float pastelAmount = 0.5f; // 0 = vivid, 1 = full white

    void Update()
    {
        float t = Time.time * speed;
        Color vividColor = Color.HSVToRGB((t % 1f), 1f, 1f);
        Color pastelColor = Color.Lerp(vividColor, Color.white, pastelAmount);
        image.color = pastelColor;
        text.color = pastelColor;
    }
}