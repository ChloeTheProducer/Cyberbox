using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName = "";
    public void BeginScene()
    {
        try
        {
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Single);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            Application.Quit();
        }
    }
}
