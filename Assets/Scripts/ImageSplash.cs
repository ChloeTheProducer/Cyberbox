using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For scene management
using System.Collections;

public class ImageSplash : MonoBehaviour
{
    public TextMeshProUGUI titleText;      // Assign the title TextMeshProUGUI
    public SpriteRenderer[] sprites;       // Assign the SpriteRenderers you want to fade
    public float fadeDuration = 1.5f;      // Duration of fade
    public CanvasGroup fadeCanvasGroup;    // CanvasGroup for scene fading
    public float displayTime = 2f;         // Time to display the title before fading out
    public string nextSceneName;           // Name of the next scene to load

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade In the scene (text and sprites)
        yield return StartCoroutine(FadeSceneIn());

        // Wait for the display time before fading out
        yield return new WaitForSeconds(displayTime);

        // Fade Out the scene (text and sprites)
        yield return StartCoroutine(FadeSceneOut());

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeSceneIn()
    {
        float elapsedTime = 0f;

        // Set initial alpha to 0 for CanvasGroup, Text, and Sprites
        fadeCanvasGroup.alpha = 0f;
        SetAlpha(titleText, 0);
        SetSpritesAlpha(0); // Set sprites' alpha to 0 initially

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            fadeCanvasGroup.alpha = alpha;
            SetAlpha(titleText, alpha);
            SetSpritesAlpha(alpha); // Fade sprites along with the text

            yield return null;
        }
    }

    IEnumerator FadeSceneOut()
    {
        float elapsedTime = 0f;

        // Start with everything fully visible
        SetAlpha(titleText, 1);
        SetSpritesAlpha(1);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Reverse fade

            fadeCanvasGroup.alpha = alpha;
            SetAlpha(titleText, alpha);
            SetSpritesAlpha(alpha); // Fade sprites along with the text

            yield return null;
        }
    }

    // Helper method to set the alpha of TextMeshProUGUI
    void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        if (text != null)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }

    // Helper method to set the alpha of all sprites
    void SetSpritesAlpha(float alpha)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a = alpha;
                sprite.color = color;
            }
        }
    }
}
