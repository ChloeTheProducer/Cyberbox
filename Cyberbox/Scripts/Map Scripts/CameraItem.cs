using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraItem : MonoBehaviour
{
    public Texture2D image;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Item: Camera") == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
