using RuntimeHandle;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

[RequireComponent(typeof (AudioSource))]
public class MapCreator : MonoBehaviour
{
    [Header("Objects")]
    public GameObject libraryHolder;
    public LibraryObject[] libraryObjects;


    [Header("Variables")]
    bool isPlaying;
    public EditorFunctions functions;

    [HideInInspector] public Material selectedMaterial;

    Ray ray;
    RaycastHit hit;
    AudioSource audioSource;
    [HideInInspector] public string currentlySelectedTool;

    public SelectTransformGizmo selectTransformGizmo;

    public Texture2D defaultCursor;

    public Camera MainCamera;

    public GameObject Controls;
    public GameObject ToolTemplate;
    public GameObject Hierarchy;
    public GameObject HierarchyTemplate;

    [Header("Events")]
    public UnityEvent onRun;
    public UnityEvent onStop;

    [Header("Tools")]
    public Tools[] gameTools;

    [Header("Runtime")]
    public GameObject PlayerPrefab;
    

    [Header("Sounds")]
    public AudioClip toolChange;

    public AudioClip destroy;
    public AudioClip clone;

    public AudioClip scale;

    [Header("Windows")]

    public GameObject materialTemplate;

    GameObject handleObj;

    [Header("TextObjects")]
    public TMP_Text Console;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ToolTemplate.SetActive(true);
        for (int i = 0; i < gameTools.Length; i++)
        {
            GameObject newObj = Instantiate(ToolTemplate, ToolTemplate.transform.parent);
            newObj.transform.Find("Image").GetComponent<Image>().sprite = gameTools[i].Icon;
            gameTools[i].gameObject = newObj;
            newObj.name = gameTools[i].name;
            newObj.GetComponent<Button>().interactable = true;
        }
        ToolTemplate.SetActive(false);

        handleObj = selectTransformGizmo.runtimeTransformHandle.gameObject;

        Application.logMessageReceived += LogToConsole;

        LogToConsole("Welcome to the Showtape Central Editor! <3", "", LogType.Log);

