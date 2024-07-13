using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelChanger : MonoBehaviour
{
    public enum Type
    {
        rayTraceCheck,
    }
    public Type type;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.supportsRayTracing == true) { gameObject.SetActive(false); } else { gameObject.SetActive(true); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
