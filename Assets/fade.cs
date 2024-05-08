using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingButton : MonoBehaviour
{
    public float fadeSpeed = 1f;
    public float delayTime = 1f;

    private Button buttonComponent;
    private Image buttonImage;
    private Color targetColor;
    private float timer;

    private void Start()
    {
        buttonComponent = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        targetColor = buttonImage.color;
        targetColor.a = 0f;
        buttonImage.color = targetColor;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delayTime)
        {
            float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
            targetColor.a = alpha;
            buttonImage.color = targetColor;
        }
    }
}
