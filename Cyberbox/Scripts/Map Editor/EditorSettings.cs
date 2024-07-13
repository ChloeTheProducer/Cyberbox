using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorSettings : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform viewport;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUIScale(float value)
    {
        canvas.scaleFactor = value;
    }

    public void RefreshViewportQuality()
    {
        RenderTexture renderTex = Camera.main.targetTexture;
        renderTex.Release();
        renderTex.width = (int)viewport.rect.size.x / (int)canvas.scaleFactor;
        renderTex.height = (int)viewport.rect.size.y / (int)canvas.scaleFactor;
    }
}
