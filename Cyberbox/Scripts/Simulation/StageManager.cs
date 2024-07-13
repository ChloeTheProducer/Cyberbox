using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class StageSelector
    {
        [Header("Stage")]
        public string stageName;
        public GameObject stage;
        [Header("Description")]

        [TextArea]
        public string stageDesc;
        public Sprite stageIcon;
        
        [Header("Characters")]
        public CharacterPos[] stageCharacters;
        [Header("Special Objects")]
        [HideInInspector]
        public GameObject curtain;
        [HideInInspector]
        public Curtain_Valves curtainValves;
        [HideInInspector]
        public GameObject lights;
        [HideInInspector]
        public LightController[] lightValves;
        public ShowTV[] tvs;

        public void Startup()
        {
             Transform stageCurtain = stage.gameObject.transform.Find("StageCurtains");
            if (stageCurtain != null )
            {
                curtain = stageCurtain.gameObject;
                curtainValves = curtain.GetComponent<Curtain_Valves>();
            }

            Transform lightsObject = stage.gameObject.transform.Find("Controlled Lights");
            if (lightsObject != null)
            {
                lights = lightsObject.gameObject;
            }
            //Find amount of lights
            int count = 0;
            foreach (Transform child in lights.transform)
            {
                count++;
                foreach (Transform grandChild in child)
                    count++;
            }
            lightValves = new LightController[count];

            //Apply Lights
            count = 0;
            foreach (Transform child in lights.transform)
            {
                lightValves[count] = child.GetComponent<LightController>();
                count++;
                foreach (Transform grandChild in child)
                {
                    lightValves[count] = grandChild.GetComponent<LightController>();
                    count++;
                } 
            }
        }
    }
    [System.Serializable]
    public class CharacterPos
    {
        public string characterName;
        public Vector3 characterPos;
        public Vector3 characterRot;
    }


    [System.Serializable]
    public class ShowTV
    {
        public tvSetting tvSettings;
        public enum tvSetting
        {
            offOnly,
            onOnly,
            offOn,
            none,
        }
        public bool onWhenCurtain;
        public bool drawer;
        public int bitOff;
        public int bitOn;
        public MeshRenderer[] tvs;

        public int curtainSubState;
    }
    public StageSelector[] stages;
    [HideInInspector]
    public int currentStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].stage.SetActive(i == 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
