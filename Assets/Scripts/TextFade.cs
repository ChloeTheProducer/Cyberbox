using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;  // For scene management

public class TextFade : MonoBehaviour
{
    public TextMeshProUGUI tipText;  // Assign your "Warning" TextMeshProUGUI here
    public float fadeDuration = 1.5f;    // Duration of fade
    public float displayTime = 2f;       // Time to display before fading out
    public CanvasGroup fadeCanvasGroup;  // CanvasGroup used for fade-in/fade-out

    

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade in "Warning"
        yield return StartCoroutine(FadeTextIn(tipText));

        // Wait for display time
        yield return new WaitForSeconds(displayTime);

        // Switch to the next scene and fade in
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        Color color = text.color;
        color.a = 0;
        text.color = color;

        while (text.color.a < 1.0f)
        {
            color.a += Time.deltaTime / fadeDuration;
            text.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        // Fade out the entire screen using CanvasGroup
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }
    }
}
