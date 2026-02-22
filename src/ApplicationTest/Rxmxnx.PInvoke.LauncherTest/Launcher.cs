namespace Rxmxnx.PInvoke.ApplicationTest;

public abstract partial class Launcher
{
	public DirectoryInfo OutputDirectory { get; }
	public DirectoryInfo? MonoOutputDirectory { get; }
	public Architecture CurrentArch { get; }

	public virtual NetVersion[] NetVersions => Enum.GetValues<NetVersion>();

	public abstract ReadOnlySpan<MonoLauncher> MonoLaunchers { get; }
	public abstract String RuntimeIdentifierPrefix { get; }
	public abstract Architecture[] Architectures { get; }
	public abstract ICppCompiler? GetCompiler(Architecture arch);
	public virtual Task<String?> GetZlibPath() => Task.FromResult<String?>(default);

	public async Task Execute()
	{
		ConsoleNotifier.ShowDiskUsage();

		Dictionary<String, Int32> results = new();
		try
		{
			foreach (Architecture arch in this.Architectures)
			foreach (FileInfo appFile in this.OutputDirectory.GetFiles(
				         $"*ApplicationTest.*{this.RuntimeIdentifierPrefix}-{Enum.GetName(arch)!.ToLower()}.*"))
			{
				String executionName = $"{Path.GetRelativePath(this.OutputDirectory.FullName, appFile.FullName)}";
				results.Add(executionName, await this.RunAppFile(appFile, arch, executionName));
			}
			if (this.MonoOutputDirectory is null || this.MonoLaunchers.IsEmpty) return;
			foreach (MonoLauncher monoLauncher in this.MonoLaunchers.ToArray())
			foreach (FileInfo executableFile in Launcher.GetMonoExecutables(this.MonoOutputDirectory))
			{
				String executionName = $"{executableFile.Name} - {monoLauncher.Architecture}";
				String executableName =
					Path.GetRelativePath(this.MonoOutputDirectory.FullName, executableFile.FullName);
				Int32 result = await Launcher.RunMonoAppFile(monoLauncher.ExecutablePath, executableName,
				                                             this.MonoOutputDirectory.FullName);
				results.Add(executionName, result);
			}
		}
		finally
		{
			if (results.Count > 0)
				ConsoleNotifier.Results(results);
		}
	}
	public async Task CompileMonoBundle()
	{
		ConsoleNotifier.ShowDiskUsage();
		if (this.MonoOutputDirectory is null || this.MonoLaunchers.IsEmpty) return;
		String? zlibPath = await this.GetZlibPath();
		foreach (MonoLauncher monoLauncher in this.MonoLaunchers.ToArray())
		{
			ICppCompiler? cppCompiler = this.GetCompiler(monoLauncher.Architecture);
			foreach (FileInfo executableFile in Launcher.GetMonoExecutables(this.MonoOutputDirectory))
			{
				await this.CompileMonoAotAssembly(monoLauncher, executableFile);
				await Launcher.PackMonoApp(monoLauncher, cppCompiler, zlibPath, this.MonoOutputDirectory,
				                           executableFile);
			}
		}
	}

	protected virtual async Task<Int32> RunAppFile(FileInfo appFile, Architecture arch, String executionName,
		CancellationToken cancellationToken)
	{
		ExecuteState state = new()
		{
			ExecutablePath = appFile.FullName,
			WorkingDirectory = this.OutputDirectory.FullName,
			Notifier = ConsoleNotifier.Notifier,
		};
		Int32 result = await Utilities.Execute(state, cancellationToken);
		ConsoleNotifier.Notifier.Result(result, executionName);
		return result;
	}
	public async Task ExecuteMonoBundle()
	{
		if (this.MonoOutputDirectory is null) return;
		Dictionary<String, Int32> results = new();
		try
		{
			foreach (Architecture arch in this.Architectures)
			foreach (FileInfo appFile in this.MonoOutputDirectory.GetDirectories($"{arch}")
			                                 .SelectMany(d => d.GetFiles("*ApplicationTest*",
			                                                             SearchOption.AllDirectories)))
			{
				String executionName = $"{Path.GetRelativePath(this.OutputDirectory.FullName, appFile.FullName)}";
				results.Add(executionName, await this.RunAppFile(appFile, arch, executionName));
			}
		}
		finally
		{
			if (results.Count > 0)
				ConsoleNotifier.Results(results);
		}
	}
}