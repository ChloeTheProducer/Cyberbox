using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using TMPro;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    AudioSource sc;

    Texture2D clickCursor;
    Texture2D hoverCursor;

    public AudioClip clickSound;
    public AudioClip hoverSound;

    public enum Animation
    {
        none,
        growText,
        spaceText,
        grow,
    }
    public Animation hoverAnimation = Animation.none;

    void Start()
    {
        sc = gameObject.AddComponent<AudioSource>();

        clickCursor = Resources.Load<Texture2D>("Cursors/CursorDefault");
        hoverCursor = Resources.Load<Texture2D>("Cursors/CursorLink");

        switch (hoverAnimation)
        {
            case Animation.none:
                break;
            case Animation.growText:
                defaultTextSize = GetComponentInChildren<TMP_Text>().fontSize;
                break;
        }
    }
    public enum HoverCursor
    {
        normal,
        link,
    }
    public HoverCursor hovercursor = HoverCursor.normal;

    float defaultTextSize;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null)
        {
            sc.PlayOneShot(clickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            sc.PlayOneShot(hoverSound);
        }
        switch (hovercursor)
        {
            case HoverCursor.normal:
                Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
                break;
            case HoverCursor.link:
                Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
        switch (hoverAnimation)
        {
            case Animation.none:
                break;
            case Animation.growText:
                TMP_Text text = GetComponentInChildren<TMP_Text>();
                text.fontSize = defaultTextSize;
                LeanTween.value(text.fontSize, text.fontSize + 10, 0.1f).setOnUpdate((float val) => { text.fontSize = val; }).setEaseInOutSine();
                break;
            case Animation.spaceText:
                TMP_Text text2 = GetComponentInChildren<TMP_Text>();
                text2.characterSpacing = 10;
                LeanTween.value(text2.characterSpacing, text2.characterSpacing + 5, 0.1f).setOnUpdate((float val) => { text2.characterSpacing = val; }).setEaseInOutSine();
                break;
            case Animation.grow:
                LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.1f).setEaseInOutSine();
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);

        switch (hoverAnimation)
        {
            case Animation.none:
                break;
            case Animation.growText:
                TMP_Text text = GetComponentInChildren<TMP_Text>();
                text.fontSize = defaultTextSize + 10;
                LeanTween.value(text.fontSize, text.fontSize - 5, 0.1f).setOnUpdate((float val) => { text.fontSize = val; }).setEaseInOutSine();
                break;
            case Animation.spaceText:
                TMP_Text text2 = GetComponentInChildren<TMP_Text>();
                text2.characterSpacing = 0;
                LeanTween.value(text2.characterSpacing, text2.characterSpacing - 10, 0.1f).setOnUpdate((float val) => { text2.characterSpacing = val; }).setEaseInOutSine();
                break;
            case Animation.grow:
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.1f).setEaseInOutSine();
                break;
        }
    }

    
}


