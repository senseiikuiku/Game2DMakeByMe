#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts in Project")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FindMissingScripts), false, "Find Missing Scripts");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in Scene"))
        {
            FindInScene();
        }

        if (GUILayout.Button("Find Missing Scripts in Prefabs"))
        {
            FindInPrefabs();
        }
    }

    // Kiểm tra scene hiện tại
    private static void FindInScene()
    {
        GameObject[] objects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject go in objects)
        {
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning("Missing script in Scene: " + go.name, go);
                }
            }
        }

        Debug.Log("Đã quét xong Scene.");
    }

    // Kiểm tra tất cả prefab trong Project
    private static void FindInPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                Component[] components = prefab.GetComponentsInChildren<Component>(true);
                foreach (Component comp in components)
                {
                    if (comp == null)
                    {
                        Debug.LogWarning("Missing script in Prefab: " + prefab.name + " (Path: " + path + ")", prefab);
                    }
                }
            }
        }

        Debug.Log("Đã quét xong Prefabs.");
    }
}
#endif
