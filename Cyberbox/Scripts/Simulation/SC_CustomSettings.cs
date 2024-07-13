using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_CustomSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LooneySpring(int input)
    {

        GameObject Characters = GameObject.Find("Characters");
        if (input == 0)
        {
            Characters.transform.Find("Pizza Cam").transform.Find("LooneyShowbiz").GetComponent<DynamicBone>().enabled = false;
            transform.parent.Find("Main/Container/CustomSettings/Pizzacam Spring/Inner Text").GetComponent<TMP_Text>().text = "Disabled";
        }
        if (input == 1)
        {
            Characters.transform.Find("Pizza Cam").transform.Find("LooneyShowbiz").GetComponent<DynamicBone>().enabled = true;
            transform.parent.Find("Main/Container/CustomSettings/Pizzacam Spring/Inner Text").GetComponent<TMP_Text>().text = "Enabled";
        }
    }
}
