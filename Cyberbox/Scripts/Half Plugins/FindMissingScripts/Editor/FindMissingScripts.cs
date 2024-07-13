using UnityEngine;
using UnityEditor;
using System.Linq;

public static class FindMissingScripts
{
    [MenuItem("Window/Find Missing Scripts/Global")]
    static void DeleteMissingScriptsInProject()
    {
        string[] prefabs = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();
        foreach (string path in prefabs)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            foreach(Component component in prefab.GetComponentsInChildren<Component>())
                if (component != null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(component.gameObject);
                    Debug.Log("Deleted missing scripts from " + component.gameObject.name);
                    break;
                }
        }
    }

    [MenuItem("Window/Find Missing Scripts/Scene")]
    static void DeleteMissingScriptsInScene()
    {
        foreach(GameObject gameObject in GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            foreach (Component component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                    Debug.Log("Deleted missing scripts from " + gameObject.name);

                    break;
                }
            }
        }
    }

}