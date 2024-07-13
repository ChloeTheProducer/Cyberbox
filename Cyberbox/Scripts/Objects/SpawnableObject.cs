using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpawnableObject : MonoBehaviour
{
    public bool EnableSounds;

    AudioSource audioSource;

    AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        audioClips = Resources.LoadAll<AudioClip>("CollisionSounds");
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 1f;
        audioSource.loop = false;
    }
    void OnCollisionEnter()
    {
        if (EnableSounds == true)
        {
            audioSource.pitch = Random.Range(0.97f, 1.03f);
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        }
    }
}
