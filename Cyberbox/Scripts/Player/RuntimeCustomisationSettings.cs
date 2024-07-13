using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCustomisationSettings : MonoBehaviour
{
    public CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeHeight(float height)
    {
        characterController.height = height;
    }
}
