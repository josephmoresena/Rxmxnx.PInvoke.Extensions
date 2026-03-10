namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	[SupportedOSPlatform("WINDOWS")]
	private sealed partial class Windows
	{
		private const String zLibUrl = "http://www.winimage.com/zLibDll/zlib123.zip";
#if ZLINK_STATIC
		private const String zLib32Url = "http://www.winimage.com/zLibDll/zlib123dll.zip";
		private const String zLib64Url = "http://www.winimage.com/zLibDll/zlib123dllx64.zip";
#endif

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
		private static async Task DownloadZLibHeadersAsync(HttpClient httpClient, DirectoryInfo includeDir)
		{
			const String zLibHeaderName = "zlib.h";
			const String zConfHeaderName = "zconf.h";
			await using MemoryStream zipStream = await Utilities.DownloadZipFile(httpClient, Windows.zLibUrl);
#if NET10_0_OR_GREATER
			await using ZipArchive archive =
				await ZipArchive.CreateAsync(zipStream, ZipArchiveMode.Read, true, default);
			await archive.GetEntry(zLibHeaderName)!.ExtractToFileAsync(
				Path.Combine(includeDir.FullName, zLibHeaderName));
			await archive.GetEntry(zConfHeaderName)!.ExtractToFileAsync(
				Path.Combine(includeDir.FullName, zConfHeaderName));
#else
			using ZipArchive archive = new(zipStream, ZipArchiveMode.Read, true, default);
			archive.GetEntry(zLibHeaderName)!.ExtractToFile(Path.Combine(includeDir.FullName, zLibHeaderName));
			archive.GetEntry(zConfHeaderName)!.ExtractToFile(Path.Combine(includeDir.FullName, zConfHeaderName));
#endif
		}
#if ZLINK_STATIC
		private static async Task DownloadStaticZLibAsync(HttpClient httpClient, String libUrl, DirectoryInfo libDir)
		{
			const String libZStaticName = "zlibstat.lib";
			String libZPath = Path.Combine(libDir.FullName, libZStaticName);
			await using MemoryStream zipStream = await Utilities.DownloadZipFile(httpClient, libUrl);
#if NET10_0_OR_GREATER
			await using ZipArchive archive =
				await ZipArchive.CreateAsync(zipStream, ZipArchiveMode.Read, true, default);
			await archive.Entries.First(a => a.Name.EndsWith(libZStaticName))
			             .ExtractToFileAsync(Path.Combine(libDir.FullName, libZPath));
#else
			using ZipArchive archive = new(zipStream, ZipArchiveMode.Read, true, default);
			archive.Entries.First(a => a.Name.EndsWith(libZStaticName))
			       .ExtractToFile(Path.Combine(libDir.FullName, libZPath));
#endif
		}
#endif
		private static async Task PrepareCompilers(Dictionary<Architecture, CppCompiler> cppCompilers,
			Architecture[] architectures)
		{
			(String vcBuildPath, String msvcPath) = await Windows.GetVisualCppPath();
			String kitPath = Windows.GetWindowsKitLibPath();
			foreach (Architecture arch in architectures)
				cppCompilers.Add(arch, new(vcBuildPath, msvcPath, kitPath, arch));
		}
		private static async Task<(String, String)> GetVisualCppPath()
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
				String vcBuildPath = Path.Combine(vsPath, "VC", "Auxiliary", "Build");
				String vcVersionText = await File.ReadAllTextAsync(
					Path.Combine(vcBuildPath, "Microsoft.VCToolsVersion.default.txt"));
				Version vcVersion = Version.Parse(vcVersionText);
				ConsoleNotifier.Notifier.Print($"Visual C++ {vcVersion} found.");
				return (vcBuildPath, Path.Combine(vsPath, "VC", "Tools", "MSVC", vcVersion.ToString()));
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
		private static String GetWindowsKitLibPath()
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
					Windows.GetVersions(kitsLibs, libPath);
				}
				return kitsLibs[kitsLibs.Keys.Max()!];
			}
			catch (Exception ex)
			{
				ConsoleNotifier.Notifier.PrintError("Fail Microsoft Windows Kit Detection", ex);
				throw;
			}
		}
		private static void GetVersions(Dictionary<Version, String> kitsLibs, DirectoryInfo libPath)
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
				kitsLibs.Add(libVersion, version.FullName);
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

		[GeneratedRegex("(.+)=(.+)")]
		private static partial Regex VsEnvRegex();
	}
}