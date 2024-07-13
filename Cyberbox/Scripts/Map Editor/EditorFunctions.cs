using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class EditorFunctions : MonoBehaviour
{
    MapCreator creator;

    [Header("Runtime Info")]
    public string mapUrl;

    [Header("Saving")]
    public List<GameObject_Struct> saveData;

    [Header("Objects")]
    TMP_Text cornerText;

    // Start is called before the first frame update
    void Start()
    {
        creator = GameObject.Find("Settings").GetComponent<MapCreator>();
        cornerText = GameObject.Find("CornerText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void New()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Save()
    {
        StartCoroutine(SaveData());
    }

    IEnumerator SaveData()
    {
        cornerText.text = "Saving...";

        if (mapUrl != "")
        {
            saveData.Clear();

            foreach (Transform obj in creator.Hierarchy.transform)
            {
                HierarchyProperties props = obj.GetComponent<HierarchyProperties>();
                // Convert Transform data to Transform_Struct
                GameObject_Struct transformData = new GameObject_Struct();

                transformData.name = props.gameObject.name;
                transformData.type = props.Type;
                transformData.icon = props.Icon.name;

                transformData.posX = obj.position.x.ToString();
                transformData.posY = obj.position.y.ToString();
                transformData.posZ = obj.position.z.ToString();
                transformData.rotX = obj.rotation.eulerAngles.x.ToString();
                transformData.rotY = obj.rotation.eulerAngles.y.ToString();
                transformData.rotZ = obj.rotation.eulerAngles.z.ToString();
                transformData.scaleX = obj.localScale.x.ToString();
                transformData.scaleY = obj.localScale.y.ToString();
                transformData.scaleZ = obj.localScale.z.ToString();

                saveData.Add(transformData);
            }

            try
            {
                File.WriteAllText(mapUrl, JsonHelper.ToJson(saveData.ToArray(), false));
                cornerText.text = "Last Saved " + DateTime.Now.ToString("HH:mm:ss tt"); ;
                Debug.Log("Successfully Saved - " + DateTime.Now);
            }
            catch (Exception ex)
            {
                cornerText.text = "Last Saved Failed";
                Debug.LogError("Save Failed: " + ex);
            }

        }
        else
        {
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Showtape Central Map", ".scmap"));

            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Select Folder or File to Save", "Save");

            if (FileBrowser.Success)
            {
                mapUrl = FileBrowser.Result[0];

                StartCoroutine(SaveData());
            }
        }
    }

    public void Add(string obj)
    {
        GameObject newObj = Instantiate(Resources.Load<GameObject>("Object Prefabs/" + obj));
        newObj.transform.SetParent(creator.Hierarchy.transform, true);
        newObj.transform.SetPositionAndRotation(new Vector3(0, 2.5f, 0), new Quaternion(0, 0, 0, 0));
        creator.RegenerateHierarchy();
    }
}

[Serializable]
public class GameObject_Struct
{
    public string name;
    public string type;
    public string icon;

    public string posX;
    public string posY;
    public string posZ;

    public string rotX;
    public string rotY;
    public string rotZ;

    public string scaleX;
    public string scaleY;
    public string scaleZ;
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}