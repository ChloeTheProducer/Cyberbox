using UnityEngine;
using SFB;
using System.IO;
using UnityEngine.Audio;

[RequireComponent (typeof(SHWHandler))]
public class PlaybackSystem : MonoBehaviour
{
    ShowSystem showSystem;
    Player player;
    GUIHandler guiHandler;
    SHWHandler shwHandler;
    public AudioSource audioSource;

    bool showtapePlaying;
    string format; // SHW, SHOWDATA

    void Start()
    {
        showSystem = gameObject.GetComponent<ShowSystem>();
        shwHandler = gameObject.GetComponent<SHWHandler>();
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        guiHandler = player.GetComponentInChildren<GUIHandler>();
    }

    void Update()
    {
        if (showtapePlaying)
        {
            if (format == "SHW")
            {
                shwHandler.PlaybackUpdate();
            }
        }
    }


    /// <summary>
    /// Loads a showtape using StandaloneFileBrowser
    /// </summary>
    public void Load()
    {
        var extensions = new[] {
            new ExtensionFilter("Showtape Files", "showdata", "rshw", "cshw", "sshw", "wshw", "yshw", "fshw" ),
            new ExtensionFilter("All Files", "*" ),
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        if (Path.GetExtension(paths[0]).EndsWith("shw"))
        {
            guiHandler.SendNotification("Warning", "The .*SHW format is highly unsafe. Please make sure the source of this showtape is trustworthy. For more information about the insecurities of the formatter used, go to https://aka.ms/binaryformatter", 2);
            shwHandler.Load(paths[0]);
            format = "SHW";
            showtapePlaying = true;
        }
    }

    /// <summary>
    /// Clears the showtape after it has finished
    /// </summary>
    public void Unload()
    {
        showtapePlaying = false;
        format = "";
    }

    /// <summary>
    /// Resumes playback of the showtape
    /// </summary>
    public void Resume()
    {

    }

    /// <summary>
    /// Pauses playback of the showtape
    /// </summary>
    public void Pause()
    {

    }
    
    /// <summary>
    /// Stops playback of the showtape
    /// </summary>
    public void Stop()
    {

    }
}
