using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using static LeanTween;
using System.Collections;
using NUnit.Framework;
using Unity.Netcode;

public class MovementSystem : NetworkBehaviour
{
    public enum MovementMode
    {
        NormalMovement,
        Fly,
    }

    public MovementMode mode = MovementMode.NormalMovement;
    public CharacterController characterController;
    public Camera mainCamera;

    // Camera Movement
    public float sensitivity = 2f;
    public float yRotationLimit = 88f;
    private Vector2 rotation = Vector2.zero;

    // Movement
    public float movementSpeed = 1f;
    public float sprintspeed = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    // Ground Check
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayerMask;

    public TMP_Text modeText;

    public PlayerInput playerInput;

    public Animator animator;

    bool isGrounded;

    float finalSpeed;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            characterController.enabled = false;
            playerInput.enabled = false;
        }
        if (NetworkManager)
        {
            animator.enabled = false;
            transform.parent.Find("Character").gameObject.SetActive(false);
        }

        if (characterController == null)
            characterController = transform.parent.GetComponent<CharacterController>();

        if (mainCamera == null)
            mainCamera = transform.parent.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.05f);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleMovementMode();
            StartCoroutine(UpdateModeText());
        }

        if (mode == MovementMode.NormalMovement)
        {
            HandleCameraMovement();
            HandlePlayerMovement();
            HandleGravity();
            if (NetworkManager) 
            {
                HandleAnimation();
            }
        }
        else if (mode == MovementMode.Fly)
        {
            HandleCameraMovement();
            HandlePlayerMovement();
        }
    }

    private void ToggleMovementMode()
    {
        if (mode == MovementMode.NormalMovement)
        {
            mode = MovementMode.Fly;
            characterController.isTrigger = false;
        }
        else
        {
            mode = MovementMode.NormalMovement;
        }
    }
    private IEnumerator UpdateModeText()
    {
        modeText.enabled = true;
        if (modeText != null)
        {
            if (mode == MovementMode.NormalMovement)
                modeText.text = "Walk Movement Mode";
            else
                modeText.text = "Fly Movement Mode";
        }
        yield return new WaitForSeconds(2);
        modeText.enabled = false;
    }

    private void HandleCameraMovement()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            rotation.x += Input.GetAxis("Mouse X") * sensitivity;
            rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

            mainCamera.transform.localRotation = xQuat * yQuat;

            // Camera Zoom
            mainCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 25f;
        }
    }

    private void HandlePlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (mode != MovementMode.Fly)
        {
            if (!Input.GetKey(KeyCode.LeftShift)) { finalSpeed = sprintspeed; } else { finalSpeed = movementSpeed; } // Shift to Run
        }
        else { finalSpeed = movementSpeed; }

        Vector3 move = (x * mainCamera.transform.right + z * mainCamera.transform.forward) * finalSpeed * Time.deltaTime;

        characterController.Move(move);  
    }

    private void HandleGravity()
    {
        // Apply gravity
        if (!isGrounded!)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(-2 * gravity * 1f); // Calculate initial vertical velocity for a desired jump height
        }
    }

    private void HandleAnimation()
    {
        if (characterController.velocity != Vector3.zero)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }
    }
}
