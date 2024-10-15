using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static LeanTween;

[RequireComponent(typeof(Canvas))]
public class GUIHandler : MonoBehaviour
{
    Player player;

    [Header("Objects")] public GameObject panelPrefab;
    public GameObject messageBoxPrefab;
    public GameObject notificationPrefab;
    public Sprite[] notificationImages;
    public GameObject hideableElements;


    [Header("Audio")] public AudioClip messageBoxSound;
    public AudioClip notificationSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject.transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendNotification(string title, string description, int iconIndex)
    {
        // Ensure we have a valid notification prefab
        if (notificationPrefab == null)
        {
            Debug.LogError("Notification prefab is not assigned!");
            return;
        }

        // Ensure the icon index is within the valid range
        if (iconIndex < 0 || iconIndex >= notificationImages.Length)
        {
            Debug.LogError("Invalid icon index!");
            return;
        }

        // Instantiate the notification prefab
        GameObject notifInstance = Instantiate(notificationPrefab);

        // Set the parent to the Canvas
        notifInstance.transform.SetParent(transform.Find("NotificationTray").transform, false);

        // Get the RectTransform of the notification
        RectTransform notifRectTransform = notifInstance.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = notifInstance.GetComponent<CanvasGroup>();

        // Ensure we have a valid RectTransform
        if (notifRectTransform == null)
        {
            Debug.LogError("Notification prefab does not have a RectTransform component!");
            return;
        }

        // Set initial position off-screen
        notifRectTransform.anchorMin = new Vector2(1, 0); // Bottom right corner
        notifRectTransform.anchorMax = new Vector2(1, 0); // Bottom right corner
        notifRectTransform.pivot = new Vector2(1, 0); // Bottom right corner
        notifRectTransform.anchoredPosition += new Vector2(-25, 100);

        // Set up the Icon
        Transform iconTransform = notifInstance.transform.Find("Icon");
        if (iconTransform != null)
        {
            Image iconImage = iconTransform.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = notificationImages[iconIndex]; // Set the icon sprite
            }
        }
        else
        {
            Debug.LogError("Icon child not found in the notification prefab!");
        }

        // Set up the Title and Description
        Transform boxTransform = notifInstance.transform.Find("Box");
        if (boxTransform != null)
        {
            TMP_Text titleText = boxTransform.Find("Title")?.GetComponent<TMP_Text>();
            TMP_Text descriptionText = boxTransform.Find("Description")?.GetComponent<TMP_Text>();

            if (titleText != null)
            {
                titleText.text = title;
            }
            else
            {
                Debug.LogError("Title TMP_Text not found in the Box child of the notification prefab!");
            }

            if (descriptionText != null)
            {
                descriptionText.text = description;
            }
            else
            {
                Debug.LogError("Description TMP_Text not found in the Box child of the notification prefab!");
            }
        }
        else
        {
            Debug.LogError("Box child not found in the notification prefab!");
        }

        player.audioSource.PlayOneShot(notificationSound);

        // Make the notification visible instantly
        notifRectTransform.gameObject.SetActive(true);

        // Set the initial opacity to fully visible (1)
        alphaCanvas(canvasGroup, 1f, 0f).setOnComplete(() =>
        {
            // Schedule the notification to fade out and destroy itself after a delay
            alphaCanvas(canvasGroup, 0f, 1f)
                .setDelay(title.Length / 2 + 5)
                .setEase(LeanTweenType.easeOutSine)
                .setOnComplete(() => Destroy(notifInstance));
        });

    }

    public void SendMessageBox(string title, string description, string[] buttons)
    {
        GameObject messageBoxInstance = Instantiate(messageBoxPrefab);
        messageBoxInstance.transform.SetParent(transform, false);
        messageBoxInstance.transform.Find("Title").GetComponent<TMP_Text>().text = title;
        messageBoxInstance.transform.Find("Description").GetComponent<TMP_Text>().text = description;
        GameObject button = messageBoxInstance.transform.Find("Buttons/UI_Button").gameObject;
        button.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject newButton = Instantiate(button);
            newButton.SetActive(true);
            newButton.transform.SetParent(button.transform.parent);
            newButton.GetComponentInChildren<TMP_Text>().text = buttons[i];
        }

        player.audioSource.PlayOneShot(messageBoxSound);
    }

    public void SendPanel(string title, GameObject contentRef, AnchorPosition anchorPosition)
    {
        var panel = Instantiate(panelPrefab, hideableElements.transform);
        var content = Instantiate(contentRef, panel.transform);
        content.SetActive(true);
        panel.SetActive(true);
        var layout = panel.AddComponent<LayoutElement>();
        var rectTransform = panel.GetComponent<RectTransform>();

        // Set preferred width and height
        layout.preferredWidth = rectTransform.rect.width;
        layout.preferredHeight = rectTransform.rect.height;

        // Set anchor and position
        switch (anchorPosition)
        {
            case AnchorPosition.TopLeft:
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = new Vector2(15, -15);
                break;
            case AnchorPosition.TopRight:
                rectTransform.anchorMin = new Vector2(1, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(1, 1);
                rectTransform.anchoredPosition = new Vector2(-15, -15);
                break;
            case AnchorPosition.BottomLeft:
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0, 0);
                rectTransform.anchoredPosition = new Vector2(15, 15);
                break;
            case AnchorPosition.BottomRight:
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.pivot = new Vector2(1, 0);
                rectTransform.anchoredPosition = new Vector2(-15, 15);
                break;
            case AnchorPosition.Center:
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = Vector2.zero; // Center with no offset
                break;
        }

        // Set the title
        content.transform.Find("Topbar/Title").GetComponent<TMP_Text>().text = title;
    }

    public enum AnchorPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }
}
