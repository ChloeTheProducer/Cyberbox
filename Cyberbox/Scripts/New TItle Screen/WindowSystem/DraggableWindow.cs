using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IDragHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    void Start()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
        canvas = transform.root.GetComponentInChildren<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
