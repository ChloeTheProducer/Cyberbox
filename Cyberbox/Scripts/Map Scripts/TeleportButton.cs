using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportButton : MonoBehaviour
{
    public string Map;

    public GameObject Fade;

    public GameObject LocalEnd;



    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Teleport()
    {
        StartCoroutine(TeleportToScene());
    }

    public void TeleportLocal()
    {
        Player = GameObject.Find("Player");
        Player.transform.position = LocalEnd.transform.position;
    }

    IEnumerator TeleportToScene()
    {
        LeanTween.alphaCanvas(Fade.GetComponent<CanvasGroup>(), 1, 2);
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(Map);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
