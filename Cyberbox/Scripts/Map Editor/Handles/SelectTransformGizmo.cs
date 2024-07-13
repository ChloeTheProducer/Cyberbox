//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RuntimeHandle;

public class SelectTransformGizmo : MonoBehaviour
{
    [HideInInspector] public Transform highlight;
    [HideInInspector] public Transform selection;
    private RaycastHit raycastHit;
    private RaycastHit raycastHitHandle;
    private GameObject runtimeTransformGameObj;
    [HideInInspector] public RuntimeTransformHandle runtimeTransformHandle;
    private int runtimeTransformLayer = 6;
    private int runtimeTransformLayerMask;

    public Vector3 posSnap;
    public Vector3 scaleSnap;
    public int rotSnap;


    private void Awake()
    {
        runtimeTransformGameObj = new GameObject();
        runtimeTransformHandle = runtimeTransformGameObj.AddComponent<RuntimeTransformHandle>();
        runtimeTransformGameObj.layer = runtimeTransformLayer;
        runtimeTransformLayerMask = 1 << runtimeTransformLayer; //Layer number represented by a single bit in the 32-bit integer using bit shift
        runtimeTransformHandle.type = HandleType.POSITION;
        runtimeTransformHandle.autoScale = true;
        runtimeTransformHandle.autoScaleFactor = 1.0f;
        runtimeTransformHandle.positionSnap = posSnap;
        runtimeTransformHandle.rotationSnap = rotSnap;
        runtimeTransformHandle.scaleSnap = scaleSnap;
        runtimeTransformHandle.snappingType = HandleSnappingType.ABSOLUTE;
        runtimeTransformGameObj.SetActive(false);
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {

            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            ApplyLayerToChildren(runtimeTransformGameObj);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (Physics.Raycast(ray, out raycastHitHandle, Mathf.Infinity, runtimeTransformLayerMask)) //Raycast towards runtime transform handle only
                {
                }
                else if (highlight)
                {
                    selection = raycastHit.transform;

                    runtimeTransformHandle.target = selection;
                    runtimeTransformGameObj.SetActive(true);

                    highlight = null;
                }
                else
                {
                    if (selection)
                    {
                        selection = null;

                        runtimeTransformGameObj.SetActive(false);
                    }
                }
            }
            else
            {
                if (selection)
                {
                    selection = null;

                    runtimeTransformGameObj.SetActive(false);
                }
            }
        }

        //Hot Keys for move, rotate, scale, local and Global/World transform
        if (runtimeTransformGameObj.activeSelf)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    runtimeTransformHandle.space = HandleSpace.WORLD;
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    runtimeTransformHandle.space = HandleSpace.LOCAL;
                }
            }
        }

    }


    private void ApplyLayerToChildren(GameObject parentGameObj)
    {
        foreach (Transform transform1 in parentGameObj.transform)
        {
            int layer = parentGameObj.layer;
            transform1.gameObject.layer = layer;
            foreach (Transform transform2 in transform1)
            {
                transform2.gameObject.layer = layer;
                foreach (Transform transform3 in transform2)
                {
                    transform3.gameObject.layer = layer;
                    foreach (Transform transform4 in transform3)
                    {
                        transform4.gameObject.layer = layer;
                        foreach (Transform transform5 in transform4)
                        {
                            transform5.gameObject.layer = layer;
                        }
                    }
                }
            }
        }
    }

}