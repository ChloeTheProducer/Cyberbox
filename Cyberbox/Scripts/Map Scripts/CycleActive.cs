using System.Collections;
using UnityEngine;

public class CycleActive : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float Duration;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        while (true) // Keep the cycling going indefinitely
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (i == currentIndex)
                {
                    gameObjects[i].SetActive(true);
                }
                else
                {
                    gameObjects[i].SetActive(false);
                }
            }

            currentIndex = (currentIndex + 1) % gameObjects.Length; // Cycle through the array

            yield return new WaitForSeconds(Duration);
        }
    }
}
