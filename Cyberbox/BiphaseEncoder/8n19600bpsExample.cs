using System;
using System.IO;
using System.Text;

namespace SerialEncoder { }

public class SerialToWavEncoder
{
    // Encode a binary message into 8-N-1 format
    public static string EncodeToSerial(string message)
    {
        StringBuilder encodedSignal = new StringBuilder();

        foreach (char character in message)
        {
            byte byteValue = (byte)character;
            string byteString = Convert.ToString(byteValue, 2).PadLeft(8, '0');

            // Start bit (0), Data bits (8 bits), Stop bit (1)
            encodedSignal.Append("0" + byteString + "1");
        }

        return encodedSignal.ToString();
    }

    // Create a WAV file from the encoded signal
    public static void CreateWavFile(string fileName, string encodedSignal)
    {
        // Sampling frequency (44100 Hz)
        const int sampleRate = 44100;
        const int baudRate = 9600;
        double bitDuration = 1.0 / baudRate;
        int samplesPerBit = (int)(sampleRate * bitDuration);

        // Prepare the wave data
        byte[] waveData = new byte[encodedSignal.Length * samplesPerBit * 2]; // 2 bytes per sample (16-bit mono)

        // Encode the signal into the wave data
        for (int i = 0; i < encodedSignal.Length; i++)
        {
            // Determine the amplitude for this bit
            short sample = encodedSignal[i] == '1' ? (short)short.MaxValue : (short)0;

            // Generate the waveform for this bit
            for (int j = 0; j < samplesPerBit; j++)
            {
                byte[] sampleBytes = BitConverter.GetBytes(sample);
                Array.Copy(sampleBytes, 0, waveData, (i * samplesPerBit + j) * 2, 2); // 2 bytes per sample (16-bit mono)
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
            writer.Write(sampleRate * 2); // Bytes per second
            writer.Write((short)2); // Block align (16-bit mono)
            writer.Write((short)16); // Bits per sample (16-bit PCM)
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(waveData.Length); // Data size

            // Write the wave data
            writer.Write(waveData);
        }
    }

    // Example method to be called from Unity
    public void EncodeAndSave(string message, string filePath)
    {
        string encodedSignal = EncodeToSerial(message);
        CreateWavFile(filePath, encodedSignal);
    }
}
