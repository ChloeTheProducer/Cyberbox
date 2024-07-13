using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using static Player;
using Unity.VisualScripting;

public class UI_SidePanel : MonoBehaviour
{
    //Windows
    GameObject WindowMenu;
    GameObject WindowSound;
    GameObject WindowShow;
    GameObject WindowCamera;
    GameObject WindowFlows;
    GameObject WindowCustomProperties;

    //Text
    public Text psiText;
    public Text volumeText;

    int currentSpacial = 3;
    public Text spacialText;
    public Text curtainText;
    public Text bonesText;
    public Text upperLightText;
    public Text camFilterText;
    public Text camSmoothText;
    public Text soundvolumeText;
    Text signalSwapText;

    [HideInInspector] public bool dynamicBonesEnabled;


    public Text intermissionMusicText;
    AudioClip[] intermissionAudios;
    int currentMusicTrack = 0;

    //Cam Filters
    int currentCamProfile = 0;
    VolumeProfile[] camProfiles;

    //Flow Controls
    int flowProfile = 0;
    int flowNumber = 0;
    Text flowProfileText;
    Text flowNumText;
    Text flowSpeedInText;
    Text flowSpeedOutText;
    Text flowWeightInText;
    Text flowWeightOutText;
    Text flowSlamInText;
    Text flowSlamOutText;
    Text flowSlamSpeedInText;
    Text flowSlamSpeedOutText;
    public string FileExtention;

    public GameObject ShowTemplate;

    //Other
    GameObject areaLights;
    GameObject MainWindow;
    DynamicBone[] allDynamics;
    UI_PlayRecord showPanelUI;
    SC_AV_Management avManagement;

    bool hidepanels = false;

    public float[] copyPasteValues = new float[8];
    private void Awake()
    {
        StartCoroutine(AwakeCoroutine());
    }

