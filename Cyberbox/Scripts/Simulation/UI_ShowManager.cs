using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShowManager : MonoBehaviour
{
    public UI_PlayRecord playRecord;
    public TMP_Text stageText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate()
    {
        stageText.text = (playRecord.stages[playRecord.currentStage].stageName + " (" + (playRecord.currentStage + 1) + "/" + playRecord.stages.Length + ")");
    }
}
