using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowSystem : MonoBehaviour
{
    public GameObject stagesContainer;
    public GameObject mackValves;
    [HideInInspector] public List<GameObject> Stages;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        // Initialize the Stages list
        Stages = new List<GameObject>();
        foreach (Transform child in stagesContainer.transform)
        {
            Stages.Add(child.gameObject);
        }
        Stages = Stages.OrderBy(stage => stage.name).ToList();
    }

    void Update()
    {
        
    }
}