using System;
using System.IO;
using System.Text;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public SerialToWavEncoder encoder;
    public string message = "Hello, World! Am I Alive?!?!??!?!";
    public string filePath = @"/home/chloe/encoded_signal.wav";

    void Start()
    {
        encoder.EncodeAndSave(message, filePath);
    }
}
