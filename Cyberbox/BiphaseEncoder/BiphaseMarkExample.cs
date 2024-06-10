using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.ComponentModel;

// This is just an example script so i can either reference back or copy and paste into a seperate top drawer and bottom drawer script which will be two scripts mentioned/referenced in the RAEBiphase script
// so that when you click export, it'll run up the RAEBiphase script to ready everything like the top drawer bits and bottom drawer bits
// then those bits will be run through their respective script which will have this code for encoding, but will also have code before hand to verify bit integrity or something of that matter
// so they can properly be encoded
//
// basically process will be, "Decode top/bottom bits from show file (i.e. rshw/show) > send to/reference respective top/bottom drawer script > verify bits/bit integrity > Encode to biphase" at least thats the general idea
// most likely its not gonna be that easy

namespace BiphaseMarkEncoder

class BiphaseMarkEncoder : MonoBehaviour
{
    // Encode a binary message into Biphase Mark Code
    public static string Encode(string message)
    {
        StringBuilder encodedSignal = new StringBuilder();
        
        // Iterate through each bit of the message
        foreach (char bit in message)
        {
            // Encode each bit according to Biphase Mark Code rules
            if (bit == '0')
            {
                encodedSignal.Append("01"); // Bit '0' is encoded as '01'
            }
            else if (bit == '1')
            {
                encodedSignal.Append("10"); // Bit '1' is encoded as '10'
            }
            else
            {
                // Handle invalid input
                throw new ArgumentException("Input message contains non-binary characters.");
            }
        }

        return encodedSignal.ToString();
    }

    // Create a WAV file from the encoded signal
    public static void CreateWavFile(string fileName, string encodedSignal)
    {
        // Sampling frequency (48000 Hz)
        const int sampleRate = 48000;

        // Duration of each bit (in seconds)
        const double bitDuration = 0.01; // For example, 10 milliseconds per bit

        // Calculate the number of samples per bit
        int samplesPerBit = (int)(sampleRate * bitDuration);

        // Prepare the wave data
        byte[] waveData = new byte[encodedSignal.Length * samplesPerBit * 4]; // 4 bytes per sample (16-bit stereo)

        // Encode the signal into the wave data
        for (int i = 0; i < encodedSignal.Length; i++)
        {
            // Calculate the frequency for this bit
            double frequency = encodedSignal[i] == '0' ? 1000 : 2000; // Arbitrarily chosen frequencies (1 kHz and 2 kHz)

            // Generate the waveform for this bit
            for (int j = 0; j < samplesPerBit; j++)
            {
                double t = (double)(i * samplesPerBit + j) / sampleRate;
                short sample = (short)(Math.Sin(2 * Math.PI * frequency * t) * short.MaxValue);
                byte[] sampleBytes = BitConverter.GetBytes(sample);
                Array.Copy(sampleBytes, 0, waveData, (i * samplesPerBit + j) * 4, 2); // 2 bytes per sample (16-bit stereo)
                Array.Copy(sampleBytes, 0, waveData, (i * samplesPerBit + j) * 4 + 2, 2); // 2 channels (stereo)
            }
        }

        // Write the wave data to a file
        using (FileStream fs = new FileStream(fileName, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            // Write the WAV header
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(36 + waveData.Length); // File size - 8
            writer.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            writer.Write(16); // PCM format header size
            writer.Write((short)1); // PCM format
            writer.Write((short)1); // Number of channels (mono)
            writer.Write(sampleRate);
            writer.Write(sampleRate * 4); // Bytes per second
            writer.Write((short)4); // Block align (16-bit stereo)
            writer.Write((short)16); // Bits per sample (16-bit PCM)
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(waveData.Length); // Data size

            // Write the wave data
            writer.Write(waveData);
        }
    }

    static void Main()
    {
        string message = "Hello World, Am I Alive?!"; // Example input message
        string encodedSignal = Encode(message);
        
        Console.WriteLine("Original message: " + message);
        Console.WriteLine("Biphase Mark Code: " + encodedSignal);

        string fileName = @"/$HOME/encoded_signal.wav";
        CreateWavFile(fileName, encodedSignal);

        Console.WriteLine($"WAV file '{fileName}' generated successfully.");
    }
}

