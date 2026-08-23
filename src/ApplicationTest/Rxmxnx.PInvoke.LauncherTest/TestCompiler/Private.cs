namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	private static String[] GetFrameworkPlatformTargets()
		=> Environment.Is64BitOperatingSystem ? ["x86", "x64",] : ["x86",];
	private static FileInfo[] GetUwpBundles(DirectoryInfo directory)
		=> directory.GetFiles("*.appxbundle", SearchOption.AllDirectories);
	private static String? GetInstalledUapPlatformVersion()
	{
		const String preferredVersion = "10.0.16299.0";
		String unionMetadata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
		                                    "Windows Kits", "10", "UnionMetadata");
		if (!Directory.Exists(unionMetadata))
			return null;
		if (File.Exists(Path.Combine(unionMetadata, preferredVersion, "Windows.winmd")))
			return preferredVersion;

		String? latestName = null;
		Version? latest = null;
		foreach (DirectoryInfo directory in new DirectoryInfo(unionMetadata).GetDirectories())
		{
			if (!Version.TryParse(directory.Name, out Version? version) ||
			    !File.Exists(Path.Combine(directory.FullName, "Windows.winmd")))
				continue;
			if (latest is not null && version <= latest) continue;
			latest = version;
			latestName = directory.Name;
		}
		return latestName;
	}
	private static String[] GetFrameworkExecutables(DirectoryInfo projectDirectory)
		=> projectDirectory.GetDirectories("*.ApplicationTest.Legacy", SearchOption.AllDirectories)
		                   .SelectMany(static d => d.GetDirectories("bin", SearchOption.TopDirectoryOnly))
		                   .SelectMany(static bin => bin.GetFiles("*.exe", SearchOption.AllDirectories))
		                   .Where(static f => TestCompiler.IsFrameworkExecutable(f)).Select(static f => f.FullName)
		                   .ToArray();
	private static Boolean IsFrameworkExecutable(FileInfo file)
	{
		if (!file.Name.Contains("ApplicationTest", StringComparison.OrdinalIgnoreCase) ||
		    file.Name.Contains("mono", StringComparison.OrdinalIgnoreCase))
			return false;
		for (DirectoryInfo? directory = file.Directory; directory is not null; directory = directory.Parent)
		{
			if (directory.Name.StartsWith("net4", StringComparison.OrdinalIgnoreCase))
				return true;
			if (directory.Name.Equals("bin", StringComparison.OrdinalIgnoreCase))
				break;
		}
		return false;
	}
	private static async Task CompileNetApp(Boolean onlyNativeAot, RestoreNetArgs restoreArgs, Architecture arch,
		String outputPath)
	{
		CompileNetArgs compileArgs = new(restoreArgs, outputPath)
		{
			BuildDependencies = true, Publish = Publish.SelfContained,
		};

		if (!OperatingSystem.IsFreeBSD())
			await TestCompiler.RestoreNet(restoreArgs);

		try
		{
			if (!onlyNativeAot)
			{
				await TestCompiler.CompileNet(compileArgs);
				compileArgs.BuildDependencies = false;

				if (!OperatingSystem.IsFreeBSD())
				{
					compileArgs.Publish = Publish.ReadyToRun;
					await TestCompiler.CompileNet(compileArgs);
				}
			}

			if (!Utilities.IsNativeAotSupported(arch, restoreArgs.Version)) return;

			compileArgs.Publish = Publish.NativeAot;
			await TestCompiler.CompileNet(compileArgs);
			await TestCompiler.CompileNetWithSwitches(compileArgs);

			if (!restoreArgs.ProjectFile.EndsWith(".csproj") || restoreArgs.Version > NetVersion.Net90) return;
			compileArgs.BuildDependencies = false;
			compileArgs.Publish |= Publish.NoReflection;
			await TestCompiler.CompileNet(compileArgs);
			await TestCompiler.CompileNetWithSwitches(compileArgs);
		}
		finally
		{
			TestCompiler.NetCleanUp(restoreArgs);
		}
	}
	private static async Task CompileWebApi(RestoreNetArgs restoreArgs, Architecture arch, String outputPath)
	{
		if (!Utilities.IsNativeAotSupported(arch, restoreArgs.Version)) return;

		CompileNetArgs compileArgs = new(restoreArgs, outputPath)
		{
			BuildDependencies = true, Publish = Publish.NativeAot,
		};

		try
		{
			if (!OperatingSystem.IsFreeBSD())
				await TestCompiler.RestoreNet(restoreArgs);
			await TestCompiler.CompileNet(compileArgs);
		}
		finally
		{
			TestCompiler.NetCleanUp(restoreArgs);
		}
	}
	private static async Task CompileNetWithSwitches(CompileNetArgs compileArgs)
	{
		await TestCompiler.CompileNet(compileArgs with
		{
			Publish = compileArgs.Publish | Publish.InvariantGlobalization,
		});
		if (!compileArgs.Publish.HasFlag(Publish.NoReflection))
		{
			await TestCompiler.CompileNet(compileArgs with
			{
				Publish = compileArgs.Publish | Publish.DisableBufferAutoComposition,
			});
			await TestCompiler.CompileNet(compileArgs with
			{
				Publish = compileArgs.Publish | Publish.DisableBufferAutoComposition |
				Publish.InvariantGlobalization,
			});
		}
	}
	private static async Task CompileNet(CompileNetArgs args)
	{
		ExecuteState<CompileNetArgs> state = new()
		{
			ExecutablePath = "dotnet",
			ArgState = args,
			AppendArgs = CompileNetArgs.Append,
			Notifier = ConsoleNotifier.Notifier,
		};
		await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
	}
	private static async Task RestoreNet(RestoreNetArgs args)
	{
		ExecuteState<RestoreNetArgs> state = new()
		{
			ExecutablePath = "dotnet",
			ArgState = args,
			AppendArgs = RestoreNetArgs.Append,
			Notifier = ConsoleNotifier.Notifier,
		};
		await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
	}
	private static Boolean ArchSupported(Architecture arch)
	{
		Architecture currentArch = RuntimeInformation.OSArchitecture;
		return arch == currentArch || currentArch switch
		{
			Architecture.X86 => OperatingSystem.IsWindows(),
			Architecture.Arm or Architecture.Armv6 => OperatingSystem.IsLinux(),
			_ => !OperatingSystem.IsFreeBSD(),
		};
	}
	private static void NetCleanUp(RestoreNetArgs restoreArgs)
	{
		try
		{
			DirectoryInfo projectDirectory = new FileInfo(restoreArgs.ProjectFile).Directory!;
			projectDirectory.GetDirectories("bin", SearchOption.TopDirectoryOnly).FirstOrDefault()?.Delete();
			projectDirectory.GetDirectories("obj", SearchOption.TopDirectoryOnly).FirstOrDefault()?.Delete();
		}
		catch (Exception)
		{
			// Ignore
		}
	}
}