using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[System.Serializable]

[RequireComponent(typeof(Animator))]
public class ValveSounds : MonoBehaviour
{
    AudioClip on;
    AudioClip off;
    AudioClip tolomatic;
    AudioClip curtain;

    Animator animator;
    AudioSource audioSource;

    void Awake()
    {
        on = (AudioClip)Resources.Load("Audio/Valve_On");
        off = (AudioClip)Resources.Load("Audio/Valve_Off");

        animator = GetComponent<Animator>();
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];
            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationEndHandler";

            clip.AddEvent(animationStartEvent);
            clip.AddEvent(animationEndEvent);
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.5f;
    }

    public void AnimationStartHandler()
    {
        audioSource.clip = on;
        audioSource.pitch = UnityEngine.Random.Range((float)0.9, (float)1.1);
        audioSource.Play();
    }
    public void AnimationEndHandler()
    {
        audioSource.clip = off;
        audioSource.pitch = UnityEngine.Random.Range((float)0.9, (float)1.1);
        audioSource.Play();
    }
}