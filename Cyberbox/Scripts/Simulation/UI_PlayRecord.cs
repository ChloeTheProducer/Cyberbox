using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using SimpleFileBrowser;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_PlayRecord : MonoBehaviour
{


    [HideInInspector]
    public StageManager.StageSelector[] stages;

    [HideInInspector]
    public int currentStage = 0;

    //Characters
    public CharacterSelector[] characters;
    public FloatEvent characterEvent = new FloatEvent();

    //Inspector Objects
    [Header("Inspector Objects")]
    public Sprite[] icons;
    public Vector3 OverwriteCharacterHolderPositioning;
    [HideInInspector] public GameObject characterHolder;
    [HideInInspector] public GameObject mackValves;
    [HideInInspector] public UI_RshwCreator creator;
    [HideInInspector] public GameObject playMultiText;
    public UI_SidePanel sidePanel;
    [HideInInspector] public VideoPlayer videoplayer;
    [Space(20)]

    //Show Data
    [Header("Show Data")]
    Mack_Valves mack;
    InputHandler inputHandlercomp;
    [HideInInspector] public GameObject thePlayer;

    // Show Information
    [HideInInspector] public bool IsShowPlaying;

    public SignalChange signalChange;
    public enum SignalChange
    {
        normal,
        PreCU,
        PrePTT,
    }

    public UI_ShowtapeManager manager;
    SC_AV_Management avManagement;

    void Start()
    {
        characterHolder = new GameObject("Characters");
        characterHolder.transform.parent = GameObject.Find("Show Selector").transform.parent;

        //Initialize Objects
        stages = GameObject.Find("Stages").GetComponent<StageManager>().stages;
        avManagement = GetComponent<SC_AV_Management>();
        thePlayer = GameObject.Find("Player");
        inputHandlercomp = mackValves.GetComponent<InputHandler>();
        mack = mackValves.GetComponent<Mack_Valves>();
        videoplayer = this.GetComponent<VideoPlayer>();
        creator = this.GetComponent<UI_RshwCreator>();
        //Start up stages
        for (int i = 0; i < stages.Length; i++)
        {
            // this is so my dumb brain doesn't have to keep reenabling the curtains when i forget to
            stages[i].Startup();
            if (stages[i].curtain != null)
            {
                stages[i].curtain.SetActive(true);
            }

        }

        GetComponent<UI_WindowMaker>().MakeThreeWindow(icons[0], icons[1], icons[2], 0, 8, 2, 3, "Customize", "Play", "Create");

        //Spawn in current Characters
        RecreateAllCharacters("");
        DisableAllCameras(); // Disable all cameras first
        playMultiText = this.GetComponent<UI_WindowMaker>().PlayWindow.gameObject;

        if (OverwriteCharacterHolderPositioning != new Vector3(0, 0, 0))
        {
            characterHolder.transform.SetLocalPositionAndRotation((OverwriteCharacterHolderPositioning), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            characterHolder.transform.SetLocalPositionAndRotation(new Vector3((float)-0.4, (float)-1.792298, (float)-3.5), Quaternion.Euler(0, 0, 0));
        }



        Debug.Log("Startup Events are completed. Show is ready!");
    }

    void Update()
    {
        //Run the Simulation
        UpdateAnims();
    }

    void UpdateAnims()
    {

        //Update Animatronics
        characterEvent.Invoke(Time.deltaTime * 60);

        //Update Lights
        for (int i = 0; i < stages[currentStage].lightValves.Length; i++)
        {
            stages[currentStage].lightValves[i].CreateMovements(Time.deltaTime * 60);
        }


        if (IsShowPlaying)
        {
            //A special case for swapping signals around in realtime through the Live Editor
            switch (signalChange)
            {
                case SignalChange.PreCU:
                    bool g = mack.topDrawer[85];
                    mack.topDrawer[85] = mack.topDrawer[80];
                    mack.topDrawer[80] = false;
                    mack.topDrawer[83] = g;
                    mack.topDrawer[92] = mack.topDrawer[90];
                    mack.topDrawer[93] = mack.topDrawer[91];
                    mack.bottomDrawer[79] = mack.bottomDrawer[74];
                    mack.bottomDrawer[90] = mack.bottomDrawer[74];
                    mack.bottomDrawer[89] = false;
                    mack.bottomDrawer[63] = true;
                    mack.topDrawer[25] = !mack.topDrawer[25];
                    mack.topDrawer[26] = !mack.topDrawer[26];
                    break;
                default:
                    break;
            }

            //Update TV turn offs
            if (avManagement.videoPath != "")
            {
                for (int i = 0; i < stages[currentStage].tvs.Length; i++)
                {
                    bool bitOff = false;
                    bool bitOn = false;
                    if (stages[currentStage].tvs[i].drawer)
                    {
                        if (mack.bottomDrawer[stages[currentStage].tvs[i].bitOff])
                        {
                            bitOff = true;
                        }
                        if (mack.bottomDrawer[stages[currentStage].tvs[i].bitOn])
                        {
                            bitOn = true;
                        }
                    }
                    else
                    {
                        if (mack.topDrawer[stages[currentStage].tvs[i].bitOff])
                        {
                            bitOff = true;
                        }
                        if (mack.topDrawer[stages[currentStage].tvs[i].bitOn])
                        {
                            bitOn = true;
                        }
                    }



                    //Curtain check
                    if (stages[currentStage].tvs[i].onWhenCurtain)
                    {
                        //0 = Off First Frame
                        //1 = Off
                        //2 = On First Frame
                        //3 = On


                        //If Force Curtain
                        if (stages[currentStage].curtainValves.curtainOverride && stages[currentStage].tvs[i].curtainSubState == 1)
                        {
                            stages[currentStage].tvs[i].curtainSubState = 2;
                        }
                        if (!stages[currentStage].curtainValves.curtainOverride && stages[currentStage].tvs[i].curtainSubState == 3)
                        {
                            stages[currentStage].tvs[i].curtainSubState = 0;
                        }



                        //Apply
                        if (stages[currentStage].tvs[i].curtainSubState == 0)
                        {
                            for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                            {
                                stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.black);
                            }
                            stages[currentStage].tvs[i].curtainSubState = 1;
                        }
                        else if (stages[currentStage].tvs[i].curtainSubState == 2)
                        {
                            for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                            {
                                stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.white);

                            }
                            stages[currentStage].tvs[i].curtainSubState = 3;
                        }

                    }
                    switch (stages[currentStage].tvs[i].tvSettings)
                    {
                        case StageManager.ShowTV.tvSetting.onOnly:
                            if (!bitOn)
                            {
                                for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                                {
                                    stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.black);
                                }
                            }
                            else
                            {
                                for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                                {
                                    stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.white);
                                }
                            }
                            break;
                        case StageManager.ShowTV.tvSetting.offOn:
                            if (stages[currentStage].tvs[i].onWhenCurtain && manager.referenceSpeaker.pitch < 0)
                            {
                                bool temp;
                                temp = bitOff;
                                bitOff = bitOn;
                                bitOn = temp;
                            }
                            if (bitOff)
                            {
                                for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                                {
                                    stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.black);
                                }
                            }
                            if (bitOn)
                            {
                                for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                                {
                                    stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.white);
                                }
                            }
                            break;
                        case StageManager.ShowTV.tvSetting.none:
                            for (int e = 0; e < stages[currentStage].tvs[i].tvs.Length; e++)
                            {
                                stages[currentStage].tvs[i].tvs[e].material.SetColor("_BaseColor", Color.white);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Update Curtains
        if (stages[currentStage].curtainValves != null)
        {
            if (manager.referenceSpeaker.pitch < 0)
            {
                stages[currentStage].curtainValves.CreateMovements(Time.deltaTime * 60, true);
            }
            else
            {
                stages[currentStage].curtainValves.CreateMovements(Time.deltaTime * 60, false);
            }

        }
    }

    /// <summary>
    /// Switches which window is displayed on the main UI panel.
    /// </summary>
    /// <param name="thewindow"></param>
    public void SwitchWindow(int thewindow)
    {
        if (SceneManager.GetActiveScene().name != "Arduino")
        {
            switch (thewindow)
            {
                case 0:
                    //Title
                    creator.EraseShowtape();
                    break;
                case 1:
                    //Main Screen
                    {
                        this.GetComponent<UI_WindowMaker>().MakeThreeWindow(icons[0], icons[1], icons[2], 0, 8, 2, 3, "Customize", "Play", "Create");
                    }
                    WindowSwitchDisable(true);
                    creator.EraseShowtape();
                    break;
                case 2:
                    this.GetComponent<UI_ShowtapeManager>().Load();
                    creator.EraseShowtape();
                    WindowSwitchDisable(true);
                    break;
                case 3:
                    //Record Screen
                    this.GetComponent<UI_WindowMaker>().MakeTwoWindow(icons[2], icons[3], 1, 5, 4, "Create", "Manage");
                    WindowSwitchDisable(true);
                    creator.EraseShowtape();
                    break;
                case 4:
                    //Edit Recording Screen
                    this.GetComponent<UI_WindowMaker>().MakeTwoWindow(icons[3], icons[5], 3, 34, 21, "Modify Bits", "Add Bits");
                    WindowSwitchDisable(true);
                    creator.EraseShowtape();
                    break;
                case 5:
                    //New Recording Screen
                    this.GetComponent<UI_WindowMaker>().MakeNewRecordWindow();
                    creator.EraseShowtape();
                    break;
                case 6:
                    //Player Menu (Single)
                    manager.Load();
                    this.GetComponent<UI_WindowMaker>().MakePlayWindow(false);
                    break;
                case 8:
                    //Customize Screen
                    this.GetComponent<UI_WindowMaker>().MakeTwoWindow(icons[8], icons[9], 1, 16, 9, "Stages", "Characters");
                    break;
                case 9:
                    //Edit Character Screen
                    DisableAllCameras(); // Disable all cameras first
                    this.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeIconsWindow(characters);
                    break;
                case 11:
                    //Recording Groups Screen (Or New Recording Screen)
                    this.GetComponent<UI_WindowMaker>().MakeRecordIconsWindow();
                    WindowSwitchDisable(false);
                    creator.EraseShowtape();
                    break;
                case 16:
                    //Stage Customize Menu
                    StageCustomMenu();
                    break;
                case 17:
                    this.GetComponent<UI_WindowMaker>().MakePlayWindow(false);
                    break;
                case 21:
                    //Recording Groups Screen (Standalone)
                    this.GetComponent<UI_WindowMaker>().MakeRecordIconsWindow();
                    WindowSwitchDisable(false);
                    creator.EraseShowtape();
                    break;
                case 22:
                    //Delete Movement Screen 1
                    this.GetComponent<UI_WindowMaker>().MakeDeleteMoveMenu(0);
                    break;
                case 23:
                    //Delete Movement Back 1
                    this.GetComponent<UI_WindowMaker>().MakeDeleteMoveMenu(-1);
                    break;
                case 24:
                    //Delete Movement Forward 1
                    this.GetComponent<UI_WindowMaker>().MakeDeleteMoveMenu(1);
                    break;
                case 28:
                    // Showtape Successful
                    this.GetComponent<UI_WindowMaker>().MakeShowtapeSuccessfulWindow();
                    break;
                case 33:
                    // Loading Window
                    this.GetComponent<UI_WindowMaker>().MakeLoadingWindow();
                    break;
                case 34:
                    this.GetComponent<UI_WindowMaker>().MakeTwoWindow(icons[6], icons[5], 4, 22, 35, "Delete Bits", "Replace Audio");
                    break;
                case 35:
                    creator.ReplaceShowAudio();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Starts new show.
    /// </summary>
    /// <param name="input"></param>
    public void StartNewShow(int input)
    {
        Debug.Log("Starting New Show");
        manager.Load();
        if (manager.speakerClip != null)
        {
            SwitchWindow(input);
        }
        IsShowPlaying = true;
    }

    /// <summary>
    /// Load a customize window for a particular character.
    /// </summary>
    /// <param name="input"></param>
    public void CharacterCustomMenu(int input)
    {
        this.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeWindow(characters, input);
        EnablePreviewCamera(input);
    }

    /// <summary>
    /// Load a customize stage window for a particular stage.
    /// </summary>
    public void StageCustomMenu()
    {
        this.GetComponent<UI_WindowMaker>().MakeStageCustomizeWindow();
    }

    /// <summary>
    /// Index up the current costume of a character. Costume 0 is no character on stage.
    /// </summary>
    /// <param name="input"></param>
    public void CostumeUp(int input)
    {
        if (characters[input].currentCostume > -1)
        {
            characters[input].currentCostume--;
            RecreateAllCharacters(characters[input].characterName);
            EnablePreviewCamera(input); // Enable the corresponding camera
            this.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeWindow(characters, input);
        }
    }

    /// <summary>
    /// Index down the current costume of a character.
    /// </summary>
    /// <param name="input"></param>
    public void CostumeDown(int input)
    {
        if (characters[input].currentCostume < characters[input].allCostumes.Length - 1)
        {
            characters[input].currentCostume++;
            RecreateAllCharacters(characters[input].characterName);
            EnablePreviewCamera(input); // Enable the corresponding camera
            this.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeWindow(characters, input);
        }
    }

    // Add a new method to enable the "PreviewCamera" for a character
    private void DisableAllCameras()
    {
        Transform[] childTransforms = characterHolder.GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childTransforms)
        {
            Camera childCamera = childTransform.GetComponentInChildren<Camera>();
            if (childCamera != null)
            {
                childCamera.enabled = false;
            }
        }
    }
    private void EnablePreviewCamera(int characterIndex)
    {
        string characterName = characters[characterIndex].characterName;

        Transform character = characterHolder.transform.Find(characterName);
        character.GetComponentInChildren<Camera>().enabled = true;

    }
    /// <summary>
    /// Index up the current stage presented.
    /// </summary>
    /// <param name="input"></param>
    public void ChangeStage(int input)
    {
        currentStage = input;
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].stage.SetActive(false);
        }
        stages[input].stage.SetActive(true);

        RecreateAllCharacters("");
        DisableAllCameras(); // Disable all cameras first
    }

    public void StageUp()
    {
        int nextStage = (currentStage + 1) % stages.Length;
        if (nextStage != currentStage)
        {
            ChangeStage(nextStage);
        }
    }

    public void StageDown()
    {
        int nextStage = (currentStage - 1 + stages.Length) % stages.Length;
        if (nextStage != currentStage)
        {
            ChangeStage(nextStage);
        }
    }



    public void RecreateAllCharacters(string singleCharacter)
    {
        if (singleCharacter == "")
        {
            //Destroy current Characters
            foreach (Transform child in characterHolder.transform)
            {
                Destroy(child.gameObject);
            }

            //Create current Characters
            int g = 0;
            for (int i = 0; i < stages[currentStage].stageCharacters.Length; i++)
            {
                for (int e = 0; e < characters.Length; e++)
                {
                    if (stages[currentStage].stageCharacters[i].characterName == characters[e].characterName)
                    {
                        try
                        {
                            if (characters[e].currentCostume != -1)
                            {
                                GameObject newChar = Instantiate(characters[e].mainCharacter);
                                newChar.name = characters[e].characterName;
                                newChar.transform.parent = characterHolder.transform;
                                newChar.transform.SetLocalPositionAndRotation(stages[currentStage].stageCharacters[i].characterPos + characters[e].allCostumes[characters[e].currentCostume].offsetPos, Quaternion.Euler(stages[currentStage].stageCharacters[i].characterRot));
                                newChar.transform.GetChild(0).GetComponent<Animatronic_Manager>().mackValves = mackValves;
                                newChar.transform.GetChild(0).GetComponent<Animatronic_Manager>().StartUp();
                                g++;

                                //Delete other costumes
                                foreach (Transform mesh in newChar.transform.GetChild(0).transform)
                                {
                                    if (!(mesh.gameObject.name == characters[e].allCostumes[characters[e].currentCostume].costumeName) && mesh.gameObject.name != "Armature")
                                    {
                                        Destroy(mesh.gameObject);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning("Failed to load Costume for " + characters[e] + ". " + ex.Message);
                        }

                    }
                }
            }
        }
        else
        {
            //Destroy Character
            foreach (Transform child in characterHolder.transform)
            {
                if (child.name == singleCharacter)
                {
                    Destroy(child.gameObject);
                }
            }

            //Create Character
            for (int e = 0; e < characters.Length; e++)
            {
                if (singleCharacter == characters[e].characterName)
                {
                    //Check for multiple of single character
                    bool[] count = new bool[stages[currentStage].stageCharacters.Length];
                    for (int i = 0; i < stages[currentStage].stageCharacters.Length; i++)
                    {
                        if (stages[currentStage].stageCharacters[i].characterName == singleCharacter)
                        {
                            count[i] = true;
                        }
                    }

                    for (int g = 0; g < count.Length; g++)
                    {
                        if (characters[e].currentCostume != -1 && count[g] == true)
                        {
                            GameObject newChar = Instantiate(characters[e].mainCharacter);

                            newChar.name = characters[e].characterName;
                            newChar.transform.parent = characterHolder.transform;
                            newChar.transform.GetChild(0).GetComponent<Animatronic_Manager>().mackValves = mackValves;
                            newChar.transform.localPosition = stages[currentStage].stageCharacters[g].characterPos;
                            newChar.transform.localRotation = Quaternion.Euler(stages[currentStage].stageCharacters[g].characterRot);
                            newChar.transform.GetChild(0).GetComponent<Animatronic_Manager>().StartUp();
                            //Delete other costumes
                            foreach (Transform mesh in newChar.transform.GetChild(0).transform)
                            {
                                if (!(mesh.gameObject.name == characters[e].allCostumes[characters[e].currentCostume].costumeName) && mesh.gameObject.name != "Armature")
                                {
                                    Destroy(mesh.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
        sidePanel.FlowLoad(-1);

        mack.RefreshVariables();
    }


    /// <summary>
    /// Open a window for the current movment group to be used.
    /// </summary>
    /// <param name="input"></param>
    public void RecordingGroupMenu(int input)
    {
        this.GetComponent<UI_WindowMaker>().MakeMoveTestWindow(input);
    }

    /// <summary>
    /// Stops the recording during a window switch.
    /// </summary>
    /// <param name="curtainStop"></param>
    void WindowSwitchDisable(bool curtainStop)
    {
        manager.recordMovements = false;
        inputHandlercomp.valveMapping = 0;
    }

    /// <summary>
    /// Stops the showtape.
    /// </summary>
    public void Stop()
    {
        manager.referenceSpeaker.time = 0;
        manager.referenceVideo.time = 0;
        manager.Play(true, false);
        SwitchWindow(1);
        if (stages[currentStage].curtain != null)
        {
            for (int i = 0; i < stages[currentStage].curtainValves.curtainbools.Length; i++)
            {
                stages[currentStage].curtainValves.curtainbools[i] = false;
            }
        }
        IsShowPlaying = false;
    }

    /// <summary>
    /// Pauses or unpauses the showtape.
    /// </summary>
    public void togglePlayback()
    {
        if ((manager.useVideoAsReference && manager.referenceVideo.isPlaying) || (!manager.useVideoAsReference && manager.referenceSpeaker.isPlaying))
        {
            avManagement.Pause();
        }
        else
        {
            avManagement.Resume();
        }
    }
}
