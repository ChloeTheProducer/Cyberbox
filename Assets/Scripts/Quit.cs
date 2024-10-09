using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    public void QuitGame()
    {
        // This will only work in a built game, not in the editor.
        Application.Quit();

        // If you're testing in the editor, you can uncomment the next line
        // to simulate quitting.
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
