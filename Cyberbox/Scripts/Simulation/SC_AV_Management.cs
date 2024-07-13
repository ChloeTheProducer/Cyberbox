using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class SC_AV_Management : MonoBehaviour
{
    public VideoClip noSignalClip;

    [HideInInspector] public string videoPath;
    [HideInInspector] public string audioPath;

    [HideInInspector] public List<AudioSource> leftSpeakers;
    [HideInInspector] public List<AudioSource> rightSpeakers;

    VideoPlayer player;
    [HideInInspector] public UI_ShowtapeManager manager;
    UI_PlayRecord playRecord;

    public SC_Controller CustomController;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        manager = GetComponent<UI_ShowtapeManager>();
        playRecord = GetComponent<UI_PlayRecord>();

        GameObject[] SpeakerObjects = GameObject.FindGameObjectsWithTag("Speaker");

        foreach (GameObject obj in SpeakerObjects)
        {
            if (obj.name == "Speaker L")
            {
                leftSpeakers.Add(obj.GetComponent<AudioSource>());
            }
            if (obj.name == "Speaker R")
            {
                rightSpeakers.Add(obj.GetComponent<AudioSource>());
            }
        }
    }

    /// <summary>
    /// Loads audio and video of a show
    /// </summary>
    public async void Load()
    {
        bool VideoExists;
        manager.referenceSpeaker.clip = manager.speakerClip;
        for (int i = 0; i < leftSpeakers.Count; i++)
        {
            leftSpeakers[i].clip = manager.speakerClip;
        }
        for (int i = 0; i < rightSpeakers.Count; i++)
        {
            rightSpeakers[i].clip = manager.speakerClip;
        }
        if (videoPath != "")
        {
            VideoExists = true;
            player.url = videoPath;
            player.Pause();
        }
        else
        {
            VideoExists = false;
            manager.useVideoAsReference = false;
            videoPath = "";
            player.clip = noSignalClip;
        }
            
        if (VideoExists == true)
        {
            await UniTask.WaitUntil(() => player.isPrepared == true);
            player.targetTexture.Release();
            player.targetTexture.width = (int)player.width;
            player.targetTexture.height = (int)player.height;
            player.targetTexture.Create();
        }

        player.Play();
        manager.Play(true, true);

        Sync();
        playRecord.SwitchWindow(17);
        playRecord.playMultiText.GetComponent<PlayMenuManager>().TextUpdate(false);

        if (CustomController)
        {
            CustomController.ShowtapePlaying = true;
            CustomController.TapeLoaded = true;
        }

    }

    /// <summary>
    /// Pauses audio and video.
    /// </summary>
    public void Pause()
    {
        Debug.Log("Audio Video Pause");
        if (videoPath != "")
        {
            player.Pause();
        }
        manager.referenceSpeaker.Pause();
        for (int i = 0; i < leftSpeakers.Count; i++)
        {
            leftSpeakers[i].Pause();
        }
        for (int i = 0; i < rightSpeakers.Count; i++)
        {
            rightSpeakers[i].Pause();
        }
        Sync();
    }

    /// <summary>
    /// Plays audio and video.
    /// </summary>
    public void Resume()
    {
        Debug.Log("Audio Video Pause");
        if (videoPath != "")
        {
            player.Play();
        }
        manager.referenceSpeaker.Play();
        for (int i = 0; i < leftSpeakers.Count; i++)
        {
            leftSpeakers[i].Play();
        }
        for (int i = 0; i < rightSpeakers.Count; i++)
        {
            rightSpeakers[i].Play();
        }
        Sync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Ensures audio and video is synced when the showtape is playing.
    /// </summary>
    public void Sync()
    {
        if (!manager.useVideoAsReference)
        {
            if (videoPath != "")
            {
                player.time = manager.referenceSpeaker.time;
            }
            for (int i = 0; i < leftSpeakers.Count; i++)
            {
                leftSpeakers[i].time = manager.referenceSpeaker.time;
            }
            for (int i = 0; i < rightSpeakers.Count; i++)
            {
                rightSpeakers[i].time = manager.referenceSpeaker.time;
            }
        }
    }


    /// <summary>
    /// Increases the speed of audio and video.
    /// </summary>
    /// <param name="input"></param>
    public void FFSong(int input)
    {
        if (input == -1)
        {
            PitchBackward();
        }
        else if (input == 0)
        {
            manager.referenceSpeaker.pitch = 1;
        }
        else
        {
            PitchForward();
        }
        for (int i = 0; i < leftSpeakers.Count; i++)
        {
            leftSpeakers[i].pitch = manager.referenceSpeaker.pitch;
        }
        for (int i = 0; i < rightSpeakers.Count; i++)
        {
            rightSpeakers[i].pitch = manager.referenceSpeaker.pitch;
        }
        if (videoPath != "")
        {
            player.playbackSpeed = manager.referenceSpeaker.pitch;
        }
        Sync();
    }

    /// <summary>
    /// Pitches forward the audio and video by one setting.
    /// </summary>
    public void PitchForward()
    {
        if (!manager.playMovements)
        {
            manager.referenceSpeaker.pitch = 0;
            manager.Play(true, true);
        }
        switch (manager.referenceSpeaker.pitch)
        {
            case -100:
                manager.referenceSpeaker.pitch = -10;
                break;
            case -10:
                manager.referenceSpeaker.pitch = -5;
                break;
            case -5:
                manager.referenceSpeaker.pitch = -2;
                break;
            case -2:
                manager.referenceSpeaker.pitch = -1;
                break;
            case -1:
                manager.referenceSpeaker.pitch = -0.5f;
                break;
            case -0.5f:
                manager.referenceSpeaker.pitch = 0.5f;
                break;
            case 0:
                manager.referenceSpeaker.pitch = 0.5f;
                break;
            case 0.5f:
                manager.referenceSpeaker.pitch = 1f;
                break;
            case 1:
                manager.referenceSpeaker.pitch = 2f;
                break;
            case 2:
                manager.referenceSpeaker.pitch = 5f;
                break;
            case 5:
                manager.referenceSpeaker.pitch = 10f;
                break;
            case 10:
                manager.referenceSpeaker.pitch = 100f;
                break;
            default:
                break;
        }
        manager.syncTvsAndSpeakers.Invoke();
    }

    /// <summary>
    /// Pitches backward the audio and video by one setting.
    /// </summary>
    public void PitchBackward()
    {
        if (!manager.playMovements)
        {
            manager.referenceSpeaker.pitch = 0;
            manager.Play(true, true);
        }
        switch (manager.referenceSpeaker.pitch)
        {
            case 100:
                manager.referenceSpeaker.pitch = 10;
                break;
            case 10:
                manager.referenceSpeaker.pitch = 5;
                break;
            case 5:
                manager.referenceSpeaker.pitch = 2;
                break;
            case 2:
                manager.referenceSpeaker.pitch = 1;
                break;
            case 1:
                manager.referenceSpeaker.pitch = 0.5f;
                break;
            case 0.5f:
                manager.referenceSpeaker.pitch = -0.5f;
                break;
            case 0:
                manager.referenceSpeaker.pitch = -0.5f;
                break;
            case -0.5f:
                manager.referenceSpeaker.pitch = -1f;
                break;
            case -1:
                manager.referenceSpeaker.pitch = -2f;
                break;
            case -2:
                manager.referenceSpeaker.pitch = -5f;
                break;
            case -5:
                manager.referenceSpeaker.pitch = -10f;
                break;
            case -10:
                manager.referenceSpeaker.pitch = -100f;
                break;
            default:
                break;
        }
        if (videoPath != "")
        {
            player.playbackSpeed = manager.referenceSpeaker.pitch;
        }
        manager.syncTvsAndSpeakers.Invoke();
    }
}
