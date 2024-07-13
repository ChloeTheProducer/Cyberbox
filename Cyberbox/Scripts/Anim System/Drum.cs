using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour
{
    public AudioClip[] collisionSounds;
    public bool changePitch;
    public AudioSource audioSource;
    private void OnTriggerEnter(Collider collider)
    {
        if (collisionSounds != null) // unity seriously doesn't return an audio source so i've gotta improvise
        {
            audioSource.mute = false;
            if (changePitch) { audioSource.pitch = Random.Range((float)0.97, (float)1.03); } else { audioSource.pitch = 1; }
            audioSource.PlayOneShot(collisionSounds[Random.Range(0, collisionSounds.Length)]);
        }
    }


}
