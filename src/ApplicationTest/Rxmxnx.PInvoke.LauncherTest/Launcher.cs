namespace Rxmxnx.PInvoke.ApplicationTest;

public abstract partial class Launcher
{
	public DirectoryInfo OutputDirectory { get; }
	public DirectoryInfo? MonoOutputDirectory { get; }
	public Architecture CurrentArch { get; }

	public abstract ReadOnlySpan<MonoLauncher> MonoLaunchers { get; }
	public abstract String RuntimeIdentifierPrefix { get; }
	public abstract Architecture[] Architectures { get; }

	public async Task Execute()
	{
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
	public async Task CompileMonoAot()
	{
		if (this.MonoOutputDirectory is null || this.MonoLaunchers.IsEmpty) return;
		foreach (MonoLauncher monoLauncher in this.MonoLaunchers.ToArray())
		foreach (FileInfo executableFile in Launcher.GetMonoExecutables(this.MonoOutputDirectory))
		{
			await Launcher.CompileMonoAot(monoLauncher, this.MonoOutputDirectory.FullName, executableFile);
			foreach (FileInfo assemblyFile in
			         executableFile.Directory!.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
				await Launcher.CompileMonoAot(monoLauncher, this.MonoOutputDirectory.FullName, assemblyFile);
			await Launcher.MakeMonoBundle(monoLauncher, this.MonoOutputDirectory, executableFile);
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
}