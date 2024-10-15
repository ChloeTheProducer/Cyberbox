using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class Interact3D : MonoBehaviour
{
    [Header("Event")]
    public GameObject targetObject;

    [FunctionDropdown("targetObject")]
    public string functionToCall;

    [Header("Sound")]
    public AudioClip buttonDown;
    public AudioClip buttonUp;

    [Header("Animation")]
    public ClickAnimation clickAnim;

    [Header("UI")]
    public int cursor;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the mouse cursor enters the collider attached to this GameObject
    void OnMouseEnter()
    {
        player.SetCursor(cursor);
    }

    // Called when the mouse cursor clicks down on the collider
    void OnMouseDown()
    {
        if (buttonDown)
        {
            player.audioSource.PlayOneShot(buttonDown);
        }

        switch (clickAnim)
        {
            case ClickAnimation.button1:
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, -0.1f, gameObject.transform.localPosition.z);
                break;
        }

        if (targetObject != null && !string.IsNullOrEmpty(functionToCall))
        {
            // Parse the functionToCall string to get the script name and method name
            string[] parts = functionToCall.Split('.');
            if (parts.Length == 2)
            {
                string scriptName = parts[0];
                string methodName = parts[1];

                // Find the script component
                MonoBehaviour script = targetObject.GetComponent(scriptName) as MonoBehaviour;
                if (script != null)
                {
                    // Get the method info
                    MethodInfo method = script.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (method != null)
                    {
                        // Invoke the method
                        method.Invoke(script, null);
                    }
                    else
                    {
                        Debug.LogWarning($"Method {methodName} not found on {scriptName}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Script {scriptName} not found on {targetObject.name}.");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid functionToCall format: {functionToCall}. Expected format: 'ScriptName.MethodName'.");
            }
        }
    }

    // Called when the mouse cursor clicks up on the collider
    private void OnMouseUp()
    {
        if (buttonUp)
        {
            player.audioSource.PlayOneShot(buttonUp);
        }

        switch (clickAnim)
        {
            case ClickAnimation.button1:
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.1f, gameObject.transform.localPosition.z);
                break;
        }
    }

    // Called when the mouse cursor exits the collider attached to this GameObject
    void OnMouseExit()
    {
        player.SetCursor(0);
    }
}

public enum ClickAnimation
{
    none,
    button1,
}

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class FunctionDropdownAttribute : PropertyAttribute
{
    public string TargetObjectName { get; private set; }

    public FunctionDropdownAttribute(string targetObjectName)
    {
        TargetObjectName = targetObjectName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FunctionDropdownAttribute))]
public class FunctionDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FunctionDropdownAttribute dropdownAttribute = (FunctionDropdownAttribute)attribute;
        SerializedProperty targetObjectProperty = property.serializedObject.FindProperty(dropdownAttribute.TargetObjectName);
        GameObject targetObject = targetObjectProperty.objectReferenceValue as GameObject;

        if (targetObject != null)
        {
            // Get all methods from the targetObject's MonoBehaviours
            MonoBehaviour[] scripts = targetObject.GetComponents<MonoBehaviour>();
            var methods = scripts.SelectMany(script =>
                script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                      .Where(m => m.ReturnType == typeof(void) && m.GetParameters().Length == 0)
                      .Select(m => new { ScriptName = script.GetType().Name, MethodName = m.Name })
            ).ToArray();

            // Create the dropdown options in "ScriptName.MethodName" format
            string[] methodNames = methods.Select(m => $"{m.ScriptName}.{m.MethodName}").ToArray();

            // Find the currently selected method in the dropdown
            int selectedIndex = Array.IndexOf(methodNames, property.stringValue);
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, methodNames);

            // Update the property value with the selected method
            if (selectedIndex >= 0)
            {
                property.stringValue = methodNames[selectedIndex];
            }
        }
        else
        {
            // Display a message indicating that the targetObject needs to be set
            EditorGUI.LabelField(position, label.text, "Set Target Object to see functions");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty targetObjectProperty = property.serializedObject.FindProperty((attribute as FunctionDropdownAttribute).TargetObjectName);
        GameObject targetObject = targetObjectProperty.objectReferenceValue as GameObject;

        if (targetObject != null)
        {
            return EditorGUI.GetPropertyHeight(property); // Normal height
        }
        else
        {
            return EditorGUIUtility.singleLineHeight; // Height for the label when targetObject is null
        }
    }
}
#endif
