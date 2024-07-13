using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Player;

public class UI_WindowMaker : MonoBehaviour
{
    GameObject ThreeWindow;
    GameObject TwoWindow;
    public GameObject PlayWindow;
    public GameObject PlayWindowArrow;
    bool PlayWindowOpen;
    public GameObject NewRecordWindow;
    GameObject ShowtapeCreated;
    GameObject RecordIconsWindow;
    GameObject MoveTestWindow;
    GameObject CharacterCustomizeWindow;
    public GameObject StageCustomizeWindow;
    GameObject DeleteOne;
    GameObject LoadingWindow;

    GameObject Canvas;


    Player player;

    [HideInInspector] public int deletePage;

    public MovementRecordings[] recordingGroups;

    public TitleManager titleManager;

    Mack_Valves mack;
    void Awake()
    {
        mack = GameObject.Find("Mack Valves").GetComponent<Mack_Valves>();
        player = GameObject.Find("Player").GetComponent<Player>();
        Canvas = transform.parent.Find("Main/Windows").gameObject;
        ThreeWindow = transform.GetChild(0).gameObject;
        TwoWindow = transform.GetChild(1).gameObject;
        RecordIconsWindow = transform.GetChild(3).gameObject;
        MoveTestWindow = transform.GetChild(4).gameObject;
        CharacterCustomizeWindow = transform.GetChild(5).gameObject;
        DeleteOne = transform.GetChild(7).gameObject;
        LoadingWindow = transform.GetChild(8).gameObject;

        DisableWindows();
    }

    public void MakeThreeWindow(Sprite one, Sprite two, Sprite three, int switchBack, int switchOne, int switchTwo, int switchThree, string butOne, string butTwo, string butThree)
    {
        DisableWindows();
        ThreeWindow.SetActive(true);
        ThreeWindow.transform.Find("Button1/Icon1").GetComponent<Image>().sprite = one;
        ThreeWindow.transform.Find("Button2/Icon2").GetComponent<Image>().sprite = two;
        ThreeWindow.transform.Find("Button3/Icon3").GetComponent<Image>().sprite = three;
        ThreeWindow.transform.Find("Button1").GetComponent<Button3D>().funcWindow = switchOne;
        ThreeWindow.transform.Find("Button2").GetComponent<Button3D>().funcWindow = switchTwo;
        ThreeWindow.transform.Find("Button3").GetComponent<Button3D>().funcWindow = switchThree;
        if (switchBack == 0)
        {
            ThreeWindow.transform.Find("Back Button").gameObject.SetActive(false);
        }
        else
        {
            ThreeWindow.transform.Find("Back Button").gameObject.SetActive(true);
            ThreeWindow.transform.Find("Back Button").GetComponent<Button3D>().funcWindow = switchBack;
        }
        ThreeWindow.transform.Find("Button1").GetChild(0).GetComponent<TMP_Text>().text = butOne;
        ThreeWindow.transform.Find("Button2").GetChild(0).GetComponent<TMP_Text>().text = butTwo;
        ThreeWindow.transform.Find("Button3").GetChild(0).GetComponent<TMP_Text>().text = butThree;
    }

    public void MakeTwoWindow(Sprite one, Sprite two, int switchBack, int switchOne, int switchTwo, string butOne, string butTwo)
    {
        DisableWindows();
        TwoWindow.SetActive(true);
        TwoWindow.transform.Find("Button1/Icon1").GetComponent<Image>().sprite = one;
        TwoWindow.transform.Find("Button2/Icon2").GetComponent<Image>().sprite = two;
        TwoWindow.transform.Find("Button1").GetComponent<Button3D>().funcWindow = switchOne;
        TwoWindow.transform.Find("Button2").GetComponent<Button3D>().funcWindow = switchTwo;
        TwoWindow.transform.Find("Back Button").GetComponent<Button3D>().funcWindow = switchBack;
        TwoWindow.transform.Find("Button1").GetChild(0).GetComponent<TMP_Text>().text = butOne;
        TwoWindow.transform.Find("Button2").GetChild(0).GetComponent<TMP_Text>().text = butTwo;
    }

    public void MakePlayWindow(bool recording)
    {
        DisableWindows();
        mack.Enable();
        player.playerState = PlayerState.frozenAllUnlock;
        TogglePlayWindow();
    }

