using UnityEngine;
using TMPro;

public class FrameCounter : MonoBehaviour
{
    public TextMeshProUGUI frameRateText; // TextMeshProUGUI component to display the frame rate
    private int frameCount = 0;
    private float elapsedTime = 0f;
    private float refreshTime = 0.1f; // Time in seconds between updates to the UI
    private int targetFPS = 165; // FPS cap

    void start()
    {
        Application.targetFrameRate = targetFPS;
    }
    
    void Update()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= refreshTime)
        {
            int fps = Mathf.RoundToInt(frameCount / elapsedTime);
            frameRateText.text = "FPS: " + Mathf.Min(fps, targetFPS) + "/165";
            frameCount = 0;
            elapsedTime = 0f;
        }
    }
}
