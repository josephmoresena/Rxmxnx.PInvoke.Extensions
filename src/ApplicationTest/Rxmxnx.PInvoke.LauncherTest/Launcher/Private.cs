namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private Launcher(DirectoryInfo outputDirectory, Boolean useMono)
	{
		this.OutputDirectory = outputDirectory;
		this.MonoOutputDirectory = useMono ? outputDirectory.CreateSubdirectory("Mono") : default;
		this.CurrentArch = RuntimeInformation.OSArchitecture;
	}
	private async Task<Int32> RunAppFile(FileInfo appFile, Architecture arch, String executionName)
	{
		using CancellationTokenSource source = new(TimeSpan.FromMinutes(5));
		CancellationTokenRegistration registry = ConsoleNotifier.RegisterCancellation(source);
		try
		{
			return await this.RunAppFile(appFile, arch, executionName, ConsoleNotifier.CancellationToken);
		}
		catch (OperationCanceledException)
		{
			return -1;
		}
		finally
		{
			registry.Unregister();
		}
	}

	private static FileInfo[] GetMonoExecutables(DirectoryInfo monoOutputDirectory)
		=> monoOutputDirectory.GetDirectories()
		                      .SelectMany(d => d.GetFiles("*ApplicationTest.*mono.exe", SearchOption.TopDirectoryOnly))
		                      .Where(f => f.DirectoryName!.EndsWith("ApplicationTest")).ToArray();
	private static async Task CompileMonoAot(MonoLauncher monoLauncher, String outputDirectory, FileInfo assemblyFile)
	{
		String logPath = Path.Combine(outputDirectory, $"{assemblyFile.Name}.{monoLauncher.Architecture}.Mono.AOT.log");
		String outputPath = assemblyFile.DirectoryName ?? String.Empty;
		String result = await Launcher.RunMonoAot(monoLauncher.ExecutablePath, assemblyFile.Name, outputPath, false);

		await Utilities.SaveTextFile(logPath, result);
		logPath = Path.Combine(outputDirectory, $"{assemblyFile.Name}.{monoLauncher.Architecture}.Mono.AOT.Hybrid.log");
		result = await Launcher.RunMonoAot(monoLauncher.ExecutablePath, assemblyFile.Name, outputPath, true);
		await Utilities.SaveTextFile(logPath, result);
		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
	}
	private static async Task PackMonoApp(MonoLauncher monoLauncher, ICppCompiler? cppCompiler, String? zLibPath,
		DirectoryInfo outputPath, FileInfo executableFile)
	{
		String applicationName = executableFile.Directory?.Name ?? executableFile.Name;
		FileInfo? linkedExecutableFile =
			await Launcher.LinkMonoApp(monoLauncher, outputPath, applicationName, executableFile);
		if (linkedExecutableFile is null) return;

		await Launcher.MakeMonoBundle(monoLauncher, outputPath, applicationName, linkedExecutableFile);
		Launcher.DeleteBundleTempFiles();
		if (cppCompiler is not null)
		{
			Boolean isWindowApp = Launcher.IsWindowApp(linkedExecutableFile);
			await Launcher.MakeMonoAotBundle(monoLauncher, cppCompiler, zLibPath, isWindowApp, outputPath,
			                                 applicationName, linkedExecutableFile);
			Launcher.DeleteBundleTempFiles();
		}
		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
	}
	private static async Task<FileInfo?> LinkMonoApp(MonoLauncher monoLauncher, DirectoryInfo outputPath,
		String applicationName, FileInfo assemblyExecutableFile)
	{
		String linkLog = Path.Combine(outputPath.FullName,
		                              $"{applicationName}.{monoLauncher.Architecture}.Mono.Link.log");
		DirectoryInfo linkOutputDirectory =
			outputPath.CreateSubdirectory($"{applicationName}.Link.{monoLauncher.Architecture}");
		String linkResult = await Launcher.RunMonoLink(monoLauncher.LinkerPath, assemblyExecutableFile.FullName,
		                                               linkOutputDirectory.FullName);
		await Utilities.SaveTextFile(linkLog, linkResult);
		return linkOutputDirectory.GetFiles(assemblyExecutableFile.Name).FirstOrDefault();
	}
	private static async Task<Int32> RunMonoAppFile(String monoExecutable, String appFilePath, String workingDirectory)
	{
		ExecuteState<String> state = new()
		{
			ExecutablePath = monoExecutable,
			ArgState = appFilePath,
			WorkingDirectory = workingDirectory,
			AppendArgs = static (s, c) => c.Add(s),
			Notifier = ConsoleNotifier.Notifier,
		};
		Int32 result = await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
		ConsoleNotifier.Notifier.Result(result, appFilePath);
		return result;
	}
	private static async Task<String> RunMonoAot(String monoExecutable, String assemblyName, String workingDirectory,
		Boolean hybrid)
	{
		String outFile = Path.GetTempFileName();
		ExecuteState<MonoAotArgs> state = new()
		{
			ExecutablePath = monoExecutable,
			ArgState = new() { AssemblyPathName = assemblyName, Hybrid = hybrid, OutputFile = outFile, },
			WorkingDirectory = workingDirectory,
			AppendArgs = MonoAotArgs.Link,
			Notifier = ConsoleNotifier.Notifier,
		};
		String result = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
		if (File.Exists(outFile)) File.Delete(outFile);
		return result;
	}
	private static async Task<String> RunMonoLink(String linkerExecutable, String executableName,
		String outputDirectory)
	{
		ExecuteState<MonoLinkArgs> state = new()
		{
			ExecutablePath = linkerExecutable,
			ArgState = new() { ExecutableName = executableName, OutputPath = outputDirectory, },
			AppendArgs = MonoLinkArgs.Link,
			Notifier = ConsoleNotifier.Notifier,
		};
		String result = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
		return result;
	}
}