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
		String aotLog = Path.Combine(outputDirectory, $"{assemblyFile.Name}.{monoLauncher.Architecture}.Mono.AOT.log");
		String result = await Launcher.RunMonoAot(monoLauncher.ExecutablePath, assemblyFile.Name,
		                                          assemblyFile.DirectoryName ?? String.Empty);
		await File.WriteAllTextAsync(aotLog, result);
	}
	private static async Task PackMonoApp(MonoLauncher monoLauncher, DirectoryInfo outputPath, FileInfo executableFile)
	{
		String applicationName = executableFile.Directory?.Name ?? executableFile.Name;
		FileInfo? linkedExecutableFile =
			await Launcher.LinkMonoApp(monoLauncher, outputPath, applicationName, executableFile);
		if (linkedExecutableFile is not null)
			await Launcher.MakeMonoBundle(monoLauncher, outputPath, applicationName, linkedExecutableFile);
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
		await File.WriteAllTextAsync(linkLog, linkResult);
		return linkOutputDirectory.GetFiles(assemblyExecutableFile.Name).FirstOrDefault();
	}
	private static async Task MakeMonoBundle(MonoLauncher monoLauncher, DirectoryInfo outputPath,
		String applicationName, FileInfo linkedExecutableFile)
	{
		DirectoryInfo binaryOutputPath = outputPath.CreateSubdirectory($"{monoLauncher.Architecture}");
		String binaryExtension = OperatingSystem.IsWindows() ? ".exe" : "";
		String binaryName = $"{applicationName}{binaryExtension}";
		String outputBinaryPath = Path.Combine(binaryOutputPath.FullName, binaryName);
		ExecuteState<MonoBundleArgs> state = new()
		{
			ExecutablePath = monoLauncher.MakerPath,
			ArgState = new()
			{
				AssemblyPathName = linkedExecutableFile.FullName,
				MonoExecutablePath = monoLauncher.ExecutablePath,
				StripAssemblyPath = monoLauncher.MonoCilStripAssemblyPath,
				OutputBinaryPath = outputBinaryPath,
			},
			WorkingDirectory = linkedExecutableFile.DirectoryName ?? "",
			AppendArgs = MonoBundleArgs.Make,
			Notifier = ConsoleNotifier.Notifier,
		};
		Int32 result = await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
		ConsoleNotifier.Notifier.Result(result, $"Bundle {applicationName} - {monoLauncher.Architecture}");
		if (result == 0 && File.Exists(monoLauncher.GarbageCollectorPath))
		{
			FileInfo gcPath = new(monoLauncher.GarbageCollectorPath);
			String appGcPath = Path.Combine(binaryOutputPath.FullName, gcPath.Name);
			if (!File.Exists(appGcPath))
				gcPath.CopyTo(appGcPath);
		}
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
	private static async Task<String> RunMonoAot(String monoExecutable, String assemblyName, String workingDirectory)
	{
		ExecuteState<String> state = new()
		{
			ExecutablePath = monoExecutable,
			ArgState = assemblyName,
			WorkingDirectory = workingDirectory,
			AppendArgs = static (s, c) =>
			{
				c.Add("--aot=full,nodebug");
				c.Add("--verbose");
				c.Add("-O=all");
				c.Add(s);
			},
			Notifier = ConsoleNotifier.Notifier,
		};
		String result = await Utilities.ExecuteWithOutput(state);
		return result;
	}
	private static async Task<String> RunMonoLink(String linkerExecutable, String executableName,
		String outputDirectory)
	{
		ExecuteState<LinkMonoArgs> state = new()
		{
			ExecutablePath = linkerExecutable,
			ArgState = new() { ExecutableName = executableName, OutputPath = outputDirectory, },
			AppendArgs = LinkMonoArgs.Link,
			Notifier = ConsoleNotifier.Notifier,
		};
		String result = await Utilities.ExecuteWithOutput(state);
		return result;
	}
}