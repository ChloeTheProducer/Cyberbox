using Cysharp.Threading.Tasks;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

[System.Serializable]
public class rshwFormat
{
    public byte[] audioData { get; set; }
    public int[] signalData { get; set; }
    public byte[] videoData { get; set; }

    public string name { get; set; }
    public string description { get; set; }
    public string creator { get; set; }


    public void Save(string filePath)
    {
        var formatter = new BinaryFormatter();
        using (var stream = File.Open(filePath, FileMode.Create))
            formatter.Serialize(stream, this);
    }
    public static async UniTask<rshwFormat> ReadFromFile(string filepath)
    {
        var formatter = new BinaryFormatter();
        using (var stream = File.OpenRead(filepath))
        {
            if (stream.Length != 0)
            {
                stream.Position = 0;
                try
                {
                    return await Task.Run(() => (rshwFormat)formatter.Deserialize(stream));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}