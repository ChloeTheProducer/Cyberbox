using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Curtain_Valves : MonoBehaviour
{

    [HideInInspector]
    public GameObject mackValves;
    [Range(0.0001f, .01f)]
    public float[] flowControlOut;
    [Range(0.0001f, .01f)]
    public float[] flowControlIn;

    Animator characterValves;
    List<int> cylindersTop = new List<int>();
    List<int> cylindersBottom = new List<int>();
    Mack_Valves bitChart;
    public bool[] curtainbools;
    public bool curtainOverride;
    public bool enableValveSounds;

    private void Start()
    {
        mackValves = GameObject.Find("Mack Valves");
        characterValves = this.GetComponent<Animator>();
        bitChart = mackValves.GetComponent<Mack_Valves>();
        for (int e = 0; e < characterValves.layerCount; e++)
        {
            string temp = characterValves.GetLayerName(e);
            if (temp[temp.Length - 1] == 'T')
            {
                cylindersTop.Add(Int32.Parse(temp.Substring(0, temp.Length - 1)));
            }
            else
            {

                cylindersBottom.Add(Int32.Parse(temp.Substring(0, temp.Length - 1)));
            }
        }
        curtainbools = new bool[characterValves.layerCount];
    }

    private void Update()
    {
        if (enableValveSounds)
        {
            if (curtainbools.Any(b => b))
            {
                GetComponent<AudioSource>().enabled = true;
            }
            else
            {
                GetComponent<AudioSource>().enabled = false;
            }
        }
    }

    public void CreateMovements(float num3, bool reverse)
    {
        //Loop through cylinders to update
        for (int i = 0; i < cylindersTop.Count; i++)
        {
            //Get current animation value
            SetTime("T", cylindersTop[i], bitChart.topDrawer, i, num3,reverse);
        }
        for (int i = 0; i < cylindersBottom.Count; i++)
        {
            //Get current animation value
            SetTime("B", cylindersBottom[i], bitChart.bottomDrawer, i, num3, reverse);
        }
    }

    void SetTime(string drawername, int currentAnim, bool[] drawer, int lasti, float num3, bool reverse)
    {
        //Cycle through parameters to find matching code
        for (int e = 0; e < characterValves.parameters.Length; e++)
        {
            if (characterValves.parameters[e].name.Substring(0, characterValves.parameters[e].name.Length - 1) == currentAnim.ToString())
            {
                //Calculate next value of the parameter
                float nextTime = characterValves.GetFloat(currentAnim.ToString() + drawername);

                //Check if animation is already done
                if (!curtainOverride)
                {
                    bool check = drawer[currentAnim - 1];
                    if(reverse)
                    {
                        check = !check;
                    }

                    if (nextTime == 0 && !check)
                    {
                        break;
                    }
                    if (nextTime == 1 && check)
                    {
                        break;
                    }
                }

                //Set bool
                if (drawer[currentAnim - 1])
                {
                    if(reverse)
                    {
                        curtainbools[lasti] = false;
                    }
                    else
                    {
                        curtainbools[lasti] = true;
                    }
                   
                }
                else if (drawer[currentAnim + 1 - 1])
                {
                    if (reverse)
                    {
                        curtainbools[lasti] = true;
                    }
                    else
                    {
                        curtainbools[lasti] = false;
                    }
                    
                }

                //Set Curtain
                if (!curtainOverride)
                {
                    if (curtainbools[lasti])
                    {
                        nextTime += (flowControlOut[e] * 1.25f * num3);
                    }
                    else
                    {
                        nextTime -= (flowControlIn[e] * 1.25f * num3);
                    }
                }
                else
                {
                    nextTime += (flowControlOut[e] * 1.25f * num3);
                }
                nextTime = Mathf.Min(Mathf.Max(nextTime, 0), 1);

                //Apply parameter
                characterValves.SetFloat(currentAnim.ToString() + drawername, nextTime);
                break;
            }
        }
    }
}
