using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

// Port tool: writes every built-in shader of this Unity version (name, local file ID, file GUID) to
// Logs/builtin_shaders.txt, so ripped materials that used a built-in shader can be pointed at it.
// Batch: Unity.exe -batchmode -quit -projectPath <path> -executeMethod ACIDBuiltinShaderMap.Run
public static class ACIDBuiltinShaderMap
{
    private static readonly string[][] s_files =
    {
        new[] { "Resources/unity_builtin_extra", "0000000000000000f000000000000000" },
        new[] { "Library/unity default resources", "0000000000000000e000000000000000" },
    };

    public static void Run()
    {
        StringBuilder sb = new StringBuilder();
        foreach (string[] file in s_files)
        {
            foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath(file[0]))
            {
                Shader shader = asset as Shader;
                if (shader != null)
                {
                    sb.AppendFormat("{0}\t{1}\t{2}\n", shader.name, Unsupported.GetLocalIdentifierInFile(shader.GetInstanceID()), file[1]);
                }
            }
        }
        Directory.CreateDirectory("Logs");
        File.WriteAllText("Logs/builtin_shaders.txt", sb.ToString());
        Debug.Log("ACIDBuiltinShaderMap: wrote Logs/builtin_shaders.txt");
    }
}
