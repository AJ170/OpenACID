using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Batch helpers used while porting. Example:
// Unity.exe -batchmode -quit -projectPath <path> -executeMethod ACIDDiagnostics.OpenBuildScenes
public static class ACIDDiagnostics
{
    // Opens every build scene once so load-time errors can be attributed to a scene.
    public static void OpenBuildScenes()
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
            {
                continue;
            }
            Debug.Log("ACIDDiagnostics: opening " + scene.path);
            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
            Debug.Log("ACIDDiagnostics: opened " + scene.path);
        }
    }

    // Loads every asset on its own so load-time errors land right after the culprit's log line.
    public static void LoadAllAssets()
    {
        foreach (string path in AssetDatabase.GetAllAssetPaths())
        {
            if (!path.StartsWith("Assets/") || path.EndsWith(".cs") || path.EndsWith(".unity") || AssetDatabase.IsValidFolder(path))
            {
                continue;
            }
            Debug.Log("ACIDDiagnostics: loading " + path);
            AssetDatabase.LoadAllAssetsAtPath(path);
            if (System.Environment.GetCommandLineArgs().Contains("-acidUnloadEach"))
            {
                EditorUtility.UnloadUnusedAssetsImmediate();
            }
        }
        EditorUtility.UnloadUnusedAssetsImmediate();
        Debug.Log("ACIDDiagnostics: all assets loaded");
    }
}
