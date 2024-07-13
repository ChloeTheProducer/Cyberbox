using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[HideInInspector]
public class SC_Flowset
{
    [Header("Information")]
    public string name;
    public string creator;
    public string description;
    public string date;
    public string creationdate;

    SC_CharacterData[] characterData;
}

public class SC_CharacterData
{
    public int CharacterID;
    public int[] FlowsIn;
    public int[] FlowsOut;
}
