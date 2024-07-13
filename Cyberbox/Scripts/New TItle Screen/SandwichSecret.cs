using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for the secret sandwich shortcut accessed by CTRL + SHIFT + BACKSPACE
/// </summary>
public class SandwichSecret : MonoBehaviour
{
    TitleManager titleManager;
    AudioSource audioSource;
    public GameObject Sandwich;
    public GameObject Background;
    public GameObject[] ItemsToDisable;
    public GameObject[] ItemsToEnable;

    [Header("Images")]
    public Texture2D background;

    [Header("Audio")]
    public AudioClip Heavy;

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
        if (Event.current.control && Event.current.shift && Event.current.keyCode == KeyCode.Tab)
        {
            Debug.Log("Let's get this party started!");
            StartCoroutine(SecretThing());
        }
    }

    public IEnumerator SecretThing()
    {
        titleManager.CloseAllPanels();
        titleManager.SwitchMenuPanel(Sandwich);
        Background.transform.Find("Backdrop").GetComponent<RawImage>().texture = background;
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
        audioSource.PlayOneShot(Heavy);

        while (true)
        {
          // Generate a random delay between 10 and 30 seconds
            float randomDelay = Random.Range(5f, 30f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

}
