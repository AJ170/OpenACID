// Rips asset bundles one at a time with an installed AssetRipper (ULegacy fork), each into its own folder, so every
// exported file is known to come from exactly one bundle. A normal multi-bundle rip merges them, and assets that sit
// at the same path in several bundles (each bundle carried its own copy) come out as numbered copies that can no
// longer be told apart.
//
// Settings match the GUI rip (static mesh separation, engine assets ignored, shader disassembly, ...). Script GUIDs
// do not depend on the run, so every export references the same scripts as the project.
//
//   BundleRip --ripper <AssetRipper dir> --managed <Managed dlls dir> --out <dir> [--drop-scripts] <bundle or dir>...
//
// --drop-scripts deletes each export's decompiled scripts after it is written (the project has them already).
// --no-scripts skips exporting scripts altogether, but then every script reference becomes a placeholder GUID: don't.
//
// A bundle whose output folder already holds a finished export (.done) is skipped, so an interrupted run resumes.
using System.Reflection;
using System.Runtime.InteropServices;
using AssetRipper.Export.UnityProjects;
using AssetRipper.Export.UnityProjects.Configuration;
using AssetRipper.Import.Configuration;
using AssetRipper.Import.Logging;
using AssetRipper.SourceGenerated.Classes.ClassID_115;

internal static class Program
{
	private static string s_ripperDir = @"D:\AssetRipper Legacy\Release";

	private static int Main(string[] args)
	{
		string? managed = null;
		string? output = null;
		bool noScripts = false;
		bool dropScripts = false;
		List<string> inputs = new();
		for (int i = 0; i < args.Length; i++)
		{
			switch (args[i])
			{
				case "--ripper": s_ripperDir = args[++i]; break;
				case "--managed": managed = args[++i]; break;
				case "--out": output = args[++i]; break;
				case "--no-scripts": noScripts = true; break;
				case "--drop-scripts": dropScripts = true; break;
				default: inputs.Add(args[i]); break;
			}
		}
		if (output is null || inputs.Count == 0)
		{
			Console.Error.WriteLine("usage: BundleRip --ripper <dir> --managed <dir> --out <dir> [--no-scripts] <bundle or dir>...");
			return 2;
		}
		AppDomain.CurrentDomain.AssemblyResolve += (_, e) =>
		{
			// Windows implementations first (runtimes/win/lib/...): the copies at the top level can be the
			// platform-neutral stubs (System.Drawing.Common throws PlatformNotSupported there).
			string file = new AssemblyName(e.Name).Name + ".dll";
			string winLib = Path.Combine(s_ripperDir, "runtimes", "win", "lib");
			string? path = Directory.Exists(winLib)
				? Directory.GetFiles(winLib, file, SearchOption.AllDirectories).OrderByDescending(p => p, StringComparer.Ordinal).FirstOrDefault()
				: null;
			path ??= Path.Combine(s_ripperDir, file);
			return File.Exists(path) ? Assembly.LoadFrom(path) : null;
		};
		NativeLibrary.SetDllImportResolver(typeof(Program).Assembly, (_, _, _) => IntPtr.Zero);
		Environment.SetEnvironmentVariable("PATH", Path.Combine(s_ripperDir, "runtimes", "win-x64", "native") + ";" + s_ripperDir + ";" + Environment.GetEnvironmentVariable("PATH"));
		return Run(managed, output, noScripts, dropScripts, inputs);
	}

	// Separate method: the AssetRipper types are only resolved once the resolver above is in place.
	private static int Run(string? managed, string output, bool noScripts, bool dropScripts, List<string> inputs)
	{
		List<string> bundles = new();
		foreach (string input in inputs)
		{
			if (Directory.Exists(input))
			{
				bundles.AddRange(Directory.GetFiles(input, "*.unity3d", SearchOption.AllDirectories).OrderBy(p => p, StringComparer.Ordinal));
			}
			else
			{
				bundles.Add(input);
			}
		}
		Directory.CreateDirectory(output);
		Logger.Add(new FileLogger(Path.Combine(output, "BundleRip.log")));
		Logger.Add(new ConsoleLogger(false));
		int failed = 0;
		foreach (string bundle in bundles)
		{
			string name = Path.GetFileNameWithoutExtension(bundle);
			string target = Path.Combine(output, name);
			string done = Path.Combine(target, ".done");
			if (File.Exists(done))
			{
				Console.WriteLine($"[BundleRip] skip {name} (done)");
				continue;
			}
			if (Directory.Exists(target))
			{
				// A partial export of this bundle from an interrupted run: only ever a folder named after the bundle,
				// inside the output folder given on the command line.
				Directory.Delete(target, true);
			}
			Console.WriteLine($"[BundleRip] {name}");
			try
			{
				Ripper ripper = new();
				Configure(ripper.Settings);
				List<string> paths = new() { bundle };
				if (managed is not null)
				{
					paths.Add(managed);
				}
				ripper.Load(paths);
				if (noScripts)
				{
					ripper.ExportProject(target, a => a is not IMonoScript, null);
				}
				else
				{
					ripper.ExportProject(target);
				}
				ripper.ResetData();
				if (dropScripts)
				{
					// The project already has the decompiled scripts (same GUIDs); every export would add another
					// copy of all of them (~24 MB, 4000 files). Only folders inside this bundle's own output.
					foreach (string dir in new[] { Path.Combine(target, "Assets", "Scripts"), Path.Combine(target, "AuxiliaryFiles", "GameAssemblies") })
					{
						if (Directory.Exists(dir))
						{
							Directory.Delete(dir, true);
						}
					}
				}
				File.WriteAllText(done, bundle);
			}
			catch (Exception ex)
			{
				failed++;
				Console.WriteLine($"[BundleRip] FAILED {name}: {ex}");
			}
		}
		Console.WriteLine($"[BundleRip] finished, {bundles.Count} bundles, {failed} failed");
		return failed == 0 ? 0 : 1;
	}

	private static void Configure(LibraryConfiguration s)
	{
		s.ScriptContentLevel = ScriptContentLevel.Level2;
		s.StreamingAssetsMode = StreamingAssetsMode.Extract;
		s.BundledAssetsExportMode = BundledAssetsExportMode.GroupByBundleName;
		s.AudioExportFormat = AudioExportFormat.Default;
		s.ImageExportFormat = ImageExportFormat.Png;
		s.MeshExportFormat = MeshExportFormat.Native;
		s.ScriptExportMode = ScriptExportMode.Decompiled;
		s.ScriptLanguageVersion = ScriptLanguageVersion.AutoSafe;
		s.ShaderExportMode = ShaderExportMode.Disassembly;
		s.SpriteExportMode = SpriteExportMode.Yaml;
		s.TerrainExportMode = TerrainExportMode.Yaml;
		s.TextExportMode = TextExportMode.Parse;
		s.EnablePrefabOutlining = false;
		s.EnableStaticMeshSeparation = true;
		s.IgnoreEngineAssets = true;
		s.ForceUpgradeToNewUnity = false;
	}
}
