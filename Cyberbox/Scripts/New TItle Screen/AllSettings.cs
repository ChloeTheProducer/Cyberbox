using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class AllSettings : MonoBehaviour
{
    [field: Header("Video Settings")] 
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown shadowQualityDropdown;
    [SerializeField] private TMP_Dropdown motionBlurDropdown;

    [field: Header("Audio Settings")]
    [SerializeField] private TMP_Dropdown soundtrackDropdown;
    [SerializeField] private AudioClip[] soundtracks;


    public void SetQualityLevelDropdown(int index)
    {   
        QualitySettings.SetQualityLevel(index, false);
        PlayerPrefs.SetInt("GameSettings: QualityLevel", index);
    }
    void Start()
    {
        int soundtrackIndex = PlayerPrefs.GetInt("GameSettings: Soundtrack");

        if (soundtrackIndex == 0)
        {
            soundtrackIndex = 1;
        }
        GameObject.Find("UI").GetComponent<AudioSource>().clip = soundtracks[soundtrackIndex];
        GameObject.Find("UI").GetComponent<AudioSource>().Play();

        // Clear existing options
        soundtrackDropdown.ClearOptions();

        // Create a list to hold the option names
        List<string> optionNames = new List<string>();

        // Add the names of the audio clips to the optionNames list
        foreach (AudioClip soundtrack in soundtracks)
        {
            optionNames.Add(soundtrack.name);
        }

        // Set the options of the dropdown to the names of the audio clips
        soundtrackDropdown.AddOptions(optionNames);
    }

    public void SetAntiAliasingDropdown(int index)
    {   
        switch(index)
        {
            case 0:
                // None
                QualitySettings.antiAliasing = 0;
            break;
            case 1: // 2x
                QualitySettings.antiAliasing = 2;
            break;
            case 2: // 4x
                QualitySettings.antiAliasing = 4;
            break;
            case 3: // 8x
                QualitySettings.antiAliasing = 8;
            break;
        }
        PlayerPrefs.SetInt("GameSettings: AntiAliasing", index);
    }
    public void SetShadowQualityDropdown(int index)
    {   
        switch(index)
        {
            case 0:
                QualitySettings.shadows = ShadowQuality.Disable;
            break;
            case 1:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.Low;
            break;
            case 2:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
            break;
            case 3:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.High;
            break;
            case 4:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
            break;
        }
        PlayerPrefs.SetInt("GameSettings: ShadowResolution", index);
    }
    public void SetMotionBlurDropdown(int index)
    {   
        QualitySettings.SetQualityLevel(index, false);
        PlayerPrefs.SetInt("GameSettings: QualityLevel", index);
    }

    public void SetSoundtrackDropdown(int index)
    {
        GameObject.Find("UI").GetComponent<AudioSource>().clip = soundtracks[index];
        GameObject.Find("UI").GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt("GameSettings: Soundtrack", index);
    }
}








