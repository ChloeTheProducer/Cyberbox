using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static LeanTween;
using System;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This handles the messagebox window system, despite the name. Also some GUI window stuff
/// </summary>
public class TitleManager : MonoBehaviour
{
    public bool standaloneMode;
    public bool adjustToRectTransform;
    [Header("Window System")]
    public GameObject windowTemplate;
    public GameObject windowHolder;

    public GameObject lastOpenedWindow;

    private bool isSwitchingMenu = false; // Flag to track the switching state
    public GameObject GUIHolder;


    [Header("Day Soundtrack System")]
    private AudioSource audioSource;
    public AudioClip[] dayClips; // An array to store clips for each day

    public Texture2D CursorDefault;
    public Texture2D CursorSelected;

    [Header("Messagebox System")]
    public GameObject messageBox;
    public Sprite[] icons;
    public AudioClip[] iconSound;
    AudioSource uiSounds;

    void Start()
    {
        
        if (standaloneMode != true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Cursor.SetCursor(CursorDefault, Vector2.zero, CursorMode.Auto);
            audioSource = GetComponent<AudioSource>();
            DayOfWeek wk = DateTime.Today.DayOfWeek;

            // Check if wk is within the valid range (0-6) and play the corresponding clip
            if ((int)wk >= 0 && (int)wk < dayClips.Length)
            {
                audioSource.clip = dayClips[(int)wk];
                audioSource.Play();
            }
        }

        windowTemplate.SetActive(false);
        uiSounds = new GameObject("uiSounds").AddComponent<AudioSource>();
    }

    public void SwitchMenuPanel(GameObject targetPanel)
    {
        windowTemplate.SetActive(true);
        GameObject newObj = Instantiate(windowTemplate, windowHolder.transform);
        targetPanel = Instantiate(targetPanel, newObj.transform);
        newObj.SetActive(true);
        targetPanel.SetActive(true);
        RectTransform rectTransform = targetPanel.GetComponent<RectTransform>();
        if (adjustToRectTransform)
        {
            newObj.GetComponent<RectTransform>().sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }
        else // Legacy "Fill" scaling on some windows
        {
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }
        windowTemplate.SetActive(false);

        // Set initial scale to 0.95
        newObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        // Fade in and scale up animation
        alphaCanvas(newObj.GetComponent<CanvasGroup>(), 0f, 0f);
        alphaCanvas(targetPanel.GetComponent<CanvasGroup>(), 0f, 0f);
        scale(newObj, Vector3.one, 0.25f).setEase(LeanTweenType.easeInOutSine);
        scale(targetPanel, Vector3.one, 0.25f).setEase(LeanTweenType.easeInOutSine);
        alphaCanvas(newObj.GetComponent<CanvasGroup>(), 1f, 0.25f);
        alphaCanvas(targetPanel.GetComponent<CanvasGroup>(), 1f, 0.25f);
        lastOpenedWindow = newObj;
    }

    public void OpenMessageBox(string title, string content, int image)
    {
        SwitchMenuPanel(messageBox);
        lastOpenedWindow.transform.Find("MessageBox(Clone)/TopbarText").GetComponent<TMP_Text>().text = title;
        lastOpenedWindow.transform.Find("MessageBox(Clone)/Content").GetComponent<TMP_Text>().text = content;
        lastOpenedWindow.transform.Find("MessageBox(Clone)/Image").GetComponent<Image>().sprite = icons[image];
        uiSounds.PlayOneShot(iconSound[image]);
    }

    public void CloseAllPanels()
    {
        foreach(Transform obj in windowHolder.transform)
        {
            CloseWindow(obj.gameObject);
        }
    }

    public void CloseWindow(GameObject gameObject)
    {
        StartCoroutine(CloseWindowCoroutine(gameObject));
    }

    private IEnumerator CloseWindowCoroutine(GameObject gameObject)
    {
        alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0f, 0.25f);
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
        gameObject.SetActive(false);

    }

    public void SwitchGuiPanel(GameObject newPanel)
    {
        newPanel.SetActive(true);
        foreach (Transform childTransform in GUIHolder.transform)
        {
            if (childTransform.gameObject.activeSelf)
            {
                if (childTransform.gameObject != newPanel)
                {
                    scale(childTransform.gameObject, new Vector3(0.9f, 0.9f, 0.9f), 0.25f);
                    alphaCanvas(childTransform.gameObject.GetComponent<CanvasGroup>(), 0f, 0.25f).setOnComplete(() => { childTransform.gameObject.SetActive(false); }); ;

                    scale(newPanel, new Vector3(0.9f, 0.9f, 0.9f), 0f);
                    alphaCanvas(newPanel.GetComponent<CanvasGroup>(), 0f, 0f);

                }
            }
        }
        scale(newPanel, new Vector3(1f, 1f, 1f), 0.25f);
        alphaCanvas(newPanel.GetComponent<CanvasGroup>(), 1f, 0.25f);

    }

    public void LaunchURL(string url)
    {
        Application.OpenURL(url);
    }

    public void Quit()
    {
        Application.Quit();
    }
}