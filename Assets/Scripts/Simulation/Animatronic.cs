using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Animatronic : MonoBehaviour
{
    private Animator animator;
    private MackValves valves;

    public string name;
    [TextArea]
    public string description;
    public MovementData[] movementData;

    void Awake()
    {
        animator = GetComponent<Animator>();
        valves = GameObject.FindGameObjectWithTag("Mack Valves").GetComponent<MackValves>();
    }

    void Start()
    {
        // because people are dumb
        if (movementData.Length != animator.parameterCount)
        {
            Debug.LogError($"{gameObject.name}'s Movement Data Length ({movementData.Length}) does not match the animator parameter count ({animator.parameterCount}). Ensure all arrays have matching lengths. Animatronic has been disabled.");
            enabled = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < movementData.Length; i++)
        {
            AnimatorControllerParameter parameter = animator.parameters[i];

            // Ensure the parameter is of type float
            if (parameter.type == AnimatorControllerParameterType.Float)
            {
                // Get the current value of the parameter
                float currentValue = animator.GetFloat(parameter.name);

                // Calculate the change based on Time.deltaTime and the individual speed
                float deltaChangeIn = movementData[i].flowIn * Time.deltaTime;
                float deltaChangeOut = movementData[i].flowOut * Time.deltaTime;

                // Update the parameter value based on the bit state
                if (valves.Bits[movementData[i].bit] == true) // Active Bit
                {
                    if (currentValue < 1f)
                    {
                        animator.SetFloat(parameter.name, Mathf.Clamp(currentValue + deltaChangeIn, 0f, 1f));
                    }
                }
                else // Inactive Bit
                {
                    if (currentValue > 0f)
                    {
                        animator.SetFloat(parameter.name, Mathf.Clamp(currentValue - deltaChangeOut, 0f, 1f));
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class MovementData
{
    public string name;
    public int bit;

    [Header("Air In (Active)")]
    [Range(0, 10)]
    public float flowIn;


    [Header("Air Out (Inactive)")]
    [Range(0, 10)]
    public float flowOut;

}
