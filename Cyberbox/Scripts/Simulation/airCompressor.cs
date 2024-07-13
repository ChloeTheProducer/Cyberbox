using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

// This script is for the air compressor... ばか

public class airCompressor : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip compressor;
    private bool CurrentlyPressedKeys;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public HashSet<KeyCode> currentlyPressedKeys = new HashSet<KeyCode>();

    private void OnGUI()
    {
        if (!Event.current.isKey) return;

        if (Event.current.keyCode != KeyCode.None)
        {
            if (Event.current.type == EventType.KeyDown)
            {
                currentlyPressedKeys.Add(Event.current.keyCode);
            }
            else if (Event.current.type == EventType.KeyUp)
            {
                currentlyPressedKeys.Remove(Event.current.keyCode);
            }
        }

        // Check for F
        if (Event.current.keyCode == KeyCode.F)
        {
            Debug.Log("Compressor on");
            
            gameObject.GetComponent<AudioReverbFilter>().enabled = true;
            audioSource.volume = 1f;
            audioSource.PlayOneShot(compressor);
        }

        // Disable F key for "" seconds when pressed while compressor audio plays, so audio cant overlap
        
    }
}