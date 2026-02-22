namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private static async Task MakeMonoBundle(MonoLauncher monoLauncher, DirectoryInfo outputPath,
		String applicationName, FileInfo linkedExecutableFile)
	{
		DirectoryInfo binaryOutputPath = Launcher.GetMonoBundleOutputPath(outputPath, monoLauncher.Architecture, false);
		String bundleLog = Path.Combine(outputPath.FullName,
		                                $"{applicationName}.{monoLauncher.Architecture}.Mono.Bundle.log");
		String binaryName = Launcher.GetMonoBundleName(applicationName, false);
		String outputBinaryPath = Path.Combine(binaryOutputPath.FullName, binaryName);
		String workingDirectory = Launcher.GetMakerWorkingDirectory(linkedExecutableFile, false);
		ExecuteState<MonoBundleArgs> state = new()
		{
			ExecutablePath = monoLauncher.MakerPath,
			ArgState = new()
			{
				Architecture = monoLauncher.Architecture,
				AssemblyPathName = linkedExecutableFile.FullName,
				MonoExecutablePath = monoLauncher.ExecutablePath,
				StripAssemblyPath = monoLauncher.MonoCilStripAssemblyPath,
				OutputPath = outputBinaryPath,
				UseLlvm = false,
			},
			WorkingDirectory = workingDirectory,
			AppendEnvs = MonoBundleArgs.MakeEnv,
			AppendArgs = MonoBundleArgs.Make,
			Notifier = ConsoleNotifier.Notifier,
		};
		String bundleResult = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
		await Utilities.SaveTextFile(bundleLog, bundleResult);
		MonoBundleSource.DeleteAotFiles(workingDirectory);

		if (monoLauncher.Architecture is not Architecture.X86)
		{
			binaryName = Launcher.GetMonoBundleName(applicationName, true);
			state = state with
			{
				ArgState = state.ArgState with
				{
					UseLlvm = true, OutputPath = Path.Combine(binaryOutputPath.FullName, binaryName),
				},
			};
			bundleLog = Path.Combine(outputPath.FullName,
			                         $"{applicationName}.{monoLauncher.Architecture}.Mono.Bundle.Llvm.log");
			bundleResult = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
			await Utilities.SaveTextFile(bundleLog, bundleResult);
			MonoBundleSource.DeleteAotFiles(workingDirectory);
		}

		if (binaryOutputPath.GetFiles().Length > 0)
			Launcher.CopyMonoRuntime(monoLauncher, binaryOutputPath);

		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
	}
	private static async Task MakeMonoAotBundle(MonoLauncher monoLauncher, ICppCompiler cppCompiler, String? zLibPath,
		Boolean isWindowsApp, DirectoryInfo outputPath, String applicationName, FileInfo linkedExecutableFile)
	{
		const String sourceFile = "main.c";
		const String assembliesPath = "temp_assemblies";

		String objectFile = OperatingSystem.IsWindows() ? "assemblies.obj" : "assemblies.o";
		String bundleLog = Path.Combine(outputPath.FullName,
		                                $"{applicationName}.{monoLauncher.Architecture}.Mono.Bundle.AOT.log");
		String workingDirectory = Launcher.GetMakerWorkingDirectory(linkedExecutableFile, true);
		ExecuteState<MonoBundleArgs> state = new()
		{
			ExecutablePath = monoLauncher.MakerPath,
			ArgState = new()
			{
				Architecture = monoLauncher.Architecture,
				AssemblyPathName = linkedExecutableFile.FullName,
				MonoExecutablePath = monoLauncher.ExecutablePath,
				CustomBuild = true,
				StripAssemblyPath = monoLauncher.MonoCilStripAssemblyPath,
				OutputPath = sourceFile,
				OutputObjectPath = objectFile,
				UseLlvm = false,
			},
			WorkingDirectory = workingDirectory,
			AppendEnvs = MonoBundleArgs.MakeEnv,
			AppendArgs = MonoBundleArgs.Make,
			Notifier = ConsoleNotifier.Notifier,
		};
		String bundleResult = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
		await Utilities.SaveTextFile(bundleLog, bundleResult);

		using MonoBundleSource source = new(workingDirectory, sourceFile, objectFile, assembliesPath);
		if (!source.Exists) return;

		String[] monoArgs = await cppCompiler.GetPkgConfigArgs("mono-2", monoLauncher.PkgConfigPath);
		String binaryName = Launcher.GetMonoBundleName(applicationName, false);
		DirectoryInfo binaryOutputPath = Launcher.GetMonoBundleOutputPath(outputPath, monoLauncher.Architecture, true);
		String outputBinaryPath = Path.Combine(binaryOutputPath.FullName, binaryName);
		ExecuteState<MonoNativeCompilationArgs> compState = new()
		{
			ExecutablePath = cppCompiler.CompilerExecutable,
			ArgState = new()
			{
				Compiler = cppCompiler,
				MonoFlags = monoArgs,
				MonoIncludePath = monoArgs.Length == 0 ? monoLauncher.IncludeRuntimePath : default,
				StaticRuntimePath = monoLauncher.StaticRuntimePath,
				AotFiles = source.AotFiles.Select(af => af.FullName),
				ObjectFile = source.ObjectFile.FullName,
				SourceFile = source.SourceFile.FullName,
				WindowApp = isWindowsApp,
				ZLibPath = zLibPath,
				BinaryOutputPath = outputBinaryPath,
				Environment = await cppCompiler.GetEnv(),
#if ZLINK_STATIC
				Architecture = monoLauncher.Architecture,
#endif
			},
			AppendEnvs = MonoNativeCompilationArgs.CompileEnv,
			AppendArgs = MonoNativeCompilationArgs.Compile,
			Notifier = ConsoleNotifier.Notifier,
		};

		await Utilities.PatchMonoBundleSource(source.SourceFile.FullName);
		bundleLog = Path.Combine(outputPath.FullName,
		                         $"{applicationName}.{monoLauncher.Architecture}.Mono.Bundle.Cpp.log");
		bundleResult = await Utilities.ExecuteWithOutput(compState, ConsoleNotifier.CancellationToken);
		await Utilities.SaveTextFile(bundleLog, bundleResult);

		if (!OperatingSystem.IsWindows() && Path.Exists(outputBinaryPath))
		{
			await Launcher.StripUnixExecutable(outputBinaryPath);
			Launcher.CopyMonoRuntime(monoLauncher, binaryOutputPath);
		}
		if (Utilities.ShowDiagnostics)
			ConsoleNotifier.ShowDiskUsage();
	}
	private static String GetMakerWorkingDirectory(FileInfo linkedExecutableFile, Boolean isAot)
		=> OperatingSystem.IsWindows() || !isAot ? linkedExecutableFile.DirectoryName ?? "" : Path.GetTempPath();
	private static void CopyMonoRuntime(MonoLauncher monoLauncher, DirectoryInfo binaryOutputPath)
	{
		FileInfo nativeRuntime = new(monoLauncher.NativeRuntimePath);
		String nativeRuntimePath = Path.Combine(binaryOutputPath.FullName, monoLauncher.NativeRuntimeName);
		if (nativeRuntime.Exists && !File.Exists(nativeRuntimePath))
			nativeRuntime.CopyTo(nativeRuntimePath, true);
	}
	private static Boolean IsWindowApp(FileInfo linkedExecutableFile)
	{
		using FileStream stream = linkedExecutableFile.OpenRead();
		using PEReader peReader = new(stream);
		Boolean isWindowApp = !peReader.PEHeaders.IsConsoleApplication;
		return isWindowApp;
	}
	private static void DeleteBundleTempFiles()
	{
		DirectoryInfo temp = new(Path.GetTempPath());
		foreach (FileInfo aotFile in temp.EnumerateFiles("mono_aot_*", MonoBundleSource.SafeEnumerationOptions))
			aotFile.Delete();
	}
	private async Task CompileMonoAotAssembly(MonoLauncher monoLauncher, FileInfo executableFile)
	{
		await Launcher.CompileMonoAot(monoLauncher, this.MonoOutputDirectory!.FullName, executableFile);
		foreach (FileInfo assemblyFile in executableFile.Directory!.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
			await Launcher.CompileMonoAot(monoLauncher, this.MonoOutputDirectory.FullName, assemblyFile);
	}
	private static async Task StripUnixExecutable(String outputBinaryPath)
	{
		ExecuteState<String> stripState = new()
		{
			ExecutablePath = "strip",
			ArgState = outputBinaryPath,
			AppendArgs = (s, c) =>
			{
				c.Add("-x");
				c.Add(s);
			},
			Notifier = ConsoleNotifier.Notifier,
		};
		await Utilities.Execute(stripState, ConsoleNotifier.CancellationToken);
	}
	private static DirectoryInfo GetMonoBundleOutputPath(DirectoryInfo outputPath, Architecture arch, Boolean isAot)
		=> outputPath.CreateSubdirectory(Path.Combine($"{arch}", !isAot ? "Bundle" : ""));
	private static String GetMonoBundleName(String applicationName, Boolean isLlvm)
	{
		String complement =
			$"{(!applicationName.Contains(".mono", StringComparison.InvariantCultureIgnoreCase) ? ".mono" : "")}";
		String binaryExtension = OperatingSystem.IsWindows() ? ".exe" : "";
		return $"{applicationName}{complement}{(isLlvm ? ".llvm" : "")}{binaryExtension}";
	}
}