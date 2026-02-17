namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed partial class Windows : Launcher, ILauncher<Windows>
	{
		public static OSPlatform Platform => OSPlatform.Windows;

		private readonly List<MonoLauncher>? _monoLaunchers;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "win";
		public override ReadOnlySpan<MonoLauncher> MonoLaunchers => CollectionsMarshal.AsSpan(this._monoLaunchers);

		private Windows(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X86 ||
				                                (a is Architecture.X64 && this.CurrentArch is not Architecture.X86))
			                         .ToArray();
			initialize = Task.CompletedTask;
			Windows.AppendMonoLauncher(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono"),
				Architecture.X64, ref this._monoLaunchers);
			if (this.CurrentArch is not Architecture.X86)
				Windows.AppendMonoLauncher(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono"),
					Architecture.X86, ref this._monoLaunchers);
		}
		[LibraryImport("Rxmxnx.PInvoke.Mkbundle.Patcher", StringMarshalling = StringMarshalling.Utf16)]
		private static partial Int32 PatchAssemblyForWindows(String monoLibPath, Int32 monoLibPathLength,
			String outputPath, Int32 outputPathLength);

		private static void AppendMonoLauncher(String monoPath, Architecture arch,
			ref List<MonoLauncher>? monoLaunchers)
		{
			String monoBinPath = Path.Combine(monoPath, "bin");
			String executablePath = Path.Combine(monoBinPath, "mono.exe");
			if (!File.Exists(executablePath)) return;

			String monoRuntimePath = Path.Combine(monoPath, "lib", "mono", "4.5");
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
				ExecutablePath = executablePath,
			});
		}
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

		public static Windows Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);

		private sealed class CppCompiler(String msvcPath, String kitPath, Architecture arch) : ICppCompiler
		{
			public IEnumerable<String> LibraryPaths
			{
				get
				{
					yield return Path.Combine(kitPath, "um", $"{arch}");
					yield return Path.Combine(kitPath, "ucrt", $"{arch}");
					yield return Path.Combine(msvcPath, "lib", $"{arch}");
				}
			}
			public String CompilerExecutable => Path.Combine(msvcPath, "bin", $"Host{arch}", $"{arch}", "cl.exe");
			public String DynamicRuntime => "/MD";
			public String IncludeFlag => "/I";
			public String OutputFlag => "/OUT:";
			public String StaticLibPathFlag => "/LIBPATH:";
			public IEnumerable<String> DefaultLink
			{
				get
				{
					yield return "kernel32.lib";
					yield return "user32.lib";
					yield return "advapi32.lib";
					yield return "shell32.lib";
					yield return "ole32.lib";
					yield return "oleaut32.lib";
					yield return "version.lib";
					yield return "ws2_32.lib";
					yield return "mswsock.lib";
					yield return "psapi.lib";
					yield return "winmm.lib";
					yield return "bcrypt.lib";
				}
			}
			public String EnableAllWarnings => String.Empty;
			public String RemovePointerWarnings => "/wd4090";
			public String BeginWholeLink => String.Empty;
			public String EndWholeLink => String.Empty;
			public String ExportDynamicSymbols => String.Empty;
			public String RuntimePath => String.Empty;
			public IEnumerable<String> BeginLink(Boolean windowApp)
			{
				yield return "/link";
				yield return $"/MACHINE:{arch}";
				yield return $"/SUBSYSTEM:{(windowApp ? "WINDOWS" : "CONSOLE")}";
			}
		}
	}
}