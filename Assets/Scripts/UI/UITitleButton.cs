using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static LeanTween;

public class UITitleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rect;
    private Button _button;
    private CanvasGroup _canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
        
        _canvasGroup.alpha = 0;
        _rect.localScale = Vector3.zero;

        var ran = Random.Range(0.1f, 0.5f);
        
        alphaCanvas(_canvasGroup, 1, 1f).setDelay(ran).setEase(LeanTweenType.easeOutQuad);
        scale(_rect, Vector3.one, 1f).setDelay(ran).setEase(LeanTweenType.easeOutQuad);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (_button != _button.interactable) return;
        // Instantiate the panel
        var loadPanel = Instantiate(gameObject, FindFirstObjectByType<Canvas>().transform);
        Destroy(loadPanel.GetComponent<UITitleButton>());
        Destroy(loadPanel.transform.Find("Text").gameObject);
    
        // Get the RectTransform of the newly instantiated panel
        var loadPanelRect = loadPanel.GetComponent<RectTransform>();

        loadPanelRect.localScale = Vector3.one;
        loadPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        loadPanelRect.anchorMin = new Vector2(0.5f, 0.5f);

        // Ensure pivot is centered
        loadPanelRect.pivot = new Vector2(0.5f, 0.5f);

        // Animate the scale to fill the screen over time
        var canvasRect = FindFirstObjectByType<Canvas>().GetComponent<RectTransform>();

        // Animate both size and position to be centered
        size(loadPanelRect, canvasRect.sizeDelta, 0.5f).setEase(LeanTweenType.easeOutQuad);
        move(loadPanelRect, Vector2.zero, 0.5f).setEase(LeanTweenType.easeOutQuad);
    }

    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_button != _button.interactable) return;
        cancel(gameObject);
        scale(_rect, new Vector3(0.9f, 0.9f, 0.9f), 0.25f).setEase(LeanTweenType.easeOutQuad);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (_button != _button.interactable) return;
        cancel(gameObject);
        scale(_rect, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutQuad);
    }
}
