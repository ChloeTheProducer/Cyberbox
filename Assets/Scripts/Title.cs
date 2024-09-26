using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    public float fadeDuration = 1.5f;    // Duration of fade
    public CanvasGroup fadeCanvasGroup;  // CanvasGroup used for fade-in/fade-out

    void Start()
    {
        StartCoroutine(FadeSceneIn());
    }

    IEnumerator FadeSceneIn()
    {
        float elapsedTime = 0f;

        // Set the CanvasGroup's alpha to 0 (fully transparent)
        fadeCanvasGroup.alpha = 0f;

        // Fade in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
