using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetter : MonoBehaviour

{
    public int mapping;


    // Start
    void OnEnable()
    {
        // if this doesn't work i will be very pissed
        InputHandler inputHandler = FindFirstObjectByType<InputHandler>();
        if (inputHandler != null) {
            inputHandler.valveMapping = mapping;
        }
    }

}