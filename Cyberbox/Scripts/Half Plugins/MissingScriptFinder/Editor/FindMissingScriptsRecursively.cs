using UnityEngine;
 using UnityEditor;
 public class FindMissingScriptsRecursively : EditorWindow {
     static int go_count = 0, components_count = 0, missing_count = 0;
 
     [MenuItem("Window/FindMissingScriptsRecursively")]
     public static void ShowWindow() {
         EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
     }
 
     public void OnGUI() {
 
         GUILayout.Label("1) Select ALL GameObject in the Hierarchy.");
         GUILayout.Label("2) Click Button [Find Missing Scripts...]");
         
         if (GUILayout.Button("Find Missing Scripts in selected GameObjects")) {
             FindInSelected();
         }
         GUILayout.Label("3) Read the result in Console.");
         GUILayout.Label("4) Open this script with Mono or VS.");
         GUILayout.Label("5) Select the GameObject where missing scripts occur.");
         GUILayout.Label("6) Edit and Undo any line in FindMissingScriptsRecursively script and save the script");
         GUILayout.Label("7) Click back onto the Unity Editor and let recompile the FindMissingScriptsRecursively script.");
         GUILayout.Label("8) Now the missing component on the gameobject is now visible for deteltion.");
         GUILayout.Label("9) Repeat this procedure for each missing script/component.");
     }
     private static void FindInSelected() {
         GameObject[] go = Selection.gameObjects;
         go_count = 0;
         components_count = 0;
         missing_count = 0;
         foreach (GameObject g in go) {
             FindInGO(g);
         }
         Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
     }
 
     private static void FindInGO(GameObject g) {
         go_count++;
         Component[] components = g.GetComponents<Component>();
         for (int i = 0; i < components.Length; i++) {
             components_count++;
             if (components[i] == null) {
             missing_count++;
                 string s = g.name;
                 Transform t = g.transform;
                 while (t.parent != null) {
                     s = t.parent.name + "/" + s;
                     t = t.parent;
                 }
                 Debug.Log(s + " has an empty script attached in position: " + i, g);
             }
         }
         // Now recurse through each child GO (if there are any):
         foreach (Transform childT in g.transform) {
             //Debug.Log("Searching " + childT.name  + " " );
             FindInGO(childT.gameObject);
         }
     }
 }
