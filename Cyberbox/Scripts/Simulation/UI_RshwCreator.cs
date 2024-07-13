using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Threading.Tasks;
using TMPro;
using NAudio.Wave;
using File = System.IO.File;
using Cysharp.Threading.Tasks;
using System;
using SimpleFileBrowser;
using Unity.VisualScripting;
using UnityEngine.Windows;

/// <summary>
///  This is the file format for the unsafe ".*shw" format. This is deprecated and should not be used
/// </summary>
public class UI_RshwCreator : MonoBehaviour
{
    public TMP_InputField Name;
    public TMP_InputField Creator;
    public TMP_InputField Description;
    public TMP_Dropdown Compression;

    [HideInInspector] public bool showtapeCreated;

    UI_ShowtapeManager manager;
    UI_PlayRecord playRecord;
    SC_AV_Management avManagement;

    [HideInInspector] public string showtapeURL;

    void Start()
    {
        playRecord = GetComponent<UI_PlayRecord>();
        manager = GetComponent<UI_ShowtapeManager>();
        avManagement = GetComponent<SC_AV_Management>();
    }

    public enum addWavResult
    {
        none,
        noSource,
        uncompressed,
    }
    public addWavResult AddWav()
    {
        manager.speakerClip = null;
        CursorLockMode lockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Adding Wav");
        //Call File Browser
        avManagement.audioPath = "";
        manager.showtapeSegmentPaths[0] = "";
        StartCoroutine(SaveFile());
        if (FileBrowser.Success)
        {
            if (FileBrowser.Result != null)
            {
                avManagement.audioPath = FileBrowser.Result[0];
                manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(FileBrowser.Result[0]));
                manager.audioVideoGetData.Invoke();
                CreateBitArray();
                if (manager.speakerClip == null)
                {
                    return addWavResult.uncompressed;
                }
                else
                {
                    return addWavResult.noSource;
                }
            }
            return addWavResult.none;
        }
        return addWavResult.noSource;
    }

    public IEnumerator SaveFile()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Showtape", "Load");
    }

    public void AddAudioFile()
    {
        StartCoroutine(AddWavSpecial());
    }

    public IEnumerator AddWavSpecial()
    {
        CursorLockMode lockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Adding Audio File");

        FileBrowser.SetFilters(false, new FileBrowser.Filter("Audio Files", ".wav", ".mp3", ".aac", ".wma"));
        FileBrowser.SetDefaultFilter(".mp3");

        //Call File Browser
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Showtape", "Load");

        avManagement.audioPath = "";
        manager.showtapeSegmentPaths[0] = "";
        if (FileBrowser.Success)
        {
            if (FileBrowser.Result != null)
            {
                var paths = FileBrowser.Result;
                string fileformat = Path.GetExtension(paths[0]);
                if (fileformat == ".mp3")
                {
                    using (var reader = new Mp3FileReader(paths[0]))
                    {
                        WaveFileWriter.CreateWaveFile("C:\\Temp\\converted.wav", reader);
                        paths[0] = "C:\\Temp\\converted.wav";
                    }
                }
                if (fileformat == ".aac" || fileformat == ".wma")
                {
                    using (var reader = new Mp3FileReader(paths[0]))
                    {
                        WaveFileWriter.CreateWaveFile("C:\\Temp\\converted.wav", reader);
                        paths[0] = "C:\\Temp\\converted.wav";
                    }
                }
                avManagement.audioPath = paths[0];
                manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(FileBrowser.Result[0]));
                CreateBitArray();
                Debug.Log("Successfully added Audio File");
            }
        }
        Cursor.lockState = lockState;
    }

    public void StartNewShow()
    {
        Debug.Log("Starting New Show");
        manager.disableCharactersOnStart = false;
        manager.recordMovements = true;
        if (avManagement.audioPath != "")
        {
            CreateBitArray();
        }
    }

    void CreateBitArray()
    {
        manager.rshwData = new BitArray[100];
        for (int i = 0; i < manager.rshwData.Length; i++)
        {
            manager.rshwData[i] = new BitArray(300);
        }
    }

    public void SaveRecording()
    {
        // Stop Show
        if (manager.rshwData != null)
        {
            manager.audioVideoPause.Invoke();
            manager.recordMovements = false;
            manager.playMovements = false;
            var shw = new rshwFormat { audioData = OpenWavParser.AudioClipToByteArray(manager.speakerClip) };
            List<int> converted = new List<int>();
            int bitCount = 0; // Counter variable for tracking the number of bits

            for (int i = 0; i < manager.rshwData.Length; i++)
            {
                converted.Add(0);
                for (int e = 0; e < 300; e++)
                {
                    if (manager.rshwData[i].Get(e) == true)
                    {
                        converted.Add(e + 1);
                        bitCount++; // Increment the counter for each bit added
                    }
                }
            }

            shw.signalData = converted.ToArray();
            shw.Save(manager.showtapeSegmentPaths[0]);
            Debug.Log("Showtape Saved");
        }
        else
        {
            Debug.Log("No Showtape. Did not save.");
        }
    }

    public void SaveNewShowtape()
    {
        StartCoroutine(SaveRecordingAs(true));
    }
    public void SaveRecordedShowtape()
    {
        StartCoroutine(SaveRecordingAs(false));
    }
    public IEnumerator SaveRecordingAs(bool NewShowtape)
    {
        CursorLockMode lockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        //Stop Show
        manager.audioVideoPause.Invoke();
        manager.recordMovements = false;
        manager.playMovements = false;
        if (manager.speakerClip != null)
        {
            var path = showtapeURL;

            if (!NewShowtape)
            {
                manager.showtapeSegmentPaths = new string[1];
                manager.showtapeSegmentPaths[0] = path;
                var shw = new rshwFormat { audioData = OpenWavParser.AudioClipToByteArray(manager.speakerClip) };
                List<int> converted = new List<int>();
                for (int i = 0; i < manager.rshwData.Length; i++)
                {
                    converted.Add(0);
                    for (int e = 0; e < 300; e++)
                    {
                        if (manager.rshwData[i].Get(e) == true)
                        {
                            converted.Add(e + 1);
                        }
                    }
                }
                shw.signalData = converted.ToArray();
                FileBrowser.SetFilters(false, new FileBrowser.Filter("Showtape", manager.fileExtention));
                yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Save Showtape File", "Save");
                shw.Save(FileBrowser.Result[0]);
                Debug.Log("Showtape Saved: " + path);
                playRecord.SwitchWindow(11);
            }
            else
            {
                FileBrowser.SetFilters(false, new FileBrowser.Filter("Showtape", manager.fileExtention));
                yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Save Showtape File", "Save");
                if (FileBrowser.Success)
                {
                    path = FileBrowser.Result[0];
                    manager.showtapeSegmentPaths = new string[1];
                    manager.showtapeSegmentPaths[0] = path;

                    var shw = new rshwFormat { 
                    audioData = OpenWavParser.AudioClipToByteArray(manager.speakerClip), 
                    creator = "",
                    description = "",
                    name = ""
                        };

                    List<int> converted = new List<int>();
                    for (int i = 0; i < manager.rshwData.Length; i++)
                    {
                        converted.Add(0);
                        for (int e = 0; e < 300; e++)
                        {
                            if (manager.rshwData[i].Get(e) == true)
                            {
                                converted.Add(e + 1);
                            }
                        }
                    }
                    shw.signalData = converted.ToArray();
                    shw.Save(path);

                    if (string.IsNullOrEmpty(path))
                    {
                        Debug.Log("No Showtape. Did not save.");
                        AudioSource sc = GameObject.Find("GlobalAudio").GetComponent<AudioSource>();
                        sc.volume = 1;
                        sc.PlayOneShot((AudioClip)Resources.Load("Deny"));
                        Cursor.lockState = lockState;
                        showtapeCreated = false;
                    }
                    playRecord.SwitchWindow(4);
                    Debug.Log("Showtape Saved: " + path);
                }
            }
        }
        Cursor.lockState = lockState;
        showtapeCreated = true;
    }

    public async void LoadFromURL(string url)
    {
        playRecord.SwitchWindow(33);
        await LoadRoutine(url);
    }

    public async UniTask LoadRoutine(string url)
    {
        showtapeURL = url;
        manager.disableCharactersOnStart = false;
        manager.playMovements = false;

        if (!string.IsNullOrEmpty(url))
        {
            try
            {
                manager.referenceSpeaker.volume = manager.refSpeakerVol;
                manager.referenceSpeaker.time = 0;
                manager.useVideoAsReference = false;
                manager.referenceVideo.time = 0;
                manager.timeSongStarted = 0;
                manager.timeSongOffset = 0;
                manager.timePauseStart = 0;
                manager.timeInputSpeedStart = 0;

                await UniTask.NextFrame();

                manager.curtainOpen.Invoke();

                await UniTask.NextFrame();

                rshwFormat thefile = await rshwFormat.ReadFromFile(url);

                await UniTask.NextFrame();

                manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(thefile.audioData);

                await UniTask.NextFrame();

                List<BitArray> newSignals = new List<BitArray>();
                int countlength = 0;

                if (thefile.signalData[0] != 0)
                {
                    countlength = 1;
                    BitArray bit = new BitArray(300);
                    newSignals.Add(bit);
                }

                for (int i = 0; i < thefile.signalData.Length; i++)
                {
                    if (thefile.signalData[i] == 0)
                    {
                        countlength += 1;
                        BitArray bit = new BitArray(300);
                        newSignals.Add(bit);
                    }
                    else
                    {
                        newSignals[countlength - 1].Set(thefile.signalData[i] - 1, true);
                    }
                }

                manager.rshwData = newSignals.ToArray();

                await UniTask.NextFrame();

                if (File.Exists(url.Remove(url.Length - Mathf.Max(manager.fileExtention.Length, 4)) + "mp4"))
                {
                    Debug.Log("Video Found for Showtape.");
                    avManagement.videoPath = url.Remove(url.Length - Mathf.Max(manager.fileExtention.Length, 4)) + "mp4";
                }
                else
                {

                }

                manager.audioVideoGetData.Invoke();

                await UniTask.NextFrame();

                if (manager.recordMovements)
                {
                    Debug.Log("Recording Showtape: " + url + " (Length: " + ((float)countlength / manager.dataStreamedFPS) + ")");
                }
                else
                {
                    Debug.Log("Playing Showtape: " + url + " (Length: " + ((float)countlength / manager.dataStreamedFPS) + ")");
                }

                await UniTask.NextFrame();

                manager.timeSongStarted = Time.time;
                manager.syncTvsAndSpeakers.Invoke();
                Debug.Log("Length = " + manager.referenceSpeaker.clip.length + " Channels = " + manager.referenceSpeaker.clip.channels + " Total = " + manager.referenceSpeaker.clip.length / manager.referenceSpeaker.clip.channels);
                playRecord.SwitchWindow(17);
            }
            catch (Exception ex)
            {
                Debug.LogError("Showtape failed to start. " + ex.Message);
                playRecord.SwitchWindow(1);
            }
        }
    }


    public void EraseShowtape()
    {
        try
        {
            manager.disableCharactersOnStart = false;
            manager.playMovements = false;
            manager.referenceSpeaker.time = 0;
            manager.timeSongStarted = 0;
            manager.timeSongOffset = 0;
            manager.timePauseStart = 0;
            manager.timeInputSpeedStart = 0;
            manager.curtainOpen.Invoke();
            manager.speakerClip = null;
            manager.rshwData = null;
            avManagement.videoPath = "";
            manager.referenceSpeaker.clip = null;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Exception: " + ex.ToString());
        }
    }

    /// <summary>
    /// Saves a showtape while returning to the Create Recording menu.
    /// This is called when creating a new showtape.
    /// </summary>
    /// <param name="input"></param>
    public void SpecialSaveAs(int input)
    {
        if (input == 11)
        {
            playRecord.SwitchWindow(input);
        }
        else
        {
            StartCoroutine(SaveRecordingAs(true));
        }
    }

    public async void ReplaceShowAudio()
    {
        //Call File Browser
        manager.showtapeSegmentPaths = new string[1];
        await FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Showtape", "Load");
        if (FileBrowser.Success)
        {
            var paths = FileBrowser.Result;
            if (paths.Length > 0)
            {
                manager.showtapeSegmentPaths[0] = paths[0];
                manager.currentShowtapeSegment = 0;
                manager.referenceSpeaker.time = 0;
                manager.playMovements = false;
                //Check if null
                if (manager.showtapeSegmentPaths[0] != "")
                {
                    rshwFormat thefile = await rshwFormat.ReadFromFile(manager.showtapeSegmentPaths[0]);
                    List<BitArray> newSignals = new List<BitArray>();
                    int countlength = 0;
                    if (thefile.signalData[0] != 0)
                    {
                        countlength = 1;
                        BitArray bit = new BitArray(300);
                        newSignals.Add(bit);
                    }
                    for (int i = 0; i < thefile.signalData.Length; i++)
                    {
                        if (thefile.signalData[i] == 0)
                        {
                            countlength += 1;
                            BitArray bit = new BitArray(300);
                            newSignals.Add(bit);
                        }
                        else
                        {
                            newSignals[countlength - 1].Set(thefile.signalData[i] - 1, true);
                        }
                    }
                    manager.rshwData = newSignals.ToArray();

                    await FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Audio", "Load Audio");
                    paths = FileBrowser.Result;
                    if (paths.Length > 0)
                    {
                        if (paths[0] != "")
                        {
                            avManagement.audioPath = paths[0];
                            manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(paths[0]));
                            SaveRecording();
                        }
                    }
                }
            }
        }
    }
}