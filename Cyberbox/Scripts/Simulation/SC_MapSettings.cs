using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SC_MapSettings : MonoBehaviour
{
    // FPS Counter
    private float _expSmoothingFactor = 0.9f;
    private float _refreshFrequency = 0.4f;

    private float _timeSinceUpdate = 0f;
    private float _averageFps = 1f;

    UI_SidePanel uiSidePanel;
    GameObject FPSWarning;

    // Variables
    [Header("Player")]
    public bool fpsBasedOptimisation = true;

    [Header("Show")]
    GameObject CustomShowTemplate;
    public List<CustomShowProperties> customShowProperties;

    // Start is called before the first frame update
    void Start()
    {
        uiSidePanel = GameObject.Find("UI Side Panel").GetComponent<UI_SidePanel>();
        FPSWarning = GameObject.Find("FPSWarning");
        CustomShowTemplate = GetComponentInChildren<UI_SidePanel>().ShowTemplate;
        SetupShowTemplate();
    }


    // Update is called once per frame
    void Update()
    {
        // Exponentially weighted moving average (EWMA)
        _averageFps = _expSmoothingFactor * _averageFps + (1f - _expSmoothingFactor) * 1f / Time.unscaledDeltaTime;

        if (_timeSinceUpdate < _refreshFrequency)
        {
            _timeSinceUpdate += Time.deltaTime;
            return;
        }

        int fps = Mathf.RoundToInt(_averageFps);

        _timeSinceUpdate = 0f;

        if (fpsBasedOptimisation)
        {
            if (fps < 25)
            {
                if (uiSidePanel.dynamicBonesEnabled == true)
                {
                    uiSidePanel.DynamicSwitch(0);
                    uiSidePanel.dynamicBonesEnabled = false;
                    FPSWarning.SetActive(true);
                    Debug.Log("Dynamic Bones Disabled based on FPS");
                }
            }
            else
            {
                if (uiSidePanel.dynamicBonesEnabled == false)
                {
                    uiSidePanel.DynamicSwitch(1);
                    uiSidePanel.dynamicBonesEnabled = true;
                    FPSWarning.SetActive(false);
                    Debug.Log("Dynamic Bones Enabled based on FPS");
                }
            }
        }
    }

    public void SetupShowTemplate()
    {
        foreach (CustomShowProperties property in customShowProperties)
        {
            CustomShowTemplate.SetActive(true);
            GameObject Clone = Instantiate(CustomShowTemplate, CustomShowTemplate.transform.parent, false );
            CustomShowTemplate.SetActive(false);
            Clone.GetComponent<TMP_Text>().text = property.Name;

            Clone.transform.Find("Inner Text").GetComponent<TMP_Text>().text = property.DefaultTextValue;

            Clone.transform.Find("Minus").GetComponent<Button>().onClick = property.MinusFunction;
            Clone.transform.Find("Plus").GetComponent<Button>().onClick = property.PlusFunction;

            Clone.name = property.Name;
            Clone.SetActive(true);
        }
    }
}

[System.Serializable]
public class CustomShowProperties
{
    public string Name;
    public Button.ButtonClickedEvent PlusFunction;
    public Button.ButtonClickedEvent MinusFunction;
    public string DefaultTextValue;
    public bool passSender;
}