    public void TogglePlayWindow()
    {
        if (PlayWindowOpen == false) // Open
        {
            PlayWindowOpen = true;
            LeanTween.moveY(PlayWindow.GetComponent<RectTransform>(), 0, 0.5f);
            PlayWindowArrow.GetComponent<RectTransform>().transform.rotation.Set(0, 0, 180, 0);
        }
        else // Close
        {
            PlayWindowOpen = false;
            LeanTween.moveY(PlayWindow.GetComponent<RectTransform>(), -180, 0.5f);
            PlayWindowArrow.GetComponent<RectTransform>().transform.rotation.Set(0, 0, -180, 0);
        }

    }

    public void MakeNewRecordWindow()
    {
        DisableWindows();
        player.playerState = PlayerState.frozenAllUnlock;
        titleManager.SwitchMenuPanel(NewRecordWindow);
    }

    public void MakeLoadingWindow()
    {
        DisableWindows();
        LoadingWindow.SetActive(true);
    }

    public void MakeShowtapeSuccessfulWindow()
    {
        DisableWindows();
        ShowtapeCreated.SetActive(true);
    }

    public void MakeRecordIconsWindow()
    {
        DisableWindows();

        RecordIconsWindow.SetActive(true);

        for (int i = 0; i < 24; i++)
        {
            if (i < recordingGroups.Length)
            {
                GameObject button = RecordIconsWindow.transform.Find("Button (" + i + ")").gameObject;
                button.SetActive(true);
                button.GetComponent<Image>().sprite = recordingGroups[i].groupIcon;
                button.transform.GetChild(0).GetComponent<Button3D>().funcName = "RecordingGroupMenu";
                button.transform.GetChild(0).GetComponent<Button3D>().funcWindow = i;
                button.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = recordingGroups[i].groupName;
            }
            else
            {
                RecordIconsWindow.transform.Find("Button (" + i + ")").gameObject.SetActive(false);
            }
        }
        RecordIconsWindow.transform.Find("Back Button").GetComponent<Button3D>().funcWindow = 4;
    }

    public void MakeCharacterCustomizeIconsWindow(CharacterSelector[] characters)
    {
        DisableWindows();
        RecordIconsWindow.SetActive(true);

        int currentButton = 0;
        for (int i = 0; i < 24; i++)
        {
            if(i < characters.Length && characters[i].allCostumes.Length > 1)
            {
                GameObject button = RecordIconsWindow.transform.Find("Button (" + currentButton + ")").gameObject;
                button.SetActive(true);
                button.transform.GetChild(0).GetComponent<Button3D>().funcName = "CharacterCustomMenu";
                button.transform.GetChild(0).GetComponent<Button3D>().funcWindow = i;
                button.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = characters[i].characterName;
                currentButton++;
            }
        }
        for (int i = currentButton; i < 24; i++)
        {
            RecordIconsWindow.transform.Find("Button (" + i + ")").gameObject.SetActive(false);
        }
        RecordIconsWindow.transform.Find("Back Button").GetComponent<Button3D>().funcWindow = 8;
    }

    public void MakeMoveTestWindow(int currentGroup)
    {
        DisableWindows();
        mack.Enable();
        MoveTestWindow.transform.Find("Ready").GetComponent<InputSetter>().mapping = currentGroup + 1;
        MoveTestWindow.SetActive(true);
        for (int i = 0; i < 33; i++)
        {
            if (i < recordingGroups[currentGroup].inputNames.Length)
            {
                GameObject button = MoveTestWindow.transform.Find("Scroll View/Viewport/Content/"+ "Button " + i).gameObject;
                
                button.SetActive(true);
                button.transform.transform.Find("Text").GetComponent<Text>().text = recordingGroups[currentGroup].inputNames[i].name;
            }
            else
            {
                MoveTestWindow.transform.Find("Scroll View/Viewport/Content/" + "Button " + i).gameObject.SetActive(false);
            }
        }
        MoveTestWindow.transform.Find("Back Button").GetComponent<Button3D>().funcWindow = 21;
       
    }

