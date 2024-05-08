using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour
{
    public float fadeSpeed = 1f;
    public float delayTime = 1f;

    private Text textComponent;
    private Color targetColor;
    private float timer;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        targetColor = textComponent.color;
        targetColor.a = 0f;
        textComponent.color = targetColor;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delayTime)
        {
            float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
            targetColor.a = alpha;
            textComponent.color = targetColor;
        }
    }
}
