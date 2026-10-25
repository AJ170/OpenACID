using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// PS Vita player settings and build entry points.
// Batch mode: Unity.exe -batchmode -quit -projectPath <path> -buildTarget PSP2 -executeMethod ACIDVitaBuild.BuildBatch
public static class ACIDVitaBuild
{
    private const string TitleId = "ACID00001";
    private const string OutputDir = "Build/PSVita";

    [MenuItem("AC Identity/Apply PS Vita Settings")]
    public static void ApplySettings()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.PSP2, ScriptingImplementation.IL2CPP);
        PlayerSettings.PSVita.shortTitle = "AC Identity";
        PlayerSettings.PSVita.contentID = "UP0000-" + TitleId + "_00-0000000000000000";
        PlayerSettings.PSVita.appVersion = "01.00";
        PlayerSettings.PSVita.masterVersion = "01.00";
        PlayerSettings.PSVita.category = PlayerSettings.PSVita.PSVitaAppCategory.Application;
        PlayerSettings.PSVita.saveDataQuota = 10240;
        // The game targeted 1 GB phones; take every extra MB the Vita offers.
        PlayerSettings.PSVita.memoryExpansionMode = PlayerSettings.PSVita.PSVitaMemoryExpansionMode.ExpandBy109MB;
        // Mode C raises the GPU clock and keeps Wi-Fi (the game still talks to its servers).
        PlayerSettings.PSVita.powerMode = PlayerSettings.PSVita.PSVitaPowerMode.ModeC;
        PlayerSettings.PSVita.acquireBGM = true;
        PlayerSettings.colorSpace = ColorSpace.Gamma;
        Debug.Log("ACIDVitaBuild: PS Vita player settings applied");
    }

    [MenuItem("AC Identity/Build PS Vita")]
    public static void Build()
    {
        string error = BuildInternal(BuildOptions.None);
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError("ACIDVitaBuild: build failed: " + error);
        }
    }

    public static void BuildBatch()
    {
        bool development = Environment.GetCommandLineArgs().Contains("-acidDevelopment");
        string error = BuildInternal(development ? BuildOptions.Development | BuildOptions.AllowDebugging : BuildOptions.None);
        EditorApplication.Exit(string.IsNullOrEmpty(error) ? 0 : 1);
    }

    private static string BuildInternal(BuildOptions options)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.PSP2)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.PSP2, BuildTarget.PSP2);
        }
        ApplySettings();
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        Directory.CreateDirectory(OutputDir);
        Debug.Log("ACIDVitaBuild: building " + scenes.Length + " scenes to " + OutputDir + " (" + options + ")");
        string error = BuildPipeline.BuildPlayer(scenes, OutputDir, BuildTarget.PSP2, options);
        Debug.Log(string.IsNullOrEmpty(error) ? "ACIDVitaBuild: build succeeded" : "ACIDVitaBuild: build failed: " + error);
        return error;
    }
}
