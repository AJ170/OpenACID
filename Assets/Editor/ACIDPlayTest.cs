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
//       [-acidPlayTap] -logFile Logs/playtest_editor.log
// -acidPlayTap: once the target scene is loaded, "tap" the loading screen's Tap to Start (starts the mission) and take
// the screenshot after that.
// -acidPlayStay: keep playing after the screenshot (for someone to play; the log keeps going) until the timeout or
// the editor is closed.
// Results: Logs/playtest.txt (timeline + messages), Logs/playtest.png.
[InitializeOnLoad]
public static class ACIDPlayTest
{
    private const string ActiveKey = "ACIDPlayTest.Active";
    private const string StartKey = "ACIDPlayTest.Start";
    private const string TargetKey = "ACIDPlayTest.Target";
    private const string TimeoutKey = "ACIDPlayTest.Timeout";
    private const string TapKey = "ACIDPlayTest.Tap";
    private const string StayKey = "ACIDPlayTest.Stay";
    private const string ReportPath = "Logs/playtest.txt";
    private const string ShotPath = "Logs/playtest.png";
    private const float SettleSeconds = 15f;

    private static float s_targetSeenAt = -1f;
    private static bool s_shotTaken;
    private static string s_lastScenes = string.Empty;
    private static bool s_tapped;
    private static double s_lastRendererCheck = -10.0;
    private static readonly System.Collections.Generic.HashSet<int> s_brokenRenderers = new System.Collections.Generic.HashSet<int>();
    private static readonly System.Collections.Generic.Dictionary<int, string> s_animStates = new System.Collections.Generic.Dictionary<int, string>();

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
        SessionState.SetBool(TapKey, Array.IndexOf(args, "-acidPlayTap") >= 0);
        SessionState.SetBool(StayKey, Array.IndexOf(args, "-acidPlayStay") >= 0);
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path, OpenSceneMode.Single);
        EditorApplication.isPlaying = true;
    }

    // Every character animator's current state per layer (full path hash, map back with the controller's
    // *_statemachine.bytes table), logged when it changes: shows where a character gets stuck.
    private static void LogCharacterAnimators()
    {
        foreach (Animator animator in UnityEngine.Object.FindObjectsOfType<Animator>())
        {
            if (!animator.name.Contains("Character_") || animator.runtimeAnimatorController == null)
            {
                continue;
            }
            StringBuilder sb = new StringBuilder();
            for (int layer = 0; layer < animator.layerCount; layer++)
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
                sb.AppendFormat("L{0}={1}{2} ", layer, info.fullPathHash, animator.IsInTransition(layer) ? "*" : string.Empty);
            }
            string now = sb.ToString();
            string before;
            int id = animator.GetInstanceID();
            if (!s_animStates.TryGetValue(id, out before) || before != now)
            {
                s_animStates[id] = now;
                Write("anim " + animator.name + " [" + animator.runtimeAnimatorController.name + "] " + now);
            }
        }
    }

    // Renderers with non-finite bounds (the engine's IsFinite(outDistanceForSort) / Invalid AABB assertions name
    // no object), each logged once with its hierarchy path and position. Checked every 2 s.
    private static void LogBrokenRenderers()
    {
        if (Elapsed() - s_lastRendererCheck < 2.0)
        {
            return;
        }
        s_lastRendererCheck = Elapsed();
        foreach (Renderer renderer in UnityEngine.Object.FindObjectsOfType<Renderer>())
        {
            Vector3 c = renderer.bounds.center;
            Vector3 e = renderer.bounds.extents;
            float sum = c.x + c.y + c.z + e.x + e.y + e.z;
            if ((float.IsNaN(sum) || float.IsInfinity(sum)) && s_brokenRenderers.Add(renderer.GetInstanceID()))
            {
                string path = renderer.name;
                for (Transform t = renderer.transform.parent; t != null; t = t.parent)
                {
                    path = t.name + "/" + path;
                }
                Write(string.Format("broken renderer {0} ({1}) position {2} bounds {3}", path, renderer.GetType().Name, renderer.transform.position, renderer.bounds));
            }
        }
    }

    // UILoadingScreen.OnTapScreen once it waits for the tap (what touching the screen does).
    private static bool TryTapLoadingScreen()
    {
        Type type = Type.GetType("Assets.Scripts.Mission.UI.UILoadingScreen, Assembly-CSharp");
        object screen = (type == null) ? null : type.GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).GetValue(null, null);
        if (screen == null)
        {
            return false;
        }
        object state = type.GetProperty("State", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(screen, null);
        if (state == null || state.ToString() != "WaitForTap")
        {
            return false;
        }
        type.GetMethod("OnTapScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(screen, null);
        return true;
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
        if (s_targetSeenAt >= 0f)
        {
            LogCharacterAnimators();
            LogBrokenRenderers();
        }
        if (targetLoaded && s_targetSeenAt < 0f)
        {
            s_targetSeenAt = (float)Elapsed();
            Write("target scene loaded");
        }
        if (s_targetSeenAt >= 0f && SessionState.GetBool(TapKey, false) && !s_tapped)
        {
            if (TryTapLoadingScreen())
            {
                s_tapped = true;
                s_targetSeenAt = (float)Elapsed();
                Write("tapped the loading screen");
            }
            return;
        }
        if (s_targetSeenAt >= 0f && !s_shotTaken && Elapsed() - s_targetSeenAt > SettleSeconds)
        {
            ScreenCapture.CaptureScreenshot(ShotPath);
            s_shotTaken = true;
            Write("screenshot " + ShotPath);
        }
        bool done = s_shotTaken && Elapsed() - s_targetSeenAt > SettleSeconds + 3f && !SessionState.GetBool(StayKey, false);
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
