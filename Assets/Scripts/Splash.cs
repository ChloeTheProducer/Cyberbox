using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;  // For scene management

public class SplashScreen : MonoBehaviour
{
    public TextMeshProUGUI splashText;  // Assign your "Warning" TextMeshProUGUI here
    public float fadeDuration = 1.5f;    // Duration of fade
    public float displayTime = 2f;       // Time to display before fading out
    public CanvasGroup fadeCanvasGroup;  // CanvasGroup used for fade-in/fade-out

    public string nextSceneName;         // Name of the next scene to load

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade in "Warning"
        yield return StartCoroutine(FadeTextIn(splashText));

        // Wait for display time
        yield return new WaitForSeconds(displayTime);

        // Switch to the next scene and fade in
        yield return StartCoroutine(FadeOutAndLoadScene());
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

    IEnumerator FadeOutAndLoadScene()
    {
        // Fade out the entire screen using CanvasGroup
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);

        // After the scene is loaded, fade in the new scene
        yield return StartCoroutine(FadeSceneIn());
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
