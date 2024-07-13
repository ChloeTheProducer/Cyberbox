using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileSystemPreparer : MonoBehaviour
{
    public string ShowtapeFolder;
    public string GameFolder;

    void Awake()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string randomGamesPath = Path.Combine(appDataPath, "randomgames");
        GameFolder = Path.Combine(randomGamesPath, "Showtape Central");
        ShowtapeFolder = Path.Combine(GameFolder, "Showtapes");

        // The folder for the roaming current user 
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        try
        {
            if (!Directory.Exists(ShowtapeFolder))
            {
                Directory.CreateDirectory(ShowtapeFolder);
                Debug.Log("Showtapes folder created successfully!");
            }
            else
            {
                Debug.Log("Showtapes folder already exists!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error creating Showtapes folder: " + e.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
