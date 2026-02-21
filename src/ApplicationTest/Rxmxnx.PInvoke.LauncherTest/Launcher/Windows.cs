namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	[SupportedOSPlatform("WINDOWS")]
	private sealed partial class Windows : Launcher, ILauncher<Windows>
	{
		private const String zLibUrl = "http://www.winimage.com/zLibDll/zlib123.zip";
#if ZLINK_STATIC
		private const String zLib32Url = "http://www.winimage.com/zLibDll/zlib123dll.zip";
		private const String zLib64Url = "http://www.winimage.com/zLibDll/zlib123dllx64.zip";
#endif

		private readonly Dictionary<Architecture, CppCompiler> _cppCompilers = new();
		private readonly List<MonoLauncher>? _monoLaunchers;

		public static OSPlatform Platform => OSPlatform.Windows;
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
			initialize = Windows.PrepareCompilers(this._cppCompilers, this.Architectures);
			Windows.AppendMonoLauncher(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono"),
				Architecture.X64, ref this._monoLaunchers);
			if (this.CurrentArch is not Architecture.X86)
				Windows.AppendMonoLauncher(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono"),
					Architecture.X86, ref this._monoLaunchers);
		}

		public override ICppCompiler? GetCompiler(Architecture arch) => this._cppCompilers.GetValueOrDefault(arch);
		public override async Task<String?> GetZlibPath()
		{
			DirectoryInfo zlibDir =
				new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				                 "Rxmxnx.PInvoke.Launcher", "zlib"));

			if (!zlibDir.Exists)
				zlibDir.Create();

			DirectoryInfo includeDir = zlibDir.CreateSubdirectory("include");
#if ZLINK_STATIC
			DirectoryInfo lib32Dir = zlibDir.CreateSubdirectory($"{Architecture.X86}");
			DirectoryInfo lib64Dir = zlibDir.CreateSubdirectory($"{Architecture.X64}");
#endif

			Boolean downloadHeaders = includeDir.GetFiles("z*.h").Count(f => f.Name is "zlib.h" or "zconf.h") < 2;
#if ZLINK_STATIC
			Boolean download32Lib = lib32Dir.GetFiles("zlibstat.lib").Length == 0;
			Boolean download64Lib = lib64Dir.GetFiles("zlibstat.lib").Length == 0;
			
			HttpClient httpClient = downloadHeaders || download32Lib || download64Lib ? new() : default!;
#else
			HttpClient httpClient = downloadHeaders ? new() : default!;
#endif
			if (downloadHeaders)
				await Windows.DownloadZLibHeadersAsync(httpClient, includeDir);
#if ZLINK_STATIC
			if (download32Lib)
				await Windows.DownloadStaticZLibAsync(httpClient, Windows.zLib32Url, lib32Dir);
			if (download64Lib)
				await Windows.DownloadStaticZLibAsync(httpClient, Windows.zLib64Url, lib64Dir);
#endif
			return zlibDir.FullName;
		}

		public static Windows Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);

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
		private static async Task DownloadZLibHeadersAsync(HttpClient httpClient, DirectoryInfo includeDir)
		{
			await using MemoryStream zipStream = await Windows.DownloadZipFile(httpClient, Windows.zLibUrl);
			await using ZipArchive archive =
				await ZipArchive.CreateAsync(zipStream, ZipArchiveMode.Read, true, default);
			await archive.GetEntry("zlib.h")!.ExtractToFileAsync(Path.Combine(includeDir.FullName, "zlib.h"));
			await archive.GetEntry("zconf.h")!.ExtractToFileAsync(Path.Combine(includeDir.FullName, "zconf.h"));
		}
