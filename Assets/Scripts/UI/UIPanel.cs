using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public void Close(GameObject obj)
    {
         Destroy(obj);
    }
}
