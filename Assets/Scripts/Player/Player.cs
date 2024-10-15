using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Camera")]
    public Camera playerCamera;
    public float sensitivity;
    public float zoomSensitivity = 10f; // Sensitivity for zooming with the scroll wheel
    private float minYAngle = -89f; // Minimum vertical angle (looking up)
    private float maxYAngle = 89f;  // Maximum vertical angle (looking down)

    public float minFOV = 15f; // Minimum field of view for the camera
    public float maxFOV = 90f; // Maximum field of view for the camera

    private float targetFOV;

    // Variables to store the current rotation around the X and Y axes
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Slerp speed for smooth camera rotation
    public float rotationSlerpSpeed;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private Vector3 velocity;
    private CharacterController controller;

    [Header("UI")]
    public GameObject[] panels;
    public RawImage crosshairObj;
    public Texture2D[] crosshairs;

    GUIHandler _guiHandler;

    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        _guiHandler = GetComponentInChildren<GUIHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _guiHandler.SendNotification("Welcome", $"Welcome to {SceneManager.GetActiveScene().name}!", 3);

        // Initialize target FOV
        targetFOV = playerCamera.fieldOfView;
        
        _guiHandler.SendPanel("Control Show", panels[0], GUIHandler.AnchorPosition.BottomLeft);
    }

    private void Update()
    {
        HandlePlayerCamera();
        HandlePlayerMovement();
    }

    private void HandlePlayerCamera()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Capture the mouse movement
            var mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Adjust the vertical angle (pitch) based on mouse Y movement
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);

            // Adjust the horizontal angle (yaw) based on mouse X movement
            yRotation += mouseX;

            // Calculate the target rotation based on xRotation and yRotation
            var targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);

            // Smoothly interpolate the camera's rotation towards the target rotation
            playerCamera.transform.localRotation = Quaternion.Slerp(
                playerCamera.transform.localRotation,
                targetRotation,
                rotationSlerpSpeed * Time.deltaTime
            );

            // Handle camera zoom
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            targetFOV -= scrollInput * zoomSensitivity;
            targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, 10f * Time.deltaTime);
        }

        if (!Input.GetMouseButtonDown(1)) return;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            crosshairObj.enabled = false;
            _guiHandler.hideableElements.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshairObj.enabled = true;
            _guiHandler.hideableElements.SetActive(false);
        }
    }

    private void HandlePlayerMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        // Get the forward direction of the camera, but flatten it to be parallel to the ground
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f; // Ensure the movement is only on the XZ plane
        cameraForward.Normalize();

        // Get the right direction of the camera
        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f; // Ensure the movement is only on the XZ plane
        cameraRight.Normalize();

        // Calculate the direction to move in based on camera's forward and right vectors
        Vector3 move = cameraRight * x + cameraForward * z;

        controller.Move(move * (moveSpeed * Time.deltaTime));

        // Apply gravity
        if (controller.isGrounded)
        {
            velocity.y = -0.5f; // Small downward force to keep the player grounded
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void SetCursor(int i)
    {
        crosshairObj.texture = crosshairs[i];
    }
}
