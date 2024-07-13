using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using static LeanTween;

public class PanelTween : MonoBehaviour
{
    public RectTransform panel;
    public float tweenTime = 1f;

    void Start()
    {
        // Get the starting and ending positions for the panel
        Vector2 startPos = new Vector2(Screen.width + panel.rect.width, panel.anchoredPosition.y);
        Vector2 endPos = new Vector2(0f, panel.anchoredPosition.y);

        // Set the starting position for the panel
        panel.anchoredPosition = startPos;

        // Tween the panel to the center of the screen
        LeanTween.move(panel, endPos, tweenTime).setEase(LeanTweenType.easeOutExpo);
    }
}
