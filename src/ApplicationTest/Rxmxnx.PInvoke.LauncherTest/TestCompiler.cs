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
	public static async Task CompileMono(DirectoryInfo projectDirectory, String msbuildPath, String outputPath)
	{
		String[] appProjectFiles = projectDirectory.GetDirectories("*.*ApplicationTest", SearchOption.AllDirectories)
		                                           .SelectMany(d => d.GetFiles("*.*proj")).Select(f => f.FullName)
		                                           .ToArray();
		foreach (String appProjectFile in appProjectFiles)
		{
			String appDirectory = Path.GetDirectoryName(appProjectFile) ?? String.Empty;
			ExecuteState<CompileMonoArgs> state = new()
			{
				ExecutablePath = msbuildPath,
				ArgState = new()
				{
					ProjectFile = appProjectFile, OutputPath = Path.GetRelativePath(appDirectory, outputPath),
				},
				AppendArgs = CompileMonoArgs.Append,
				Notifier = ConsoleNotifier.Notifier,
			};
			await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
		}
	}
}