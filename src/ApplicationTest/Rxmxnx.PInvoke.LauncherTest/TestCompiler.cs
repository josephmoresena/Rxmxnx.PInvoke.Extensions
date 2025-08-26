namespace Rxmxnx.PInvoke.ApplicationTest;

public static partial class TestCompiler
{
	public static async Task CompileNet(DirectoryInfo projectDirectory, String os, String outputPath,
		Boolean onlyNativeAot = false)
	{
		Architecture[] architectures = OperatingSystem.IsWindows() ?
			[Architecture.X86, Architecture.X64, Architecture.Arm64,] :
			!OperatingSystem.IsLinux() ? [Architecture.X64, Architecture.Arm64,] :
				[Architecture.X64, Architecture.Arm, Architecture.Arm64,];

		String[] appProjectFiles = projectDirectory.GetDirectories("*.*ApplicationTest", SearchOption.AllDirectories)
		                                           .SelectMany(d => d.GetFiles("*.*proj")).Select(f => f.FullName)
		                                           .ToArray();

		foreach (Architecture arch in architectures)
		{
			if (!TestCompiler.ArchSupported(arch)) continue;

			String rid = $"{os}-{Enum.GetName(arch)!.ToLower()}";
			foreach (NetVersion netVersion in Enum.GetValues<NetVersion>())
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
		}
	}
	public static async Task CompileMono(DirectoryInfo projectDirectory, MonoLauncher monoLauncher, String outputPath)
	{
		FileInfo[] appProjectFiles = projectDirectory.GetDirectories("*.*ApplicationTest", SearchOption.AllDirectories)
		                                             .SelectMany(d => d.GetFiles("*.*proj")).ToArray();
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
}