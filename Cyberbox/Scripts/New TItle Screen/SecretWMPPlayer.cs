using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for the secret WMP player shortcut accessed by CTRL + SHIFT + ENTER
/// </summary>
public class SecretWMPPlayer : MonoBehaviour
{
    TitleManager titleManager;
    AudioSource audioSource;
    public GameObject WMPWindow;
    public GameObject Background;
    public GameObject[] ItemsToDisable;
    public GameObject[] ItemsToEnable;

    [Header("Images")]
    public Texture2D background;
    public Texture2D cursor;

    [Header("Audio")]
    public AudioClip startup;

    // Start is called before the first frame update
    void Start()
    {
        titleManager = GetComponent<TitleManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HashSet<KeyCode> currentlyPressedKeys = new HashSet<KeyCode>();

    private void OnGUI()
    {
        if (!Event.current.isKey) return;

        if (Event.current.keyCode != KeyCode.None)
        {
            if (Event.current.type == EventType.KeyDown)
            {
                currentlyPressedKeys.Add(Event.current.keyCode);
            }
            else if (Event.current.type == EventType.KeyUp)
            {
                currentlyPressedKeys.Remove(Event.current.keyCode);
            }
        }

        // Check for Ctrl, Shift, and Enter
        if (Event.current.control && Event.current.shift && Event.current.keyCode == KeyCode.Return)
        {
            Debug.Log("Let's get this party started!");
            StartCoroutine(SecretThing());
        }
    }

    public IEnumerator SecretThing()
    {
        titleManager.CloseAllPanels();
        titleManager.SwitchMenuPanel(WMPWindow);
        Background.transform.Find("Backdrop").GetComponent<RawImage>().texture = background;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        foreach (GameObject obj in ItemsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in ItemsToEnable)
        {
            obj.SetActive(true);
        }
        audioSource.Stop();
        gameObject.GetComponent<AudioReverbFilter>().enabled = false;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(startup);

        while (true)
        {
            // Generate a random delay between 10 and 30 seconds
            float randomDelay = Random.Range(5f, 30f);
            yield return new WaitForSeconds(randomDelay);

            // Open a random content message box
            string[] messages = {
                "Why are we still here... Just to suffer.",
                "amogus", "Train on the water...", "Boat on the track.",
                "You thought I was gonna say tits.", "Error: System32 deleted!",
                "sudo rm -rf ~", "Is this really your computer, or are you 'borrowing' it",
                "Behind you!", "Found your 'Homework' folder!", "I think I heard your neighbor fall down their stairs.",
                "Spaghetti!", "Leeeeerooooyyy Jeeeeenkiiiiinsssss", "Anyone else smell burnt toast",
                "Nice CPU. Would be a shame if I smashed it with a hammer!",
                "Screw windows media player, install VLC.", "You should use VIM.",
                "Who are you talking to?", "Why tho!", "Why is your desktop so empty.",
                "You should use linux.", "Really, Windows Vista? Go back to Windows 98, nerd!",
                "SOMEONE HELP, I'M TRAPPED IN THE POWER SUPPLY!", "Why am I running on an acer?", "Are you Bill Gates?",
                "I've changed you password.", "Jenny I got your number.", "867-5309", "Randombeans is cool!",
                "I think you left your oven on?", "Well Seymour I made it, despite your directions.", "Why is there smoke coming out of your oven?",
                "Good lord, what is happening in there!", "So long, gay bowser!",
                "Failed to run CHKDSK, please contact ... for more information.", "Failed to run task manager.",
                "Error", "Failed to run", "Failed to run Windows Media Player. Please contact.", "VLC, Why not Windows Media Player?",
                "CHSDSK will now shut down your system due to the error!", "Initializing virus...", "Loading...",
                "Error 2: Electric Boogaloo", "Initialzing RAM!",
            "Windows Defender found 5 errors with your computer.",
                "Unknown error.", "Your computer is not performant enough to run Windows Vista.",
                "Memory Error", "Failed to connect to the internet.",
                "*portal turret noises*",
                "Why are you watching Hatsune Miku?",
                "CPU Error",
                "A RAM stick has been detected as missing.",
                "One of your RAM sticks fell out.",
                "Critical error. Unknown reason.",
                "Failed to open Help", "Karen, where are the kids?" };

            int randomIndex = Random.Range(0, messages.Length);
            titleManager.OpenMessageBox("Error", messages[randomIndex], Random.Range(1, 5));
        }
    }

}
