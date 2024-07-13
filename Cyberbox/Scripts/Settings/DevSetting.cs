using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSetting : MonoBehaviour
{
    bool Toggled = false;
    float PSICache;
    public enum Option
    {
        cameraprojection,
        jellymode,
        mackvalvemadness,
    }
    public Option option;

    void Start()
    {
        PSICache = GameObject.Find("Mack Valves").GetComponent<Mack_Valves>().PSI;
    }
    public void Toggle()
    {
        switch(option) 
        {
            case Option.cameraprojection:
                if (Toggled == true) 
                {
                    GameObject.Find("Player").GetComponentInChildren<Camera>().orthographic = false;
                    Toggled = false;
                }
                else
                {
                    GameObject.Find("Player").GetComponentInChildren<Camera>().orthographic = true;
                    Toggled = true;
                }
                break;
            case Option.jellymode:
                if (Toggled == false)
                {
                    DynamicBone dynamicBone = GameObject.Find("Characters").AddComponent<DynamicBone>();
                    dynamicBone.m_Damping = 0;
                    dynamicBone.m_Stiffness = 0;
                    dynamicBone.m_Elasticity = 0;
                    dynamicBone.m_Root = GameObject.Find("Characters").GetComponent<Transform>();
                    Toggled = true;
                }
                else
                {
                    Toggled = false;
                }
                break;
            case Option.mackvalvemadness:
                if (Toggled == true)
                {
                    GameObject.Find("Mack Valves").GetComponent<Mack_Valves>().PSI = PSICache;
                    Toggled = false;
                }
                else
                {
                    GameObject.Find("Mack Valves").GetComponent<Mack_Valves>().PSI = 5000f;
                    Toggled = true;
                }
                break;
        }
    }
}