#if ZLINK_STATIC
		private static async Task DownloadStaticZLibAsync(HttpClient httpClient, String libUrl, DirectoryInfo libDir)
		{
			await using MemoryStream zipStream = await Windows.DownloadZipFile(httpClient, libUrl);
			await using ZipArchive archive =
				await ZipArchive.CreateAsync(zipStream, ZipArchiveMode.Read, true, default);
			await archive.Entries.First(a => a.Name.EndsWith("zlibstat.lib"))
			             .ExtractToFileAsync(Path.Combine(libDir.FullName, "zlibstat.lib"));
		}
#endif
		private static async Task<MemoryStream> DownloadZipFile(HttpClient httpClient, String url)
		{
			using HttpResponseMessage response = await httpClient.GetAsync(url);
			MemoryStream zipStream = new();
			await response.Content.CopyToAsync(zipStream);
			return zipStream;
		}
		private static async Task PrepareCompilers(Dictionary<Architecture, CppCompiler> cppCompilers,
			Architecture[] architectures)
		{
			String vcPath = await Windows.GetVisualCppPath();
			String kitPath = Windows.GetWindowsKitPath();
			foreach (Architecture arch in architectures)
				cppCompilers.Add(arch, new(vcPath, kitPath, arch));
		}
		private static async Task<String> GetVisualCppPath()
		{
			const String registryPath = @"\Microsoft\VisualStudio\SxS\VS7";
			try
			{
				String vsWherePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
				                                  "Microsoft Visual Studio", "Installer", "vswhere.exe");
				ExecuteState<String> vsPropertyQuery = new()
				{
					ExecutablePath = vsWherePath,
					ArgState = "installationVersion",
					AppendArgs = (prop, c) =>
					{
						c.Add("-latest");
						c.Add("-property");
						c.Add(prop);
					},
					Notifier = ConsoleNotifier.Notifier,
				};
				String vsVersionText =
					await Utilities.ExecuteWithOutput(vsPropertyQuery, ConsoleNotifier.CancellationToken);
				ConsoleNotifier.Notifier.Print($"Visual Studio Version {vsVersionText} found.");

				Version vsVersion = Version.Parse(vsVersionText);
				String registryKey = $"{vsVersion.Major}.0";

				Boolean writable = false;
				if (Windows.OpenSoftwareKey(registryPath) is not { } registry)
				{
					registry = Windows.CreateSoftwareKey(registryPath);
					writable = true;
				}
				if (registry.GetValue(registryKey) is not String vsPath || String.IsNullOrWhiteSpace(vsPath))
				{
					vsPath = await Utilities.ExecuteWithOutput(vsPropertyQuery with { ArgState = "installationPath", },
					                                           ConsoleNotifier.CancellationToken);
					if (writable)
						registry.SetValue(registryKey, vsPath);
					else
						Windows.SetSoftwareValue(registryPath, registryKey, vsPath);
					ConsoleNotifier.Notifier.Print($"Set {registry} @{registryKey} -> {vsPath}");
				}
				String vcVersionText = await File.ReadAllTextAsync(
					Path.Combine(vsPath, "VC", "Auxiliary", "Build", "Microsoft.VCToolsVersion.default.txt"));
				Version vcVersion = Version.Parse(vcVersionText);
				ConsoleNotifier.Notifier.Print($"Visual C++ {vcVersion} found.");
				return Path.Combine(vsPath, "VC", "Tools", "MSVC", vcVersion.ToString());
			}
			catch (Exception ex)
			{
				ConsoleNotifier.Notifier.PrintError("Fail Microsoft Visual Studio Detection", ex);
				throw;
			}
		}
		private static void SetSoftwareValue(String registryPath, String name, String value)
		{
			RegistryKey registry = Windows.OpenSoftwareKey(registryPath, true)!;
			registry.SetValue(name, value);
		}
		private static RegistryKey? OpenSoftwareKey(String registryPath, Boolean writable = false)
		{
			RegistryKey? result = default;
			if (Environment.Is64BitOperatingSystem)
			{
				if (Registry.LocalMachine.OpenSubKey(@$"SOFTWARE\Wow6432Node\{registryPath}", writable) is { } local32)
					result = local32;
				else if (Registry.CurrentUser.OpenSubKey(@$"SOFTWARE\Wow6432Node\{registryPath}", writable) is
				         { } user32)
					result = user32;
			}
			if (result is null && Registry.LocalMachine.OpenSubKey(@$"SOFTWARE\{registryPath}", writable) is { } local)
				result = local;
			result ??= Registry.CurrentUser.OpenSubKey(@$"SOFTWARE\{registryPath}", true);

			if (result is not null && !writable)
				ConsoleNotifier.Notifier.Print($"Found registry key {result}");
			return result;
		}
		private static RegistryKey CreateSoftwareKey(String registryPath)
		{
			String prefix = Environment.Is64BitOperatingSystem == Environment.Is64BitProcess ?
				@"SOFTWARE\" :
				@"SOFTWARE\Wow6432Node\";
			RegistryKey result = Registry.CurrentUser.CreateSubKey($"{prefix}{registryPath}", true);
			ConsoleNotifier.Notifier.Print($"Created registry key {result}");
			return result;
		}
		private static String GetWindowsKitPath()
		{
			const String registryPath = @"\Microsoft\Microsoft SDKs\Windows";
			try
			{
				RegistryKey registry = Windows.OpenSoftwareKey(registryPath)!;
				Dictionary<Version, String> kitsLibs = new();
				foreach (String kitPath in Windows.GetWindowsKits(registry))
				{
					DirectoryInfo libPath = new(Path.Combine(kitPath, "Lib"));
					if (!libPath.Exists) continue;
					Windows.GetVersions(libPath, kitsLibs);
				}
				return kitsLibs[kitsLibs.Keys.Max()!];
			}
			catch (Exception ex)
			{
				ConsoleNotifier.Notifier.PrintError("Fail Microsoft Windows Kit Detection", ex);
				throw;
			}
		}
		private static void GetVersions(DirectoryInfo libPath, Dictionary<Version, String> kitsLibs)
		{
			foreach (DirectoryInfo version in libPath.GetDirectories())
			{
				if (!Version.TryParse(version.Name, out Version? libVersion))
				{
					ConsoleNotifier.Notifier.Print($"Invalid Windows Kit directory [{version.FullName}].");
					continue;
				}
				DirectoryInfo? umDir = version.GetDirectories("um").FirstOrDefault();
				DirectoryInfo? ucrtDir = version.GetDirectories("ucrt").FirstOrDefault();
				if (umDir is null || ucrtDir is null)
				{
					String missing = umDir is null && ucrtDir is null ? "'um' and 'ucrt'" :
						umDir is null ? "'um'" : "'ucrt'";
					ConsoleNotifier.Notifier.Print($"Missing {missing} libraries [{version.FullName}].");
					continue;
				}
				kitsLibs.Add(libVersion, libPath.FullName);
				ConsoleNotifier.Notifier.Print($"Found libraries [{version.FullName}] found.");
			}
		}
		private static String[] GetWindowsKits(RegistryKey registry)
		{
			Dictionary<Version, String> kits = new();
			foreach (String subKeyName in registry.GetSubKeyNames())
			{
				RegistryKey subKeyRegistry = registry.OpenSubKey(subKeyName)!;
				String? installationFolder = subKeyRegistry.GetValue("InstallationFolder")?.ToString();
				String? version = subKeyRegistry.GetValue("ProductVersion")?.ToString();
				if (String.IsNullOrWhiteSpace(installationFolder) ||
				    !Version.TryParse(version, out Version? kitVersion))
					continue;
				kits.Add(kitVersion, installationFolder);
				ConsoleNotifier.Notifier.Print($"Windows Kit Version {kitVersion} [{installationFolder}] found.");
			}
			return kits.OrderBy(p => p.Key).Select(p => p.Value).ToArray();
		}

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