        RegenerateHierarchy();
        RegenerateLibrary();
    }

    // Update is called once per frame
    void Update()
    {
        ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        var input = Input.inputString;
        // Convert the input string to an integer
        if (int.TryParse(input, out int index))
        {
            // Check if the index is within the bounds of the gameTools array
            if (index >= 1 && index <= gameTools.Length)
            {
                // Loop through all game tools
                for (int i = 0; i < gameTools.Length; i++)
                {
                    currentlySelectedTool = gameTools[index - 1].name;
                    // Check if the current tool is the selected one
                    if (i == index - 1)
                    {
                        // Enable the selected tool
                        gameTools[i].onEnable.Invoke();
                        gameTools[i].gameObject.GetComponent<Button>().interactable = false;
                        
                        if (gameTools[i].EnableCustomCursor)
                        {
                            Cursor.SetCursor(gameTools[i].Cursor, new Vector2(40, 40), CursorMode.ForceSoftware);
                        }
                        else
                        {
                            Cursor.SetCursor(defaultCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
                        }

                        audioSource.PlayOneShot(toolChange);
                    }
                    else
                    {
                        // Disable interaction for other tools
                        gameTools[i].gameObject.GetComponent<Button>().interactable = true;
                    }
                }
            }
        }

        StartCoroutine(ToolCheck());
    }

    public void ToggleRun()
    {
        if (isPlaying) // Stop
        {
            onRun.Invoke();
            GameObject player = Instantiate(PlayerPrefab, transform);
            player.name = "runtimePlayer";
        }
        else // Play
        {
            Destroy(GameObject.Find("runtimePlayer"));
            onStop.Invoke();
            isPlaying = true;
        }
    }

    void LogToConsole(string logString, string stackTrace, LogType type)
    {
        string styledLog = "";

        // Styling based on log type
        switch (type)
        {
            case LogType.Error:
                styledLog = "<color=red>Error: " + logString + "</color>";
                break;
            case LogType.Warning:
                styledLog = "<color=yellow>Warning: " + logString + "</color>";
                break;
            case LogType.Log:
                styledLog = "<color=black>" + logString + "</color>";
                break;
            case LogType.Assert:
                styledLog = "<color=orange>Assert: " + logString + "</color>";
                break;
            case LogType.Exception:
                styledLog = "<color=magenta>Exception: " + logString + "</color>";
                break;
            default:
                styledLog = logString;
                break;
        }

        // Append the new log with a new line
        Console.text += styledLog + "\n";

        // Optionally, limit the number of lines displayed to prevent text overflow
        // You can adjust the limit as needed
        int maxLines = 20;
        if (Console.text.Split('\n').Length > maxLines)
        {
            string[] lines = Console.text.Split('\n');
            Console.text = string.Join("\n", lines, lines.Length - maxLines, maxLines);
        }
    }

    IEnumerator ToolCheck()
    {
        selectTransformGizmo.enabled = true;

        switch (currentlySelectedTool)
        {
            case "Move":
                EnableHandles();
                selectTransformGizmo.runtimeTransformHandle.type = HandleType.POSITION;
                break;
            case "Rotate":
                EnableHandles();
                selectTransformGizmo.runtimeTransformHandle.type = HandleType.ROTATION;
                break;
            case "Scale":
                EnableHandles();
                selectTransformGizmo.runtimeTransformHandle.type = HandleType.SCALE;
                break;
            case "Delete":
                DisableHandles();
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.transform.root.name == "Objects")
                    {
                        audioSource.PlayOneShot(destroy);
                        Destroy(hit.collider.gameObject);
                    }
                }
                RegenerateHierarchy();
                break;
            case "Clone":
                DisableHandles();
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.transform.root.name == "Objects")
                        {
                            GameObject newObj = Instantiate(hit.collider.gameObject, hit.collider.gameObject.transform.root);
                            newObj.transform.position = new Vector3(newObj.transform.position.x, newObj.transform.position.y + 1, newObj.transform.position.z);
                            newObj.name = hit.collider.gameObject.transform.name;
                            audioSource.PlayOneShot(clone);
                        }
                    }
                }
                RegenerateHierarchy();
                break;
        }
        yield return null;
    }

    public void DisableHandles()
    {
        foreach (Transform child in GameObject.Find("Objects").transform)
        {
            child.tag = "Untagged";
        }
       handleObj.SetActive(false);
    }

    public void LoadMaterials()
    {
        materialTemplate.SetActive(true);
        foreach (Transform child in materialTemplate.transform.parent)
        {
            if (child.name != "Material")
            {
                Destroy(child);
            }
        }

        foreach (Material material in Resources.LoadAll<Material>("Live Materials"))
        {
            GameObject newObj = Instantiate(materialTemplate, materialTemplate.transform.parent);
            newObj.name = material.name;
            newObj.GetComponent<RawImage>().texture = material.mainTexture;
            newObj.GetComponent<Button>().onClick.AddListener(delegate { MaterialClick(material); });
        }

        materialTemplate.SetActive(false);
    }

    public void EnableHandles()
    {
        foreach (Transform child in GameObject.Find("Objects").transform)
        {
            child.tag = "Selectable";
        }
    }

    public void MaterialClick(Material material)
    {
        selectedMaterial = material;
    }

    public void RegenerateHierarchy()
    {
        foreach(Transform uiObj in HierarchyTemplate.transform.parent)
        {
            if (uiObj != HierarchyTemplate.transform)
            {
                Destroy(uiObj.gameObject);
            }
        }
        HierarchyTemplate.SetActive(true);
        foreach (Transform obj in Hierarchy.transform)
        {
            GameObject newObj = Instantiate(HierarchyTemplate, HierarchyTemplate.transform.parent).gameObject;
            newObj.name = obj.name;
            newObj.GetComponentInChildren<TMP_Text>().text = obj.name;
            newObj.transform.Find("Icon").GetComponent<Image>().sprite = obj.GetComponent<HierarchyProperties>().Icon;
        }
        HierarchyTemplate.SetActive(false);
    }

    public void RegenerateLibrary()
    {

        GameObject template =  libraryHolder.transform.Find("Template").gameObject;
        template.SetActive(true);

        foreach(LibraryObject obj in libraryObjects)
        {
            Transform gameObject = Instantiate(template, template.transform.parent).transform;
            gameObject.transform.Find("Text").GetComponent<TMP_Text>().text = obj.name;
            gameObject.transform.Find("Icon").GetComponent<Image>().sprite = obj.icon;
            gameObject.GetComponent<Button>().onClick.AddListener(delegate { functions.Add(obj.name); });
        }

        template.SetActive(false);
    }
}


[Serializable]
public class Tools
{
    public string name;
    [HideInInspector] public GameObject gameObject;
    public string Description;
    public Sprite Icon;
    public bool EnableCustomCursor;
    public Texture2D Cursor;
    public UnityEvent onEnable;
}

[Serializable]
public class LibraryObject
{
    public string name;
    public Sprite icon;
}

