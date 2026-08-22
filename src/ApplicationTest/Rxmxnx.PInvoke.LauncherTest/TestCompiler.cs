namespace Rxmxnx.PInvoke.ApplicationTest;

public static partial class TestCompiler
{
	public static async Task CompileNet(DirectoryInfo projectDirectory, String os, String outputPath,
		NetVersion[] netVersions, Boolean onlyNativeAot = false)
	{
		Architecture[] architectures = OperatingSystem.IsWindows() ?
			[Architecture.X86, Architecture.X64, Architecture.Arm64,] :
			OperatingSystem.IsMacOS() ? [Architecture.X64, Architecture.Arm64,] :
				OperatingSystem.IsLinux() ? [Architecture.X64, Architecture.Arm, Architecture.Arm64,] :
					[RuntimeInformation.OSArchitecture,];

		NetVersion maxNetVersion = netVersions.Max();
		String[] appProjectFiles = projectDirectory.GetDirectories("*.*ApplicationTest", SearchOption.AllDirectories)
		                                           .SelectMany(d => d.GetFiles("*.*proj")).Select(f => f.FullName)
		                                           .ToArray();
		String? webProjectFile = projectDirectory.GetDirectories("*.*WebApiTest", SearchOption.AllDirectories)
		                                         .SelectMany(d => d.GetFiles("*.*proj")).Select(f => f.FullName)
		                                         .FirstOrDefault();

		foreach (Architecture arch in architectures)
		{
			if (!TestCompiler.ArchSupported(arch)) continue;

			String rid = $"{os}-{Enum.GetName(arch)!.ToLower()}";
			foreach (NetVersion netVersion in netVersions)
			{
				foreach (String appProjectFile in appProjectFiles)
				{
					await TestCompiler.CompileNetApp(onlyNativeAot,
					                                 new()
					                                 {
						                                 ProjectFile = appProjectFile,
						                                 RuntimeIdentifier = rid,
						                                 Version = netVersion,
					                                 }, arch, outputPath);
				}

				if (netVersion != maxNetVersion || String.IsNullOrEmpty(webProjectFile)) continue;
				await TestCompiler.CompileWebApi(
					new() { ProjectFile = webProjectFile, RuntimeIdentifier = rid, Version = netVersion, }, arch,
					outputPath);
			}
		}
	}
	public static async Task CompileMono(DirectoryInfo projectDirectory, MonoLauncher monoLauncher, String outputPath)
	{
		FileInfo[] appProjectFiles =
		[
			.. projectDirectory.GetDirectories("*.ApplicationTest", SearchOption.AllDirectories)
			                   .SelectMany(d => d.GetFiles("*.*proj")),
			.. projectDirectory.GetDirectories("*.ApplicationTest.Legacy", SearchOption.AllDirectories)
			                   .SelectMany(d => d.GetFiles("*.*proj")),
		];
		foreach (FileInfo appProjectFile in appProjectFiles)
		{
			String appDirectory = appProjectFile.DirectoryName ?? String.Empty;
			String appName = appProjectFile.Name[..appProjectFile.Name.AsSpan().LastIndexOf('.')];
			String appOutputDirectory = Path.Combine(outputPath, appName);

			Directory.CreateDirectory(appOutputDirectory);
			ExecuteState<CompileMonoArgs> state = new()
			{
				ExecutablePath = monoLauncher.MsbuildPath,
				ArgState = new()
				{
					ProjectFile = appProjectFile.FullName,
					OutputPath = Path.GetRelativePath(appDirectory, appOutputDirectory),
				},
				AppendArgs = CompileMonoArgs.Append,
				Notifier = ConsoleNotifier.Notifier,
				WorkingDirectory = appDirectory,
			};
			await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
		}
	}
	[SupportedOSPlatform("WINDOWS")]
	public static async Task CompileAppx(String msbuildPath, DirectoryInfo projectDirectory, String outputPath)
	{
		String[] appProjectFiles = projectDirectory
		                           .GetDirectories("*.*ApplicationTest.Windows", SearchOption.AllDirectories)
		                           .SelectMany(d => d.GetFiles("*.*proj")).Select(f => f.FullName).ToArray();
		foreach (String appProjectFile in appProjectFiles)
		{
			DirectoryInfo tempDirectory = new(Path.Combine(Path.GetTempPath(), $"{Guid.CreateVersion7()}"));
			ExecuteState<CompileAppxArgs> state = new()
			{
				ExecutablePath = msbuildPath,
				ArgState = new() { ProjectPath = appProjectFile, OutputPath = tempDirectory.FullName, },
				AppendArgs = CompileAppxArgs.Append,
				Notifier = ConsoleNotifier.Notifier,
			};

			tempDirectory.Create();
			await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
			foreach (FileInfo appx in tempDirectory.GetFiles("*.appxbundle", SearchOption.AllDirectories))
				appx.MoveTo(Path.Combine(outputPath, appx.Name), true);
			tempDirectory.Delete(true);
			if (Utilities.ShowDiagnostics)
				ConsoleNotifier.ShowDiskUsage();
		}
	}
	[SupportedOSPlatform("WINDOWS")]
	public static async Task<String[]> CompileFramework(DirectoryInfo projectDirectory)
	{
		FileInfo[] appProjectFiles = projectDirectory
		                             .GetDirectories("*.ApplicationTest.Legacy", SearchOption.AllDirectories)
		                             .SelectMany(static d => d.GetFiles("*.*proj")).ToArray();
		foreach (FileInfo appProjectFile in appProjectFiles)
		{
			ExecuteState<String> state = new()
			{
				ExecutablePath = "dotnet",
				ArgState = appProjectFile.FullName,
				AppendArgs = static (p, a) =>
				{
					a.Add("build");
					a.Add(p);
					a.Add("-c");
					a.Add("Release");
					a.Add("/p:UsePackage=true");
				},
				Notifier = ConsoleNotifier.Notifier,
			};
			Int32 result = await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
			if (result != 0)
				throw new InvalidOperationException(
					$".NET Framework legacy compilation failed with exit code 0x{result:x8}.");
			if (Utilities.ShowDiagnostics)
				ConsoleNotifier.ShowDiskUsage();
		}
		String[] executablePaths = TestCompiler.GetFrameworkExecutables(projectDirectory);
		if (executablePaths.Length == 0)
			throw new InvalidOperationException("No .NET Framework legacy executables were produced.");
		return executablePaths;
	}
}
