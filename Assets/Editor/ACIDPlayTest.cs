using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Unattended play-through of the startup: opens the first build scene, enters Play mode, records every loaded scene
// and every warning / error / exception, takes a screenshot once the target scene has been loaded for a while, and
// quits the editor. Play mode needs the normal editor, not -batchmode:
//   Unity.exe -projectPath <path> -executeMethod ACIDPlayTest.Begin [-acidPlayTarget AnimusGlobe] [-acidPlayTimeout 900]
//       -logFile Logs/playtest_editor.log
// Results: Logs/playtest.txt (timeline + messages), Logs/playtest.png.
[InitializeOnLoad]
public static class ACIDPlayTest
{
    private const string ActiveKey = "ACIDPlayTest.Active";
    private const string StartKey = "ACIDPlayTest.Start";
    private const string TargetKey = "ACIDPlayTest.Target";
    private const string TimeoutKey = "ACIDPlayTest.Timeout";
    private const string ReportPath = "Logs/playtest.txt";
    private const string ShotPath = "Logs/playtest.png";
    private const float SettleSeconds = 15f;

    private static float s_targetSeenAt = -1f;
    private static bool s_shotTaken;
    private static string s_lastScenes = string.Empty;

    static ACIDPlayTest()
    {
        if (SessionState.GetBool(ActiveKey, false))
        {
            Application.logMessageReceived += OnLog;
            EditorApplication.update += Update;
        }
    }

    public static void Begin()
    {
        string[] args = Environment.GetCommandLineArgs();
        string target = Arg(args, "-acidPlayTarget") ?? "AnimusGlobe";
        int timeout = int.Parse(Arg(args, "-acidPlayTimeout") ?? "900");
        Directory.CreateDirectory("Logs");
        File.WriteAllText(ReportPath, string.Format("[{0:HH:mm:ss}] play test, target {1}, timeout {2}s\n", DateTime.Now, target, timeout));
        if (File.Exists(ShotPath))
        {
            File.Delete(ShotPath);
        }
        SessionState.SetBool(ActiveKey, true);
        SessionState.SetString(StartKey, DateTime.Now.Ticks.ToString());
        SessionState.SetString(TargetKey, target);
        SessionState.SetInt(TimeoutKey, timeout);
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path, OpenSceneMode.Single);
        EditorApplication.isPlaying = true;
    }

    private static string Arg(string[] _args, string _name)
    {
        int i = Array.IndexOf(_args, _name);
        return (i >= 0 && i + 1 < _args.Length) ? _args[i + 1] : null;
    }

    private static double Elapsed()
    {
        long start;
        long.TryParse(SessionState.GetString(StartKey, "0"), out start);
        return (DateTime.Now - new DateTime(start)).TotalSeconds;
    }

    private static void Write(string _line)
    {
        File.AppendAllText(ReportPath, string.Format("[{0,6:F1}s] {1}\n", Elapsed(), _line));
    }

    private static void OnLog(string _message, string _stackTrace, LogType _type)
    {
        if (_type == LogType.Log)
        {
            Write("log: " + _message);
            return;
        }
        string trace = string.IsNullOrEmpty(_stackTrace) ? string.Empty : "\n        " + _stackTrace.Trim().Replace("\n", "\n        ");
        Write(_type.ToString().ToUpperInvariant() + ": " + _message + trace);
    }

    private static void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }
        StringBuilder scenes = new StringBuilder();
        bool targetLoaded = false;
        string target = SessionState.GetString(TargetKey, "AnimusGlobe");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            scenes.Append(scene.name).Append(scene.isLoaded ? " " : "(loading) ");
            if (scene.isLoaded && scene.name == target)
            {
                targetLoaded = true;
            }
        }
        string now = scenes.ToString().Trim();
        if (now != s_lastScenes)
        {
            s_lastScenes = now;
            Write("scenes: " + now);
        }
        if (targetLoaded && s_targetSeenAt < 0f)
        {
            s_targetSeenAt = (float)Elapsed();
            Write("target scene loaded");
        }
        if (s_targetSeenAt >= 0f && !s_shotTaken && Elapsed() - s_targetSeenAt > SettleSeconds)
        {
            ScreenCapture.CaptureScreenshot(ShotPath);
            s_shotTaken = true;
            Write("screenshot " + ShotPath);
        }
        bool done = s_shotTaken && Elapsed() - s_targetSeenAt > SettleSeconds + 3f;
        if (done || Elapsed() > SessionState.GetInt(TimeoutKey, 900))
        {
            Write(done ? "done" : "TIMEOUT");
            SessionState.SetBool(ActiveKey, false);
            EditorApplication.update -= Update;
            Application.logMessageReceived -= OnLog;
            EditorApplication.Exit(done ? 0 : 1);
        }
    }
}
