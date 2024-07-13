using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttonparallax : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float maxOffset = 5f;
    public float parallaxSpeed = 10f;
    public float maxRotation = 10f;

    private Vector3 originalPos;
    private Quaternion originalRotation;
    private RectTransform rectTransform;
    private bool isHovering = false;

    void Start()
    {
        // Get the original position and rotation of the button
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
    }

    void Update()
    {
        if (isHovering)
        {
            // Calculate the parallax offset based on the cursor position
            Vector3 mousePos = Input.mousePosition;
            Vector3 buttonPos = rectTransform.position;
            float offset = Mathf.Clamp(Vector3.Distance(mousePos, buttonPos) / parallaxSpeed, 0f, maxOffset);

            // Apply the parallax offset to the button's position
            rectTransform.localPosition = originalPos + new Vector3(offset, -offset, 0f);

            // Calculate the rotation based on the cursor position
            float rotationX = Mathf.Clamp((mousePos.y - buttonPos.y) / parallaxSpeed, -maxRotation, maxRotation);
            float rotationY = Mathf.Clamp((mousePos.x - buttonPos.x) / parallaxSpeed, -maxRotation, maxRotation);

            // Apply the rotation to the button's rotation
            rectTransform.localRotation = originalRotation * Quaternion.Euler(-rotationX, rotationY, 0f);
        }
        else
        {
            // Reset the button's position and rotation when not hovering
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, originalPos, Time.deltaTime * parallaxSpeed);
            rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, originalRotation, Time.deltaTime * parallaxSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set isHovering to true when the cursor enters the button
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the button's position and rotation when the cursor exits the button
        isHovering = false;
    }
}
