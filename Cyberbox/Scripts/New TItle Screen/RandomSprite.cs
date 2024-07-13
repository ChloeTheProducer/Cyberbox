using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RandomSprite : MonoBehaviour
{
    public Sprite[] Sprites;
    void Start()
    {
        GetComponent<Image>().sprite = Sprites[Random.Range(0, Sprites.Length)];
    }
}
