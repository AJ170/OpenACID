using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Offline;
using UnityEditor;
using UnityEngine;

// Rebuilds the game's downloaded content for the PS Vita: one bundle per imported bundle (Assets/BundleAssets, see
// Tools/BundleImport), named by LocalBundles.BundleKey, holding the assets of Assets/BundleAssets/_import/bundles.tsv
// (level scenes in their own bundles, as Unity requires). The player opens them with LocalBundles.FileSource.
//
// Output: Build/PSVitaData/ACID/AcierData (copy to ux0:data/ACID/AcierData on the Vita): the offline data next to
// the project (../AcierData: game database, dictionaries, missions, boot.txt) plus Bundles/ with the bundles and
// bundles.tsv.
// Batch: Unity.exe -batchmode -quit -projectPath <path> -buildTarget PSP2 -executeMethod ACIDBundleBuild.BuildBatch
//        [-acidBundles name,name] (only those bundle keys)
public static class ACIDBundleBuild
{
    private const string OutputRoot = "Build/PSVitaData/ACID/AcierData";

    [MenuItem("AC Identity/Build PS Vita Bundles")]
    public static void Build()
    {
        string error = BuildInternal(null);
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError("ACIDBundleBuild: " + error);
        }
    }

    public static void BuildBatch()
    {
        string[] args = Environment.GetCommandLineArgs();
        int i = Array.IndexOf(args, "-acidBundles");
        HashSet<string> only = (i >= 0 && i + 1 < args.Length) ? new HashSet<string>(args[i + 1].ToLowerInvariant().Split(',')) : null;
        string error = BuildInternal(only);
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError("ACIDBundleBuild: " + error);
        }
        EditorApplication.Exit(string.IsNullOrEmpty(error) ? 0 : 1);
    }

    private static string BuildInternal(HashSet<string> _only)
    {
        if (!File.Exists(LocalBundles.ManifestPath))
        {
            return "no " + LocalBundles.ManifestPath + " (run Tools/BundleImport/import_bundles.py)";
        }
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.PSP2)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.PSP2, BuildTarget.PSP2);
        }

        // bundle key -> project paths; scenes go in bundles of their own
        Dictionary<string, HashSet<string>> assets = new Dictionary<string, HashSet<string>>();
        Dictionary<string, string> scenes = new Dictionary<string, string>();
        foreach (string line in File.ReadAllLines(LocalBundles.ManifestPath))
        {
            string[] f = line.Split('\t');
            if (f.Length < 5 || !File.Exists(f[4]))
            {
                continue;
            }
            string key = LocalBundles.BundleKey(f[0]);
            if (_only != null && !_only.Contains(key))
            {
                continue;
            }
            if (f[1] == "1")
            {
                scenes[key] = f[4];
                continue;
            }
            HashSet<string> paths;
            if (!assets.TryGetValue(key, out paths))
            {
                paths = new HashSet<string>();
                assets.Add(key, paths);
            }
            paths.Add(f[4]);
        }
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        foreach (KeyValuePair<string, HashSet<string>> kv in assets)
        {
            builds.Add(new AssetBundleBuild { assetBundleName = kv.Key, assetNames = kv.Value.OrderBy(p => p).ToArray() });
        }
        foreach (KeyValuePair<string, string> kv in scenes)
        {
            builds.Add(new AssetBundleBuild { assetBundleName = kv.Key, assetNames = new[] { kv.Value } });
        }

        string bundleDir = OutputRoot + "/Bundles";
        Directory.CreateDirectory(bundleDir);
        Debug.Log(string.Format("ACIDBundleBuild: building {0} bundles ({1} scenes) to {2}", builds.Count, scenes.Count, bundleDir));
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(bundleDir, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.PSP2);
        if (manifest == null)
        {
            return "BuildAssetBundles failed";
        }
        File.Copy(LocalBundles.ManifestPath, bundleDir + "/" + LocalBundles.ManifestName, true);

        string data = Path.GetFullPath(Path.Combine(Application.dataPath, "../../AcierData"));
        if (Directory.Exists(data))
        {
            foreach (string src in Directory.GetFiles(data, "*", SearchOption.AllDirectories))
            {
                string dst = Path.Combine(OutputRoot, src.Substring(data.Length + 1));
                Directory.CreateDirectory(Path.GetDirectoryName(dst));
                File.Copy(src, dst, true);
            }
        }
        long bytes = Directory.GetFiles(bundleDir).Sum(p => new FileInfo(p).Length);
        Debug.Log(string.Format("ACIDBundleBuild: done, {0} bundles, {1} MB, data in {2}", manifest.GetAllAssetBundles().Length, bytes / 1048576, OutputRoot));
        return null;
    }
}
