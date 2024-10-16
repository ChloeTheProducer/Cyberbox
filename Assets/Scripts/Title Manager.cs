using UnityEngine;
using System;
using TMPro;

/// <summary>
/// This handles the messagebox window system, despite the name. Also some GUI window stuff
/// </summary>
public class TitleManager : MonoBehaviour
{
    public bool standaloneMode;

    [Header("Day Soundtrack System")]
    private AudioSource audioSource;
    public AudioClip[] dayClips; // An array to store clips for each day
    public float volume;

    public TextMeshProUGUI songCredits;

    void Start()
    {
        
        if (standaloneMode != true)
        {
            volume = 0.5f;
            audioSource = GetComponent<AudioSource>();
            DayOfWeek wk = DateTime.Today.DayOfWeek;

            // Check if wk is within the valid range (0-6) and play the corresponding clip
            if ((int)wk >= 0 && (int)wk < dayClips.Length)
            {
                audioSource.clip = dayClips[(int)wk];
                audioSource.Play();
                audioSource.volume = volume;

                // Set the songCredits text with information about the song playing
                songCredits.text = $"{dayClips[(int)wk].name}";
            }
        }
    }
}