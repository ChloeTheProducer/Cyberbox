using System.Diagnostics;
using UnityEngine;

public class UserManual : MonoBehaviour
{
    private void OpenPDF()
    {
        Application.OpenURL(System.Environment.CurrentDirectory + "/User Manual.pdf");
    }
}
