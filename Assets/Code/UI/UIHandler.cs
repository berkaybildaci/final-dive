using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler UIHandlerInstance;
    public Image imageToFade; // Assign the image you want to fade in the Inspector
    public float fadeTime = 1f; // Adjust the fade duration in seconds

    private bool isFading = false;

    private IEnumerator FadeOut()
    {
        isFading = true; // Set flag to indicate fading is in progress

        float elapsedTime = 0f;
        Color currentColor = imageToFade.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            currentColor.a = alpha;
            imageToFade.color = currentColor;

            yield return null;
        }

        isFading = false; // Reset flag when fading is complete
    }

    public void StartFade()
    {
        if (!isFading) // Check if fading is not already in progress
        {
            StartCoroutine(FadeOut());
        } else
        {
            StopCoroutine(FadeOut());
            StartCoroutine(FadeOut());
        }
    }
    void Awake()
    {
        UIHandlerInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
