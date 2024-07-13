using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static LeanTween;

public class Buttonhover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float duration = 0.5f;
    public int expandedWidth = 400;

    private int originalWidth;
    private RectTransform rectTransform;
    TitleManager titleManager;

    void Start()
    {
        // Get the original width of the button
        rectTransform = GetComponent<RectTransform>();
        originalWidth = (int)rectTransform.rect.width;
        // if (SceneManager.GetActiveScene().name == "Title Screen");
        // {
        //    titleManager = GameObject.Find("Title Manager").GetComponent<TitleManager>();
        // }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Tween the width of the button to the expanded width
        LeanTween.value(gameObject, originalWidth, expandedWidth, duration)
            .setOnUpdate((float val) =>
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, val);
            })
            .setEase(LeanTweenType.easeOutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Tween the width of the button back to the original width
        LeanTween.value(gameObject, expandedWidth, originalWidth, duration)
            .setOnUpdate((float val) =>
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, val);
            })
            .setEase(LeanTweenType.easeOutQuad);
    }
}
