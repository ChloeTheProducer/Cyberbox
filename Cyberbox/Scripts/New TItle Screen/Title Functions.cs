using UnityEngine;
using System;
using System.IO;
using TMPro;
using SimpleFileBrowser;

public class TitleFunctions : MonoBehaviour
{
    public GameObject Topbar;
    public GameObject FolderCreationWarning;

    // Start is called before the first frame update
    string documentsFolder;
    string showtapeCentralPath;
    string flowFolderPath;
    string showtapeFolderPath;

    TitleManager titleManager;
    void Start()
    {
        titleManager = gameObject.GetComponent<TitleManager>();
        documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        showtapeCentralPath = PlayerPrefs.GetString("Data: Game Data Directory");
        if (string.IsNullOrEmpty(showtapeCentralPath)) 
        {
            showtapeCentralPath = Path.Combine(documentsFolder, "Showtape Central");
        }

        flowFolderPath = Path.Combine(showtapeCentralPath, "Flows");
        showtapeFolderPath = Path.Combine(showtapeCentralPath, "Showtapes");

        if (!Directory.Exists(showtapeFolderPath))
        {
            FolderCreationWarning.SetActive(true);
            FolderCreationWarning.transform.Find("Folder").GetComponent<TMP_Text>().text = showtapeCentralPath;
            Topbar.SetActive(false);
        }
        else
        {
            Topbar.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateFolder()
    {

        if (!Directory.Exists(showtapeCentralPath))
        {
            Directory.CreateDirectory(showtapeCentralPath);
        }

        if (!Directory.Exists(flowFolderPath))
        {
            Directory.CreateDirectory(flowFolderPath);
        }

        if (!Directory.Exists(showtapeFolderPath))
        {
            Directory.CreateDirectory(showtapeFolderPath);
        }

        PlayerPrefs.SetString("Data: Game Data Directory", showtapeCentralPath);
        PlayerPrefs.SetString("Data: Flow Folder", flowFolderPath);
        PlayerPrefs.SetString("Data: Showtape Folder", showtapeFolderPath);
    }
    public void ChangeDataDirectory()
    {
        FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select");
        if (FileBrowser.Success)
        {
            string docDirectory = FileBrowser.Result[0];
            showtapeCentralPath = Path.Combine(docDirectory, showtapeCentralPath);
            flowFolderPath = Path.Combine(showtapeCentralPath, "Flows");
            showtapeFolderPath = Path.Combine(showtapeCentralPath, "Showtapes");
            FolderCreationWarning.transform.Find("Folder").GetComponent<TMP_Text>().text = showtapeCentralPath;
        }
    }
}
