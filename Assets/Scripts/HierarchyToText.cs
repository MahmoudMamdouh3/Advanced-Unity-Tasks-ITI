using UnityEngine;
using UnityEditor;
using System.Text;

public class HierarchyToText : EditorWindow
{
    [MenuItem("Tools/ITI/Hierarchy To Tree Text")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyToText>("Hierarchy To Tree");
    }

    private GameObject targetObject;
    private string generatedTree = "";
    private Vector2 scrollPos;

    void OnGUI()
    {
        GUILayout.Label("Select Robot Kyle to Generate Tree", EditorStyles.boldLabel);
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Generate Tree Structure"))
        {
            if (targetObject != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(targetObject.name);
                GenerateTree(targetObject.transform, "", sb);
                generatedTree = sb.ToString();
            }
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));
        EditorGUILayout.TextArea(generatedTree, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Copy to Clipboard"))
        {
            GUIUtility.systemCopyBuffer = generatedTree;
            Debug.Log("Tree copied to clipboard! Paste it in the chat.");
        }
    }

    void GenerateTree(Transform parent, string indent, StringBuilder sb)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            bool isLast = (i == parent.childCount - 1);
            
            sb.Append(indent);
            sb.Append(isLast ? "└── " : "├── ");
            sb.AppendLine(child.name);

            GenerateTree(child, indent + (isLast ? "    " : "│   "), sb);
        }
    }
}