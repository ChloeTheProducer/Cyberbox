using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    NetworkTransform networkTransform;
    Transform spawnLocation;
    public Camera playerCamera;

    void Start()
    {
        networkTransform = GetComponent<NetworkTransform>();
        spawnLocation = GameObject.FindGameObjectWithTag("SpawnLocation").transform;
        if (!IsLocalPlayer) playerCamera.gameObject.SetActive(false); // Disable cameras on other players so it can't be seen by the client
        if (IsOwner) // Set the spawn location
        {
            this.transform.position = spawnLocation.position;
        }
    }
}
