using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Reflection;

public class ProjectDumper : EditorWindow
{
    [MenuItem("Tools/DUMP EVERYTHING")]
    public static void DumpProject()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== PROJECT HIERARCHY & INSPECTOR DATA ===");

        // 1. DUMP HIERARCHY & COMPONENTS
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject go in allObjects)
        {
            if (go.transform.parent == null) // Start from roots
                DumpGameObject(go, sb, 0);
        }

        sb.AppendLine("\n=== PROJECT FILES ===");
        string[] allFiles = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        foreach (string file in allFiles) sb.AppendLine(file.Replace(Application.dataPath, "Assets"));

        string path = Application.dataPath + "/PROJECT_DUMP.txt";
        File.WriteAllText(path, sb.ToString());
        Debug.Log("DUMP COMPLETE: " + path);
        EditorUtility.RevealInFinder(path);
    }

    private static void DumpGameObject(GameObject go, StringBuilder sb, int indent)
    {
        string space = new string('-', indent * 2);
        sb.AppendLine($"{space}GameObject: {go.name} (Active: {go.activeSelf})");

        foreach (Component comp in go.GetComponents<Component>())
        {
            if (comp == null) continue;
            sb.AppendLine($"{space}  [Component: {comp.GetType().Name}]");

            // Use reflection to see public variables (The Inspector data)
            FieldInfo[] fields = comp.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var val = field.GetValue(comp);
                sb.AppendLine($"{space}    - {field.Name}: {val}");
            }
        }

        foreach (Transform child in go.transform) DumpGameObject(child.gameObject, sb, indent + 1);
    }
}