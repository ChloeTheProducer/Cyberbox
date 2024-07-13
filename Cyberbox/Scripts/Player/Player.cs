using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Initial")]
    //Attatched Objects
    public Camera PlayerCamera;

    //Speeds and Base attributes

    [Header("Speed")]
    public float baseSpeed = 2f;
    public float sprintSpeed = 2.5f;

    [Header("Jump")]
    public bool enableJump;
    public float gravity = 12f;
    public float jumpSpeed = 9f;
    public float airControl = 1;
    public float airTurnSpeed = 1;

    [Header("WorldSpace UI")]
    public bool enableUIClick;
    public GameObject cursor;
    public TMP_Text cursorText;
    public LayerMask uiLayerMask;
    public LayerMask uiCueLayerMask;
    public LayerMask playerLayerMask;

    [Header("Flashlight")]
    public bool enableFlashlight;
    public GameObject flashlight;
    public int flashState;

    [Header("CameraZoom")]
    public float maxFov = 120;
    public float minFov = 20;

    [Header("CameraSmooth")]
    public bool enableCamSmooth;
    public float smoothSpeed;
    public float maxVelocity;

    [Header("CameraMode")]
    public bool CinematicMode;

    [Header("PlayerState")]
    public PlayerState playerState;

    public enum PlayerState
    {
        normal,
        frozenBody,
        frozenCam,
        frozenAll,
        frozenCamUnlock,
        frozenAllUnlock,
    }

    public CameraState cameraState;

    public enum CameraState
    {
        normal,
        flyCamera
    }


    public GameObject pauseMenu;
    public bool canPause = true;

    float flashsmoothScroll = 63;
    [HideInInspector]
    public Vector2 holdingRotation;

    //New Input
    [SerializeField]
    bool fixedUpdatelowerFPS;

    [Header("User Interface")]
    public Texture2D CursorDefault;
    public GameObject[] functionPanels;
    public TitleManager titleManager;

    GameObject camerasObject;


    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Awake()
    {
        Cursor.SetCursor(CursorDefault, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        
        DisableChildCamerasExceptFirst();
        camerasObject = GameObject.Find("Cameras");
        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;
    }

    private void FixedUpdate()
    {
        fixedUpdatelowerFPS = !fixedUpdatelowerFPS;

        if (fixedUpdatelowerFPS)
        {
            //UI Click
            if (enableUIClick)
            {

                RayCastClick();
            }
        }
    }

    void Update()
    {

        FunctionMenusCheck();

        //Flashlight
        if (enableFlashlight)
        {
            FlashlightCheck();
        }


        // Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitToTitle();
        }

        if (Input.GetMouseButtonDown(1)) // 1 represents the right mouse button
        {
            // Toggle cursor lock state based on the current state
            switch (Cursor.lockState)
            {
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cameraState == CameraState.flyCamera) // disable
            {
                cameraState = CameraState.normal;
            }
            else // enable
            {
                cameraState = CameraState.flyCamera;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CinematicMode == true)
            {
                CinematicMode = false;
            }
            else
            {
                CinematicMode = true;
            }
        }

        if (PlayerCamera != null)
        {
            if (CinematicMode)
            {
                // Find the "Cameras" GameObject
                if (camerasObject != null)
                {
                    PlayerCamera.enabled = false;
                    // Activate camera based on key press
                    for (int i = 0; i < camerasObject.transform.childCount; i++)
                    {
                        Camera camera = camerasObject.transform.GetChild(i).GetComponent<Camera>();

                        if (Input.GetKeyDown((i + 1).ToString()))
                        {
                            // Disable all cameras first
                            foreach (Transform child in camerasObject.transform)
                            {
                                child.GetComponent<Camera>().enabled = false;
                            }

                            // Enable the selected camera
                            camera.enabled = true;
                        }
                    }
                }
            }
            else
            {
                PlayerCamera.enabled = true;

                // Find the "Cameras" GameObject
                if (camerasObject != null)
                {
                    // Disable all child GameObjects
                    foreach (Transform child in camerasObject.transform)
                    {
                        child.GetComponent<Camera>().enabled = false;
                    }
                }
            }
        }
    }

    public void DisableChildCamerasExceptFirst()
    {
        // Find the "Cameras" GameObject
        if (camerasObject != null)
        {
            for (int i = 0; i < camerasObject.transform.childCount; i++)
            {
                Camera camera = camerasObject.transform.GetChild(i).GetComponent<Camera>();
                camera.enabled = i == 0;
            }
        }
    }

    public void FunctionMenusCheck()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            titleManager.SwitchMenuPanel(functionPanels[0]);
        }
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void QuitToTitle()
    {
        SceneManager.LoadScene("Title Screen", LoadSceneMode.Single);
    }

    void RayCastClick()
    {
        cursor.SetActive(false);
        RaycastHit hit;
        if (CinematicMode != true)
        {
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, 10f, uiLayerMask))
            {
                Button3D hitcol = hit.collider.GetComponent<Button3D>();
                if (hitcol != null)
                {
                    cursor.SetActive(true);
                    cursorText.text = hitcol.buttonText;
                    hitcol.Highlight(gameObject.name);

                    if (Input.GetMouseButton(0))
                    {
                        hitcol.StartClick(gameObject.name);
                    }
                    else
                    {
                        hitcol.EndClick(gameObject.name);
                    }
                }
            }
        }
    }

    void FlashlightCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) || flashState == 1)
        {
            flashlight.SetActive(!flashlight.activeSelf);
            if (flashlight.activeSelf)
            {
                AudioSource sc = GameObject.Find("GlobalAudio").GetComponent<AudioSource>(); Resources.Load("ting");
                sc.clip = (AudioClip)Resources.Load("Flashlight On");
                sc.Play();
            }
            else
            {
                AudioSource sc = GameObject.Find("GlobalAudio").GetComponent<AudioSource>(); Resources.Load("ting");
                sc.clip = (AudioClip)Resources.Load("Flashlight Off");
                sc.Play();
            }
        }
    }
}