    IEnumerator AwakeCoroutine()
    {
        Transform Parent = transform.parent;
        MainWindow = Parent.transform.Find("Main").gameObject;
        LeanTween.alphaCanvas(MainWindow.GetComponent<CanvasGroup>(), 0, 0.1f);
        GameObject Container = MainWindow.transform.Find("Container").gameObject;
        WindowSound = Container.transform.Find("Sound").gameObject;
        WindowShow = Container.transform.Find("Show").gameObject;
        WindowCamera = Container.transform.Find("Camera").gameObject;
        WindowCustomProperties = Container.transform.Find("CustomSettings").gameObject;
        VolumeProfile[] profiles = Resources.LoadAll<VolumeProfile>("Cam Filters");

        camProfiles = new VolumeProfile[profiles.Length];
        for (int i = 0; i < profiles.Length; i++) 
        {
            camProfiles[i] = profiles[i];
        }

        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio/Intermission");

        intermissionAudios = new AudioClip[audioClips.Length];
        for (int i = 0; i < audioClips.Length; i++)
        {
            intermissionAudios[i] = audioClips[i];
        }

        showPanelUI = GameObject.Find("Show Selector/UI").GetComponent<UI_PlayRecord>();
        avManagement = GameObject.Find("Show Selector/UI").GetComponent<SC_AV_Management>();
        areaLights = transform.root.Find("Area Lights").gameObject;

        yield return new WaitForSeconds(1f);
        allDynamics = FindObjectsOfType(typeof(DynamicBone)) as DynamicBone[];
        yield return new WaitForSeconds(.2f);
        FlowUpdate();
        yield return new WaitForSeconds(.2f);
        showPanelUI.thePlayer = GameObject.Find("Player");
        if (showPanelUI.thePlayer != null)
        {
            VHSToggle(0);
        }
        AudioSource gg = GameObject.Find("GlobalAudio").GetComponent<AudioSource>();
        soundvolumeText.GetComponent<Text>().text = Mathf.Ceil(gg.volume * 100).ToString();

        // my lazy ass doesn't want to bother making a silent audio clip
        AudioClip silentClip = AudioClip.Create("None", 1, 1, 44100, false);
        intermissionAudios = new AudioClip[] { silentClip }.Concat(intermissionAudios).ToArray();

        SpacialToggle(0);
        DisableValveSounds();


        psiText.text = ((GameObject.Find("Mack Valves").GetComponent<Mack_Valves>().PSI * 2).ToString() + " PSI");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            avManagement.Pause();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (MainWindow.GetComponent<CanvasGroup>().alpha == 1)
            {
                // If the window is active, fade out and slide out simultaneously
                LeanTween.alphaCanvas(MainWindow.GetComponent<CanvasGroup>(), 0, 0.25f);
                GameObject.Find("Player").GetComponent<Player>().playerState = PlayerState.normal;
                MainWindow.SetActive(false);
            }
            else
            {
                MainWindow.SetActive(true);
                LeanTween.alphaCanvas(MainWindow.GetComponent<CanvasGroup>(), 1, 0.25f);
                GameObject.Find("Player").GetComponent<Player>().playerState = PlayerState.frozenAllUnlock;
            }
        }
    }

    public void Upperlights(int input)
    {
        AudioSource sc = GameObject.Find("GlobalAudio").GetComponent<AudioSource>(); Resources.Load("ting");
        sc.clip = (AudioClip)Resources.Load("Tech Lights");
        sc.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        sc.Play();
        if (areaLights.activeSelf)
        {
            upperLightText.GetComponent<Text>().text = "Off";
            areaLights.SetActive(false);
        }
        else
        {
            upperLightText.GetComponent<Text>().text = "On";
            areaLights.SetActive(true);
        }
    }

    public void SwapWindow(int input)
    {
        WindowCamera.SetActive(false);
        WindowShow.SetActive(false);
        WindowSound.SetActive(false);
        WindowCustomProperties.SetActive(false);
        switch (input)
        {
            case 0:
                WindowMenu.SetActive(true);
                break;
            case 1:
                WindowSound.SetActive(true);
                break;
            case 2:
                WindowShow.SetActive(true);
                break;
            case 3:
                WindowCamera.SetActive(true);
                break;
            case 5:
                WindowFlows.SetActive(true);
                break;
            case 6:
                WindowCustomProperties.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DynamicSwitch(int value)
    {
        if (value == 1)
        {
            bonesText.text = "On";
            allDynamics = FindObjectsOfType(typeof(DynamicBone)) as DynamicBone[];
            for (int i = 0; i < allDynamics.Length; i++)
            {
                allDynamics[i].enabled = true;
            }
            dynamicBonesEnabled = true;
        }
        else
        {
            bonesText.text = "Off";
            allDynamics = FindObjectsOfType(typeof(DynamicBone)) as DynamicBone[];
            for (int i = 0; i < allDynamics.Length; i++)
            {
                allDynamics[i].enabled = false;
            }
            dynamicBonesEnabled = false;
        }
    }

    public void SignalSwap(int input)
    {

        if (showPanelUI.signalChange == UI_PlayRecord.SignalChange.normal)
        {
            signalSwapText.text = "On";
            switch (input)
            {
                case 0:
                    showPanelUI.signalChange = UI_PlayRecord.SignalChange.PreCU;
                    break;
                case 1:
                    showPanelUI.signalChange = UI_PlayRecord.SignalChange.PrePTT;
                    break;
                default:
                    break;
            }

        }
        else
        {
            signalSwapText.text = "Off";
            showPanelUI.signalChange = UI_PlayRecord.SignalChange.normal;
        }
    }

    public void VHSToggle(int input)
    {
        currentCamProfile = Mathf.Max(Mathf.Min(currentCamProfile + input, camProfiles.Length - 1), 0);
        camFilterText.text = camProfiles[currentCamProfile].name;
        Camera.main.AddComponent<Volume>().profile = camProfiles[currentCamProfile];
    }

    public void SetSmoothCam(int input)
    {
        if (showPanelUI.thePlayer.GetComponent<Player>().enableCamSmooth == true)
        {
            camSmoothText.text = "Off";
            showPanelUI.thePlayer.GetComponent<Player>().enableCamSmooth = false;
        }
        else
        {
            camSmoothText.text = "On";
            showPanelUI.thePlayer.GetComponent<Player>().enableCamSmooth = true;
        }
    }

    public void AutoCurtains(int input)
    {
        for (int i = 0; i < showPanelUI.stages.Length; i++)
        {
            if (showPanelUI.stages[i].curtainValves != null)
            {
                if (showPanelUI.stages[i].curtainValves.curtainOverride == true)
                {
                    curtainText.text = "Off";
                    showPanelUI.stages[i].curtainValves.curtainOverride = false;
                }
                else
                {
                    curtainText.text = "On";
                    showPanelUI.stages[i].curtainValves.curtainOverride = true;
                }
            }
        }
    }

    public void PSIChange(int input)
    {
        showPanelUI.mackValves.GetComponent<Mack_Valves>().PSI = Mathf.Max(5, showPanelUI.mackValves.GetComponent<Mack_Valves>().PSI + input);
        psiText.text = showPanelUI.mackValves.GetComponent<Mack_Valves>().PSI * 2 + " PSI";
    }

    public void MusicVolumeChange(int input)
    {
        for (int i = 0; i < avManagement.rightSpeakers.Count; i++)
        {
            avManagement.rightSpeakers[i].GetComponent<AudioSource>().volume += input * .05f;
        }
        for (int i = 0; i < avManagement.leftSpeakers.Count; i++)
        {
            avManagement.leftSpeakers[i].GetComponent<AudioSource>().volume += input * .05f;
        }
        volumeText.GetComponent<Text>().text = Mathf.Ceil(avManagement.rightSpeakers[0].GetComponent<AudioSource>().volume * 100).ToString();
    }

    public void IntermissionMusicChange(int input)
    {
        this.GetComponent<AudioSource>().Stop();

        currentMusicTrack = Mathf.Max(Mathf.Min(currentMusicTrack + input, intermissionAudios.Length - 1), 0);

        if (currentMusicTrack == 0)
        {
            intermissionMusicText.text = "None";
        }
        else
        {
            intermissionMusicText.text = intermissionAudios[currentMusicTrack].name;
            foreach (AudioSource audioSource in avManagement.leftSpeakers)
            {
                if (showPanelUI.IsShowPlaying == false)
                {
                    if (audioSource != null)
                    {
                        audioSource.clip = intermissionAudios[currentMusicTrack];
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                }
            }

            foreach (AudioSource audioSource in avManagement.rightSpeakers)
            {
                if (showPanelUI.IsShowPlaying == false)
                {
                    if (audioSource != null)
                    {
                        audioSource.clip = intermissionAudios[currentMusicTrack];
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                }
            }
        }
    }

    public void SoundVolumeChange(int input)
    {
        AudioSource gg = GameObject.Find("GlobalAudio").GetComponent<AudioSource>();
        AudioSource ga = GameObject.Find("GlobalAmbience").GetComponent<AudioSource>();
        gg.volume += input * .05f;
        ga.volume += input * .05f;
        soundvolumeText.GetComponent<Text>().text = Mathf.Ceil(gg.volume * 100).ToString();
    }

    public void SpacialToggle(int input)
    {
        currentSpacial += input;
        if (currentSpacial == 0)
        {
            DisableValveSounds();
            for (int i = 0; i < avManagement.rightSpeakers.Count; i++)
            {
                avManagement.rightSpeakers[i].GetComponent<AudioSource>().spatialBlend = 0;
            }
            for (int i = 0; i < avManagement.leftSpeakers.Count; i++)
            {
                avManagement.leftSpeakers[i].GetComponent<AudioSource>().spatialBlend = 0;
            }
            spacialText.GetComponent<Text>().text = "Off";
        }
        if (currentSpacial == 1)
        {
            DisableValveSounds();
            for (int i = 0; i < avManagement.rightSpeakers.Count; i++)
            {
                avManagement.rightSpeakers[i].GetComponent<AudioSource>().spatialBlend = 1;
            }
            for (int i = 0; i < avManagement.leftSpeakers.Count; i++)
            {
                avManagement.leftSpeakers[i].GetComponent<AudioSource>().spatialBlend = 1;
            }
            spacialText.GetComponent<Text>().text = "Basic";
        }
        if (currentSpacial == 2)
        {
            DisableValveSounds();
            for (int i = 0; i < avManagement.rightSpeakers.Count; i++)
            {
                avManagement.rightSpeakers[i].GetComponent<AudioSource>().spatialBlend = 1;
            }
            for (int i = 0; i < avManagement.leftSpeakers.Count; i++)
            {
                avManagement.leftSpeakers[i].GetComponent<AudioSource>().spatialBlend = 1;
            }
            spacialText.GetComponent<Text>().text = "Full";
        }
        if (currentSpacial == 3)
        {
            EnableValveSounds();
            for (int i = 0; i < avManagement.rightSpeakers.Count; i++)
            {
                avManagement.rightSpeakers[i].GetComponent<AudioSource>().spatialBlend = 1;
            }
            for (int i = 0; i < avManagement.leftSpeakers.Count; i++)
            {
                avManagement.leftSpeakers[i].GetComponent<AudioSource>().spatialBlend = 1;
            }
            spacialText.GetComponent<Text>().text = "Full + Valves";
        }
        if (currentSpacial > 3)
        {
            currentSpacial = 0; // Reset it to 0 if it goes beyond 3.
        }

    }

    // Function to disable ValveSounds scripts in the Characters GameObject
    void DisableValveSounds()
    {
        GameObject characters = GameObject.Find("Characters");
        AudioSource[] valveSounds = characters.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource valve in valveSounds)
        {
            valve.mute = true;
        }
    }

    // Function to enable ValveSounds scripts in the Characters GameObject
    void EnableValveSounds()
    {
        GameObject characters = GameObject.Find("Characters");
        AudioSource[] valveSounds = characters.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource valve in valveSounds)
        {
            valve.mute = false;
        }
    }


    public void FlowProfileUpDown(int input)
    {
        flowNumber = 0;
        flowProfile += input;
        if (flowProfile < 0)
        {
            flowProfile = 0;
        }
        if (flowProfile > showPanelUI.characters.Length - 1)
        {
            flowProfile = showPanelUI.characters.Length - 1;
        }
        FlowUpdate();
    }
    public void FlowNumberUpDown(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        flowNumber += input;
        if (flowNumber < 0)
        {
            flowNumber = 0;
        }
        if (flowNumber > theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().cylBit.Count - 1)
        {
            flowNumber = theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().cylBit.Count - 1;
        }
        FlowUpdate();
    }

    public void FlowCopy()
    {
        copyPasteValues[0] = float.Parse(flowSpeedInText.text);
        copyPasteValues[1] = float.Parse(flowSpeedOutText.text);
        copyPasteValues[2] = float.Parse(flowWeightInText.text);
        copyPasteValues[3] = float.Parse(flowWeightOutText.text);
        copyPasteValues[4] = float.Parse(flowSlamInText.text);
        copyPasteValues[5] = float.Parse(flowSlamOutText.text);
        copyPasteValues[6] = float.Parse(flowSlamSpeedInText.text);
        copyPasteValues[7] = float.Parse(flowSlamSpeedOutText.text);
    }

    public void FlowPaste()
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().flowControlIn[flowNumber] = copyPasteValues[0];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().flowControlOut[flowNumber] = copyPasteValues[1];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().gravityScale[flowNumber] = copyPasteValues[2];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().gravityScaleOut[flowNumber] = copyPasteValues[3];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashControlIn[flowNumber] = copyPasteValues[4];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashControlOut[flowNumber] = copyPasteValues[5];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashSpeedIn[flowNumber] = copyPasteValues[6];
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashSpeedOut[flowNumber] = copyPasteValues[7];
        FlowUpdate();
    }

    public void FlowUpdate()
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
    }

    public void FlowUpdater(GameObject thetest)
    {
        string[] paths = new string[1];

        paths[0] = Application.dataPath + "/StreamingAssets/Flows/Default." + FileExtention;

        if (paths.Length > 0)
        {
            if (paths[0] != "")
            {
                flowFormat thefile = flowFormat.ReadFromFile(paths[0]);
                for (int i = 0; i < thefile.characters.Length; i++)
                {
                    GameObject theCharacter = null;
                    if (thefile.characters[i] != null)
                    {
                        switch (thefile.characters[i].name)
                        {
                            case "Billy Bob":
                                thefile.characters[i].name = "Unknown Mech";
                                break;
                            case "Looney Bird":
                                thefile.characters[i].name = "Pizza Cam";
                                break;
                            case "Rolfe & Earl":
                                thefile.characters[i].name = "Chuck E. Cheese";
                                break;
                            case "Mitzi":
                                thefile.characters[i].name = "Helen Henny";
                                break;
                            case "Sun":
                                thefile.characters[i].name = "Building";
                                break;
                            case "Klunk":
                                thefile.characters[i].name = "Uncle Pappy";
                                break;
                            case "Beach Bear":
                                thefile.characters[i].name = "Jasper T. Jowls";
                                break;
                            case "Fatz":
                                thefile.characters[i].name = "Mr. Munch";
                                break;
                            case "Dook":
                                thefile.characters[i].name = "Pasqually";
                                break;
                            default:
                                break;
                        }

                        //HORRIBLE oversight for flows, gotta redo this
                        string finalname = thetest.transform.parent.name;
                        finalname = finalname.Replace("(Clone)", "").Trim();
                        switch (finalname)
                        {
                            case "Building":
                                finalname = "Building";
                                break;
                            case "Moon":
                                finalname = "Moon";
                                break;
                            case "Charlie Cheese":
                                finalname = "Chuck E. Cheese";
                                break;
                            case "Cheerleader Chicken":
                                finalname = "Helen Henny";
                                break;
                            case "Jaas B. Bowls":
                                finalname = "Jasper T. Jowls";
                                break;
                            case "Mr. Crunch":
                                finalname = "Mr. Munch";
                                break;
                            case "The Italian":
                                finalname = "Pasqually";
                                break;
                            case "Camera":
                                finalname = "Pizza Cam";
                                break;
                            case "Unused Mech":
                                finalname = "Unused Mech";
                                break;
                            case "Wink":
                                finalname = "Wink";
                                break;
                            case "Cyberamic Chuck":
                                finalname = "Chuck E Cheese";
                                break;
                            case "Cyberamic Flag Wavers":
                                finalname = "Flags";
                                break;
                            case "Cyberamic Guest":
                                finalname = "Guest";
                                break;
                            case "Cyberamic Jasper":
                                finalname = "Jasper T Jowls";
                                break;
                            case "Cyberamic Munch":
                                finalname = "Munch";
                                break;
                            case "Cyberamic Pasqually":
                                finalname = "Pasqually";
                                break;
                            case "Cyberamic Warblettes":
                                finalname = "Warblettes";
                                break;
                            case "Studio C Chuck":
                                finalname = "Chuck E Cheese";
                                break;
                            case "Uncle Pappy":
                                finalname = "Uncle Pappy";
                                break;
                            case "Kooser Chuck":
                                finalname = "Chuck E. Cheese";
                                break;
                            case "Swift - Tree":
                                finalname = "Building";
                                break;
                            case "Swift - Swift":
                                finalname = "Chuck E. Cheese";
                                break;
                            case "Swift - Merlin":
                                finalname = "Helen Henny";
                                break;
                            case "Swift - Redwood":
                                finalname = "Jasper T. Jowls";
                                break;
                            case "Swift - Bruno":
                                finalname = "Mr. Munch";
                                break;
                            case "Swift - Warwick":
                                finalname = "Pasqually";
                                break;
                            case "Swift - Forestcam":
                                finalname = "Pizza Cam";
                                break;
                            default:

                                break;
                        }


                        if (finalname == thefile.characters[i].name)
                        {
                            theCharacter = thetest;
                        }
                    }
                    if (theCharacter != null)
                    {
                        int extra = thefile.characters[i].flowsIn.Length / 2;
                        Animatronic_Manager cv = theCharacter.transform.GetComponent<Animatronic_Manager>();
                        if (extra < cv.flowControlIn.Length)
                        {
                            //Old File Format
                            for (int e = 0; e < cv.flowControlIn.Length; e++)
                            {
                                cv.flowControlIn[e] = thefile.characters[i].flowsIn[e] / 1000f;
                                cv.flowControlOut[e] = thefile.characters[i].flowsOut[e] / 1000f;
                                cv.gravityScale[e] = thefile.characters[i].weightIn[e] / 1000f;
                                cv.gravityScaleOut[e] = thefile.characters[i].weightOut[e] / 1000f;
                            }
                        }
                        else
                        {
                            //New File Format
                            for (int e = 0; e < extra; e++)
                            {
                                cv.flowControlIn[e] = thefile.characters[i].flowsIn[e] / 1000f;
                                cv.flowControlOut[e] = thefile.characters[i].flowsOut[e] / 1000f;
                                cv.gravityScale[e] = thefile.characters[i].weightIn[e] / 1000f;
                                cv.gravityScaleOut[e] = thefile.characters[i].weightOut[e] / 1000f;
                                cv.smashControlIn[e] = thefile.characters[i].flowsIn[e + extra] / 1000f;
                                cv.smashControlOut[e] = thefile.characters[i].flowsOut[e + extra] / 1000f;
                                cv.smashSpeedIn[e] = thefile.characters[i].weightIn[e + extra] / 1000f;
                                cv.smashSpeedOut[e] = thefile.characters[i].weightOut[e + extra] / 1000f;
                            }
                        }
                    }
                }
            }
        }
    }

    public String SearchBitChartName(int bit, bool drawer)
    {
        if (drawer)
        {
            bit += 150;
        }
        UI_WindowMaker windowMaker = showPanelUI.GetComponent<UI_WindowMaker>();
        for (int i = 0; i < windowMaker.recordingGroups.Length; i++)
        {
            for (int e = 0; e < windowMaker.recordingGroups[i].inputNames.Length; e++)
            {
                int finalBitNum = 0;
                if (windowMaker.recordingGroups[i].inputNames[e].drawer)
                {
                    finalBitNum += 150;
                }
                if (windowMaker.recordingGroups[i].inputNames[e].index + finalBitNum == bit)
                {
                    return windowMaker.recordingGroups[i].inputNames[e].name;
                }
            }

        }
        return "Nothing";
    }

    public void FlowInSpeedUpDown(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().flowControlIn[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().flowControlIn[flowNumber] * 100) + input) / 100.00f);
        FlowUpdate();
    }

    public void FlowOutSpeedUpDown(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().flowControlOut[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().flowControlOut[flowNumber] * 100) + input) / 100.00f);
        FlowUpdate();
    }
    public void FlowInWeightUpDown(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().gravityScale[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().gravityScale[flowNumber] * 100) + input) / 100.00f);
        FlowUpdate();
    }

    public void FlowOutWeightUpDown(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().gravityScaleOut[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().gravityScaleOut[flowNumber] * 100) + input) / 100.00f); ;
        FlowUpdate();
    }

    public void SmashControlIn(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashControlIn[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashControlIn[flowNumber] * 100) + input) / 100.00f); ;
        FlowUpdate();
    }
    public void SmashControlOut(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashControlOut[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashControlOut[flowNumber] * 100) + input) / 100.00f); ;
        FlowUpdate();
    }
    public void SmashSpeedIn(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashSpeedIn[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashSpeedIn[flowNumber] * 100) + input) / 100.00f); ;
        FlowUpdate();
    }
    public void SmashSpeedOut(int input)
    {
        GameObject theCharacter = null;
        foreach (Transform child in showPanelUI.characterHolder.transform)
        {
            if (child.name == showPanelUI.characters[flowProfile].characterName)
            {
                theCharacter = child.gameObject;
            }
        }
        if (theCharacter == null)
        {
            return;
        }
        theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashSpeedOut[flowNumber] = Mathf.Max(0, (Mathf.Round(theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>().smashSpeedOut[flowNumber] * 100) + input) / 100.00f); ;
        FlowUpdate();
    }

    public void FlowLoad(int input)
    {
        StartCoroutine(FlowLoadRoutine(input));
    }

    IEnumerator FlowLoadRoutine(int input)
    {
        string[] paths = new string[1];

        paths[0] = Application.dataPath + "/StreamingAssets/Flows/Default." + FileExtention;

        if (paths.Length > 0)
        {
            if (paths[0] != "")
            {
                flowFormat thefile = flowFormat.ReadFromFile(paths[0]);
                for (int i = 0; i < thefile.characters.Length; i++)
                {
                    GameObject theCharacter = null;
                    foreach (Transform child in showPanelUI.characterHolder.transform)
                    {
                        if (thefile.characters[i] != null)
                        {
                            switch (thefile.characters[i].name)
                            {
                                case "Billy Bob":
                                    thefile.characters[i].name = "Unknown Mech";
                                    break;
                                case "Looney Bird":
                                    thefile.characters[i].name = "Pizza Cam";
                                    break;
                                case "Rolfe & Earl":
                                    thefile.characters[i].name = "Chuck E. Cheese";
                                    break;
                                case "Mitzi":
                                    thefile.characters[i].name = "Helen Henny";
                                    break;
                                case "Sun":
                                    thefile.characters[i].name = "Building";
                                    break;
                                case "Klunk":
                                    thefile.characters[i].name = "Uncle Pappy";
                                    break;
                                case "Beach Bear":
                                    thefile.characters[i].name = "Jasper T. Jowls";
                                    break;
                                case "Fatz":
                                    thefile.characters[i].name = "Mr. Munch";
                                    break;
                                case "Dook":
                                    thefile.characters[i].name = "Pasqually";
                                    break;
                                default:
                                    break;
                            }
                            if (child.name == thefile.characters[i].name)
                            {
                                theCharacter = child.gameObject;
                            }
                        }
                        else
                        {
                            try
                            {
                                int extra = thefile.characters[i].flowsIn.Length / 2;

                                Animatronic_Manager cv = theCharacter.transform.GetChild(0).GetComponent<Animatronic_Manager>();
                                if (extra < cv.flowControlIn.Length)
                                {
                                    //Old File Format
                                    for (int e = 0; e < cv.flowControlIn.Length; e++)
                                    {
                                        cv.flowControlIn[e] = thefile.characters[i].flowsIn[e] / 1000f;
                                        cv.flowControlOut[e] = thefile.characters[i].flowsOut[e] / 1000f;
                                        cv.gravityScale[e] = thefile.characters[i].weightIn[e] / 1000f;
                                        cv.gravityScaleOut[e] = thefile.characters[i].weightOut[e] / 1000f;
                                    }
                                }
                                else
                                {
                                    //New File Format
                                    for (int e = 0; e < extra; e++)
                                    {
                                        cv.flowControlIn[e] = thefile.characters[i].flowsIn[e] / 1000f;
                                        cv.flowControlOut[e] = thefile.characters[i].flowsOut[e] / 1000f;
                                        cv.gravityScale[e] = thefile.characters[i].weightIn[e] / 1000f;
                                        cv.gravityScaleOut[e] = thefile.characters[i].weightOut[e] / 1000f;
                                        cv.smashControlIn[e] = thefile.characters[i].flowsIn[e + extra] / 1000f;
                                        cv.smashControlOut[e] = thefile.characters[i].flowsOut[e + extra] / 1000f;
                                        cv.smashSpeedIn[e] = thefile.characters[i].weightIn[e + extra] / 1000f;
                                        cv.smashSpeedOut[e] = thefile.characters[i].weightOut[e + extra] / 1000f;
                                    }
                                }
                            }
                            catch
                            {
                                // suppresses error with viewport
                            }
                        }
                    }
                }
            }
            FlowUpdate();
            yield return null;
        }
    }
}
