using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Maps : MonoBehaviour
{
    public GameObject mapTemplate;
    public MapCollection[] maps;

    // Start is called before the first frame update
    void Start()
    {
        foreach(MapCollection map in maps)
        {
            GameObject newObj = Instantiate(mapTemplate, mapTemplate.transform.parent);
            newObj.name = map.name;
            newObj.transform.GetComponentInChildren<TMP_Text>().text = map.name;
            newObj.transform.Find("Image").GetComponent<Image>().sprite = map.image;
            newObj.GetComponent<LoadScene>().sceneName = map.sceneName;
        }
        mapTemplate.SetActive(false);
    }
}


// Class for the Map array
[Serializable]
public class MapCollection
{
    public string name;
    public Sprite image;
    public string sceneName;
}