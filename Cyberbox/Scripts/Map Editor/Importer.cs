using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using UnityGLTF;
using System.IO;

public class Importer : MonoBehaviour
{
    public GameObject Objects;
    public Sprite meshIcon;
    MapCreator creator;

    private void Start()
    {
        creator = GetComponent<MapCreator>();
    }

    public void Import()
    {
        StartCoroutine(ImportFile());
    }

    IEnumerator ImportFile()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Models", ".gltf", ".fbx"));

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Select File to Import", "Import");

        if (FileBrowser.Success)
        {
            switch (Path.GetExtension(FileBrowser.Result[0]))
            {
                case ".gltf":
                case ".fbx":
                    GameObject importedModel = new GameObject();
                    importedModel.transform.SetParent(Objects.transform, true);
                    importedModel.transform.SetPositionAndRotation(new Vector3(0, 5, 0), new Quaternion(0, 0, 0, 0));
                    GLTFComponent gltf = importedModel.AddComponent<GLTFComponent>();
                    // gltf.shaderOverride = Shader.Find("HDRP/Lit");
                    gltf.GLTFUri = FileBrowser.Result[0];

                    HierarchyProperties hierProperties = importedModel.AddComponent<HierarchyProperties>();
                    hierProperties.Icon = meshIcon;
                    importedModel.transform.name = "Mesh";

                    // if (gltf.textures != null)
                    {
                        // Access textures list after it's initialized
                        // for (int i = 0; i < gltf.textures.Count; i++)
                        {
                            // gltf.textures[i].SetTexture(Shader.PropertyToID("Base Color"), gltf.textures[i].mainTexture);
                        }
                    }

                    Debug.Log("Successfully imported model! " + FileBrowser.Result[0]);
                    break;
                default:
                    Debug.LogError("Unsupported file format.");
                    break;
            }

            creator.RegenerateHierarchy();
        }
    }
}
