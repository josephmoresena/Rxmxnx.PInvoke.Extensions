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

		await File.WriteAllTextAsync(logPath, result);
		logPath = Path.Combine(outputDirectory, $"{assemblyFile.Name}.{monoLauncher.Architecture}.Mono.AOT.Hybrid.log");
		result = await Launcher.RunMonoAot(monoLauncher.ExecutablePath, assemblyFile.Name, outputPath, true);
		await File.WriteAllTextAsync(logPath, result);
		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
	}
	private static async Task PackMonoApp(MonoLauncher monoLauncher, DirectoryInfo outputPath, FileInfo executableFile)
	{
		String applicationName = executableFile.Directory?.Name ?? executableFile.Name;
		FileInfo? linkedExecutableFile =
			await Launcher.LinkMonoApp(monoLauncher, outputPath, applicationName, executableFile);
		if (linkedExecutableFile is not null)
			await Launcher.MakeMonoBundle(monoLauncher, outputPath, applicationName, linkedExecutableFile);
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
		await File.WriteAllTextAsync(linkLog, linkResult);
		return linkOutputDirectory.GetFiles(assemblyExecutableFile.Name).FirstOrDefault();
	}
	private static async Task MakeMonoBundle(MonoLauncher monoLauncher, DirectoryInfo outputPath,
		String applicationName, FileInfo linkedExecutableFile)
	{
		DirectoryInfo binaryOutputPath = outputPath.CreateSubdirectory($"{monoLauncher.Architecture}");
		String bundleLog = Path.Combine(outputPath.FullName,
		                                $"{applicationName}.{monoLauncher.Architecture}.Mono.Bundle.log");
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
				UseLlvm = false,
			},
			WorkingDirectory = linkedExecutableFile.DirectoryName ?? "",
			AppendArgs = MonoBundleArgs.Make,
			Notifier = ConsoleNotifier.Notifier,
		};
		String bundleResult = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
		await File.WriteAllTextAsync(bundleLog, bundleResult);

		FileInfo nativeRuntime = new(monoLauncher.NativeRuntimePath);
		state = state with
		{
			ArgState = state.ArgState with
			{
				UseLlvm = true,
				OutputBinaryPath =
				Path.Combine(binaryOutputPath.FullName, $"{applicationName}.llvm{binaryExtension}"),
			},
		};
		bundleLog = Path.Combine(outputPath.FullName,
		                         $"{applicationName}.{monoLauncher.Architecture}.Mono.Bundle.Llvm.log");
		bundleResult = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
		await File.WriteAllTextAsync(bundleLog, bundleResult);

		if (nativeRuntime.Exists && binaryOutputPath.GetFiles().Length > 0)
			nativeRuntime.CopyTo(Path.Combine(binaryOutputPath.FullName, monoLauncher.NativeRuntimeName), true);

		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
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
		ExecuteState<MonoAotArgs> state = new()
		{
			ExecutablePath = monoExecutable,
			ArgState = new() { AssemblyPathName = assemblyName, Hybrid = hybrid, },
			WorkingDirectory = workingDirectory,
			AppendArgs = MonoAotArgs.Link,
			Notifier = ConsoleNotifier.Notifier,
		};
		String result = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
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