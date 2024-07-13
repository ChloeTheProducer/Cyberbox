using UnityEngine;

public class Mack_Valves : MonoBehaviour
{
    public float PSI;
    [HideInInspector] public bool Active;
    public bool[] topDrawer = new bool[95];
    public bool[] bottomDrawer = new bool[97];

    [HideInInspector] public DynamicBone[] bones;
    [HideInInspector] public Animator[] animators;
    [HideInInspector] public LightController[] lights;


    void Start()
    {
        PSI /= 2;
        Disable();
    }


    /// <summary>
    /// Refreshes the DynamicBone and Animator variables
    /// </summary>
    public void RefreshVariables()
    {
        GameObject chars = GameObject.Find("Characters");
        UI_PlayRecord playRecord = GameObject.Find("UI").GetComponent<UI_PlayRecord>();
        bones = chars.GetComponentsInChildren<DynamicBone>();
        animators = chars.GetComponentsInChildren<Animator>();
        lights = GameObject.Find("Stages").gameObject.transform.GetChild(playRecord.currentStage).GetComponentsInChildren<LightController>();

        Debug.Log(bones.Length + " bones found, " + animators.Length + " animators found, " + lights.Length + " lights found.");
    }

    /// <summary>
    /// Disable Dynamic Bones and Animators
    /// </summary>
    public void Disable()
    {
        Active = false;

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].enabled = false;
        }
        for (int i = 0;i < animators.Length; i++)
        {
            animators[i].enabled = false;
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = false;
        }
    }

    /// <summary>
    /// Enable Dynamic Bones and Animators
    /// </summary>
    public void Enable()
    {
        Active = true;

        for (int i = 0; i < bones.Length;i++)
        {
            bones[i].enabled = true;
        }
        for (int i = 0; i<animators.Length; i++)
        {
            animators[i].enabled = false;
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
        }
    }

}
