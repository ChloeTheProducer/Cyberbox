using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SHWHandler : MonoBehaviour
{
    PlaybackSystem playbackSystem;
    BitArray[] signals;
    MackValves valves;

    float timeSongStarted = 0;
    float timeSongOffset = 0;
    float timePauseStart = 0;
    float timeInputSpeedStart = 0;
    int previousFramePosition = 0;
    bool previousAnyButtonHeld = false;
    float dataStreamedFPS = 60.0f;

    rshwFormat showtape;

    void Start()
    {
        playbackSystem = GetComponent<PlaybackSystem>();
        valves = GameObject.FindGameObjectWithTag("Mack Valves").GetComponent<MackValves>();
    }
    public void Load(string path)
    {
        showtape = rshwFormat.ReadFromFile(path);
        playbackSystem.audioSource.clip = OpenWavParser.ByteArrayToAudioClip(showtape.audioData);

        List<BitArray> newSignals = new List<BitArray>();

        int countlength = 0;

        if (showtape.signalData[0] != 0)
        {
            countlength = 1;
            BitArray bit = new BitArray(300);
            newSignals.Add(bit);
        }
        for (int i = 0; i < showtape.signalData.Length; i++)
        {
            if (showtape.signalData[i] == 0)
            {
                countlength += 1;
                BitArray bit = new BitArray(300);
                newSignals.Add(bit);
            }
            else
            {
                newSignals[countlength - 1].Set(showtape.signalData[i] - 1, true);
            }
        }

        signals = newSignals.ToArray();
        playbackSystem.audioSource.Play();
    }

    public void PlaybackUpdate()
    {
        if (signals != null)
        {
            for (int i = 0; i < valves.Bits.Length; i++)
            {
                valves.Bits[i] = false;
            }

            int arrayDestination = (int)(playbackSystem.audioSource.time * dataStreamedFPS);

            //Check if new frames need to be created
            if (arrayDestination >= signals.Length && signals.Length != 0)
            {
                arrayDestination = signals.Length;
            }

            //Apply the current frame of simulation data to the Mack Valves
            if (arrayDestination < signals.Length)
            {
                for (int i = 0; i < 150; i++)
                {
                    if (signals[arrayDestination].Get(i))
                    {
                        valves.Bits[i] = true;
                    }
                    if (signals[arrayDestination].Get(i + 150))
                    {
                        valves.Bits[i + 150] = true;
                    }
                }
            }


            if (playbackSystem.audioSource.time >= playbackSystem.audioSource.clip.length * playbackSystem.audioSource.clip.channels)
            {
                Debug.Log("Song over");
                previousFramePosition = arrayDestination;
                signals = null;
                playbackSystem.Unload();

            }
        }
    }
}
