namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed partial class Windows
	{
		private static void AppendMonoLauncher(String monoPath, Architecture arch,
			ref List<MonoLauncher>? monoLaunchers)
		{
			String monoBinPath = Path.Combine(monoPath, "bin");
			String executablePath = Path.Combine(monoBinPath, "mono.exe");
			if (!File.Exists(executablePath)) return;

			String monoLibPath = Path.Combine(monoPath, "lib");
			String monoRuntimePath = Path.Combine(monoLibPath, "mono", "4.5");
			monoLaunchers ??= new(2);
			String mkbundleBatchPath = Windows.PatchMaker(arch, monoBinPath, monoRuntimePath, executablePath);
			monoLaunchers.Add(new()
			{
				Architecture = arch,
				NativeRuntimeName = "mono-2.0-sgen.dll",
				MsbuildPath = Path.Combine(monoBinPath, "msbuild.bat"),
				LinkerPath = Path.Combine(monoBinPath, "monolinker.bat"),
				MakerPath = mkbundleBatchPath,
				NativeRuntimePath = Path.Combine(monoBinPath, "mono-2.0-sgen.dll"),
				MonoCilStripAssemblyPath = Path.Combine(monoRuntimePath, "mono-cil-strip.exe"),
				IncludeRuntimePath = Path.Combine(monoPath, "include", "mono-2.0"),
				StaticRuntimePath = Path.Combine(monoLibPath, "libmono-static-sgen.lib"),
				ExecutablePath = executablePath,
			});
		}
		[SuppressMessage("ReSharper", "UseRawString")]
		private static String PatchMaker(Architecture arch, String monoBinPath, String monoRuntimePath,
			String executablePath)
		{
			ConsoleNotifier.Notifier.Print($"Patching Mono {monoRuntimePath}...");
			String monoPatchPath = Windows.GetMakerPath(arch);
			String result = Path.Combine(monoBinPath, "mkbundle.bat");
			try
			{
				Int32 patchResult =
					Windows.PatchAssemblyForWindows(monoRuntimePath, monoRuntimePath.Length, monoPatchPath,
					                                monoPatchPath.Length);
				ConsoleNotifier.Notifier.Result(patchResult, "mkbundle.exe patch");
				if (patchResult >= 0)
				{
					String mkbundleAssemblyPath = Path.Combine(monoPatchPath, "mkbundle.exe");
					String mkbundleBatchPath = Path.Combine(monoPatchPath, "mkbundle.bat");

					File.WriteAllText(mkbundleBatchPath, $@"@echo off
""{executablePath}"" %MONO_OPTIONS% ""{mkbundleAssemblyPath}"" %*
", Encoding.ASCII);
					result = mkbundleBatchPath;
					ConsoleNotifier.Notifier.Print($"Patched Mono {monoRuntimePath} on {monoPatchPath}.", true);
				}
			}
			catch (Exception ex)
			{
				ConsoleNotifier.Notifier.PrintError($"Fail patching Mono {monoRuntimePath}", ex);
			}
			return result;
		}
		private static String GetMakerPath(Architecture arch)
		{
			String result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			                             "Rxmxnx.PInvoke.Mkbundle.Patcher", $"{arch}");
			Directory.CreateDirectory(result);
			return result;
		}

		[LibraryImport("Rxmxnx.PInvoke.Mkbundle.Patcher", StringMarshalling = StringMarshalling.Utf16)]
		private static partial Int32 PatchAssemblyForWindows(String monoLibPath, Int32 monoLibPathLength,
			String outputPath, Int32 outputPathLength);
	}
}