    public void MakeCharacterCustomizeWindow(CharacterSelector[] characters, int current)
    {
        DisableWindows();
        CharacterCustomizeWindow.SetActive(true);
        if (characters[current].currentCostume == characters[current].allCostumes.Length - 1)
        {
            CharacterCustomizeWindow.transform.Find("Down").gameObject.SetActive(false);
        }
        else
        {
            CharacterCustomizeWindow.transform.Find("Down").gameObject.SetActive(true);
        }
        if (characters[current].currentCostume == -1)
        {
            CharacterCustomizeWindow.transform.Find("Up").gameObject.SetActive(false);
        }
        else
        {
            CharacterCustomizeWindow.transform.Find("Up").gameObject.SetActive(true);
        }
        if (characters[current].currentCostume != -1)
        {
            CharacterCustomizeWindow.transform.Find("Full Costume").gameObject.GetComponent<Text>().text = characters[current].allCostumes.Length.ToString();
            CharacterCustomizeWindow.transform.Find("Current Costume").gameObject.GetComponent<Text>().text = (1 + characters[current].currentCostume).ToString();
            CharacterCustomizeWindow.transform.Find("Name").gameObject.GetComponent<Text>().text = characters[current].allCostumes[characters[current].currentCostume].costumeName;
            CharacterCustomizeWindow.transform.Find("Type").gameObject.GetComponent<Text>().text = characters[current].allCostumes[characters[current].currentCostume].costumeType.ToString();
            CharacterCustomizeWindow.transform.Find("Description").gameObject.GetComponent<Text>().text = characters[current].allCostumes[characters[current].currentCostume].costumeDesc;
            CharacterCustomizeWindow.transform.Find("Year").gameObject.GetComponent<Text>().text = characters[current].allCostumes[characters[current].currentCostume].yearOfCostume;
            CharacterCustomizeWindow.transform.Find("Down").gameObject.GetComponent<Button3D>().funcWindow = current;
            CharacterCustomizeWindow.transform.Find("Up").gameObject.GetComponent<Button3D>().funcWindow = current;
        }
        else
        {
            CharacterCustomizeWindow.transform.Find("Full Costume").gameObject.GetComponent<Text>().text = characters[current].allCostumes.Length.ToString();
            CharacterCustomizeWindow.transform.Find("Current Costume").gameObject.GetComponent<Text>().text = (1 + characters[current].currentCostume).ToString();
            CharacterCustomizeWindow.transform.Find("Name").gameObject.GetComponent<Text>().text = "None";
            CharacterCustomizeWindow.transform.Find("Type").gameObject.GetComponent<Text>().text = "";
            CharacterCustomizeWindow.transform.Find("Description").gameObject.GetComponent<Text>().text = "No character is present.";
            CharacterCustomizeWindow.transform.Find("Year").gameObject.GetComponent<Text>().text = "";
            CharacterCustomizeWindow.transform.Find("Down").gameObject.GetComponent<Button3D>().funcWindow = current;
            CharacterCustomizeWindow.transform.Find("Up").gameObject.GetComponent<Button3D>().funcWindow = current;

        }
    }

    public void MakeStageCustomizeWindow()
    {
        DisableWindows();
        player.playerState = PlayerState.frozenAllUnlock;
        titleManager.SwitchMenuPanel(StageCustomizeWindow);
    }

    public void MakeDeleteMoveMenu(int page)
    {
        DisableWindows();
        DeleteOne.SetActive(true);
    }

    public string SearchBitChartName(int bit)
    {
        for (int i = 0; i < recordingGroups.Length; i++)
        {
            for (int e = 0; e < recordingGroups[i].inputNames.Length; e++)
            {
                int finalBitNum = 0;
                if (recordingGroups[i].inputNames[e].drawer)
                {
                    finalBitNum += 150;
                }
                if (recordingGroups[i].inputNames[e].index + finalBitNum == bit || bit == 0)
                {
                    return bit.ToString() + "- " + recordingGroups[i].groupName + " - " + recordingGroups[i].inputNames[e].name;
                }
            }
        }
        return bit.ToString() + "- Nothing";
    }

    public int SearchBitChartGroupID(int bit)
    {
        for (int i = 0; i < recordingGroups.Length; i++)
        {
            for (int e = 0; e < recordingGroups[i].inputNames.Length; e++)
            {
                int finalBitNum = 0;
                if (recordingGroups[i].inputNames[e].drawer)
                {
                    finalBitNum += 150;
                }
                if (    recordingGroups[i].inputNames[e].index + finalBitNum == bit)
                {
                    return i;
                }
            }

        }
        return 0;
    }

    void DisableWindows()
    {
        mack.Disable();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).gameObject.activeSelf)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }
        player.playerState = PlayerState.normal;
        titleManager.CloseAllPanels();
    }

    [System.Serializable]
    public class MovementRecordings
    {
        public string groupName;
        public Sprite groupIcon;
        public inputNames[] inputNames;
    }
    [System.Serializable]
    public class inputNames
    {
        public string name;
        public bool drawer;
        public int index;
    }
}

[System.Serializable]
public class ShowtapeYear
{
    public ShowTapeSelector[] groups;
}

[System.Serializable]
public class ShowTapeSelector
{
    public string showtapeName;
    public string showtapeDate;
    public string showtapeLength;
    public string ytLink;
}