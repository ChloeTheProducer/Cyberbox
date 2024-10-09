using System.Diagnostics;
using UnityEngine;

public class UserManual : MonoBehaviour
{
    // Set the path to your PDF file here
    public string pdfFilePath = "/home/chloe/User Manual.pdf";

    // This method will be triggered when the button is clicked
    public void OpenPDF()
    {
        if (!string.IsNullOrEmpty(pdfFilePath))
        {
            // Open the PDF file with the default application
            Process.Start(new ProcessStartInfo(pdfFilePath) { UseShellExecute = true });
        }
        else
        {
            UnityEngine.Debug.Log("PDF file path is not set or is invalid.");
        }
    }
}
