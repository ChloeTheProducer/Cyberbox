using UnityEngine;
using UnityEditor;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour
{
    [Header("Show Settings")]
    [Min(0)]
    public int bit;
    public bool inverted;
    private MackValves valves;

    [Header("Light Settings")]
    [Range(0, 1000)]
    [Tooltip("The target brightness of the light")]
    public int brightness;
    private float currentBrightness;

    [Range(0, 5)]
    [Tooltip("Time in seconds the light takes to go from off to on")]
    public float onSpeed;

    [Range(0, 5)]
    [Tooltip("Time in seconds the light takes to go from on to off")]
    public float offSpeed;

    private Light lightComponent;
    private bool active;

    [Header("Effects")]
    public LightingPattern lightingPattern;
    public enum LightingPattern
    {
        None,
        Rainbow,
        Strobe,
        Breathing,
    }
    [Range(0.01f, 25f)]
    public float patternSpeedMultiplier;

    private Coroutine currentPatternCoroutine;

    void Start()
    {
        lightComponent = GetComponent<Light>();
        valves = GameObject.FindGameObjectWithTag("Mack Valves").GetComponent<MackValves>();

        // Initialize light intensity
        currentBrightness = lightComponent.intensity;
        lightComponent.intensity = 0f;
        active = false;
    }

    void Update()
    {
        bool shouldLightBeOn = valves.Bits[bit];

        // Invert the bit if inverted is true
        if (inverted)
        {
            shouldLightBeOn = !shouldLightBeOn;
        }

        if (shouldLightBeOn && !active)
        {
            // Turn the light on
            StartCoroutine(ChangeLightIntensity(brightness, onSpeed));
            active = true;

            // Start the selected lighting pattern
            StartLightingPattern();
        }
        else if (!shouldLightBeOn && active)
        {
            // Turn the light off
            StartCoroutine(ChangeLightIntensity(0, offSpeed));
            active = false;

            // Stop any active pattern coroutine
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);
                currentPatternCoroutine = null;
            }
        }
    }


    private void StartLightingPattern()
    {
        // Stop any previous pattern
        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
        }

        // Start the new pattern
        switch (lightingPattern)
        {
            case LightingPattern.Rainbow:
                currentPatternCoroutine = StartCoroutine(RainbowPattern());
                break;
            case LightingPattern.Strobe:
                currentPatternCoroutine = StartCoroutine(StrobePattern());
                break;
            case LightingPattern.Breathing:
                currentPatternCoroutine = StartCoroutine(BreathingPattern());
                break;
            case LightingPattern.None:
            default:
                currentPatternCoroutine = null;
                break;
        }
    }

    private IEnumerator ChangeLightIntensity(int targetBrightness, float duration)
    {
        float startIntensity = lightComponent.intensity;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            lightComponent.intensity = Mathf.Lerp(startIntensity, targetBrightness, time / duration);
            currentBrightness = lightComponent.intensity; // Update currentBrightness
            yield return null;
        }

        lightComponent.intensity = targetBrightness;
        currentBrightness = targetBrightness; // Update currentBrightness at the end
    }

    private IEnumerator RainbowPattern()
    {
        while (true)
        {
            float time = Mathf.PingPong(Time.time * patternSpeedMultiplier, 1f);
            lightComponent.color = Color.HSVToRGB(time, 1f, 1f);
            yield return null;
        }
    }

    private IEnumerator StrobePattern()
    {
        while (true)
        {
            lightComponent.enabled = !lightComponent.enabled;
            yield return new WaitForSeconds(1f / patternSpeedMultiplier);
        }
    }

    private IEnumerator BreathingPattern()
    {
        while (true)
        {
            float time = Mathf.PingPong(Time.time * patternSpeedMultiplier, 1f);
            lightComponent.intensity = Mathf.Lerp(0, brightness, time);
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Repaint the inspector to update the help box visibility
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this != null) // Ensure object still exists
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
        };
    }

    [CustomEditor(typeof(LightController))]
    public class LightControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector fields
            DrawDefaultInspector();

            // Get the reference to the Light component
            LightController lightComponent = (LightController)target;

            // Check if brightness exceeds 22500
            if (lightComponent.brightness > 500)
            {
                // Display a help box in the inspector
                EditorGUILayout.HelpBox("Brightness is very high! This may cause excessive glare or performance issues.", MessageType.Warning);
            }
        }
    }
#endif
}
