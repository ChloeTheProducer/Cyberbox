using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using static OpenWavParser;

public class GameSettings : MonoBehaviour
{
    public Button Left;
    public Button Right;
    public TMP_Text text;
    private Button Apply;
    public enum Option
    {
        experimental,
        devconsole,
    }
    public Option option;
    private string[] options;

    private int Pos = 1;
    private int minValue = 1;
    private int maxValue;

    // Start is called before the first frame update
    void Start()
    {
        Left.onClick.AddListener(delegate { ChangeSetting(-1); });
        Right.onClick.AddListener(delegate { ChangeSetting(1); });


        if (option == Option.experimental || option == Option.devconsole)
        {
            options = new string[]
            {
                "Off",
                "On",
            };
        }
        maxValue = options.Length;

        LoadSettings();
    }
    public void ChangeSetting(int change)
    {
        Pos += change;
        Pos = Mathf.Clamp(Pos, minValue, maxValue);  // Ensure Pos stays within the range
        UpdateButtonVisibility();
        text.text = Pos.ToString();

        if (option == Option.experimental)
        {
            text.text = options[Pos - 1];
            PlayerPrefs.SetInt("Game Settings: Experimental", Pos);
        }
        if (option == Option.devconsole)
        {
            text.text = options[Pos - 1];
            PlayerPrefs.SetInt("Game Settings: DevConsole", Pos);
        }
        else
        {
            Debug.LogWarning("Missing setting from ChangeSetting function. Gary will be mad.");
        }
    }

    // Function to update button visibility based on the current Pos
    void UpdateButtonVisibility()
    {
        if (Pos <= minValue)
        {
            Left.interactable = false;
        }
        else
        {
            Left.interactable = true;
        }

        if (Pos >= maxValue)
        {
            Right.interactable = false;
        }
        else
        {
            Right.interactable = true;
        }
    }
    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        switch (option)
        {
            case Option.experimental:
                Pos = PlayerPrefs.GetInt("Game Settings: Experimental", 1);
                break;
            case Option.devconsole:
                Pos = PlayerPrefs.GetInt("Game Settings: DevConsole", 1);
                break;
            default:
                Debug.LogError("Failed to load settings. You forgot to add the setting to the loadsettings function, dummy.");
                break;
        }
        text.text = options[Pos - 1];
    }
}