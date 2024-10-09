using UnityEngine;
using System;

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

    void Start()
    {
        
        if (standaloneMode != true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            volume = 0.2f;
            audioSource = GetComponent<AudioSource>();
            DayOfWeek wk = DateTime.Today.DayOfWeek;

            // Check if wk is within the valid range (0-6) and play the corresponding clip
            if ((int)wk >= 0 && (int)wk < dayClips.Length)
            {
                audioSource.clip = dayClips[(int)wk];
                audioSource.Play();
                audioSource.volume = volume;
            }
        }
    }
}