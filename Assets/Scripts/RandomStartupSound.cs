using UnityEngine;

public class RandomStartupSound : MonoBehaviour
{
    [Header("Random Startup Sound")]
    public AudioClip[] startupSounds; // Array to store the boot sounds
    private AudioSource audioSource;
    public float volume;
    
    // I can't believe setting up a script to play a random boot sound on play which is very easy, is so damn hard for my brain to understand how to do that I
    // either need to reuse an already existing script or use chatgpt, which I'm not doing here as I understand I actually need to learn these things
    // if I wanna make more software in the future
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetRandomStartSound();
        volume = 0.25f;
    }
    
    void SetRandomStartSound()
    {
        if (startupSounds.Length > 0)
        {
            // Select a random clip from the array
            AudioClip randomClip = startupSounds[Random.Range(0, startupSounds.Length)];
            audioSource.clip = randomClip;
            audioSource.Play();
            audioSource.volume = volume;
        }
        
    }
}
