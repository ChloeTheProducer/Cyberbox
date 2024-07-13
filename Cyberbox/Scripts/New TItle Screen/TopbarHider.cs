using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopbarHider : MonoBehaviour
{
    public GameObject[] objectsToCheck;

    private void Update()
    {
        bool anyActive = false;

        // Check if any object in the array is active
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.activeSelf)
            {
                anyActive = true;
                break;
            }
        }

        // Enable or disable the attached GameObject based on the result
        if (!anyActive)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
