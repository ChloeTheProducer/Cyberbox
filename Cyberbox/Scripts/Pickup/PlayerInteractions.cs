using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public LayerMask layers;
    private Vector3 raycastPos;
    public GameObject lookObject;
    private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject pickupParentLook;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;
    public bool freeze;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;
    public Vector2 holdingRotation;
    public LayerMask pickupLayerMask;
    RaycastHit hit;
    Vector3 fixedRot;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hit.point, sphereCastRadius);
    }

    //Interactable Object detections and distance check
    void Update()
    {
        if (mainCamera != null)
        {
            //Here we check if we're currently looking at an interactable object
            raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, layers))
            {
                lookObject = hit.collider.transform.root.gameObject;
                if (lookObject.layer != 9 && lookObject.transform.childCount > 0)
                {
                    lookObject = lookObject.transform.GetChild(0).gameObject;
                }
            }
            else
            {
                lookObject = null;
            }
        }


    }

    //Velocity movement toward pickup parent and rotation
    private void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            //Rotation
            //lookRot = Quaternion.LookRotation(pickupParentLook.transform.position - pickupRB.position);
            //lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
            lookRot = Quaternion.Euler(fixedRot);
            pickupRB.MoveRotation(lookRot);
        }

    }
}