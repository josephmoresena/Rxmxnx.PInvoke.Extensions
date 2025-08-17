namespace Rxmxnx.PInvoke.ApplicationTest;

public abstract partial class Launcher
{
	public DirectoryInfo OutputDirectory { get; }
	public DirectoryInfo? MonoOutputDirectory { get; }
	public Architecture CurrentArch { get; }
	public abstract String RuntimeIdentifierPrefix { get; }
	public abstract String MonoMsbuildPath { get; }
	public abstract String MonoExecutablePath { get; }
	public virtual String? Mono2ExecutablePath => default;
	public virtual Architecture Mono2Arch => RuntimeInformation.OSArchitecture;

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
			if (this.MonoOutputDirectory is null) return;
			foreach (FileInfo appFile in this.MonoOutputDirectory.GetFiles("*ApplicationTest.*mono.exe"))
			{
				String executionName = $"{Path.GetRelativePath(this.MonoOutputDirectory.FullName, appFile.FullName)}";
				results.Add(appFile.Name,
				            await Launcher.RunMonoAppFile(this.MonoExecutablePath, executionName,
				                                          this.MonoOutputDirectory.FullName));
				if (String.IsNullOrEmpty(this.Mono2ExecutablePath)) return;
				results.Add($"{appFile.Name} ({this.Mono2Arch})",
				            await Launcher.RunMonoAppFile(this.Mono2ExecutablePath, executionName,
				                                          this.MonoOutputDirectory.FullName));
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
		if (this.MonoOutputDirectory is null) return;
		foreach (FileInfo assemblyFile in this.MonoOutputDirectory.GetFiles())
		{
			if (!assemblyFile.Extension.Contains(".exe") && !assemblyFile.Extension.Contains(".dll")) continue;
			String assemblyName = $"{Path.GetRelativePath(this.MonoOutputDirectory.FullName, assemblyFile.FullName)}";
			String result =
				await Launcher.RunMonoAot(this.MonoExecutablePath, assemblyName, this.MonoOutputDirectory.FullName);
			await File.WriteAllTextAsync($"{assemblyFile.FullName}.Mono.AOT.log", result);
			if (String.IsNullOrEmpty(this.Mono2ExecutablePath)) continue;

			String result2 =
				await Launcher.RunMonoAot(this.Mono2ExecutablePath, assemblyName, this.MonoOutputDirectory.FullName);
			await File.WriteAllTextAsync($"{assemblyFile.FullName}.{this.Mono2Arch}.Mono.AOT.log", result2);
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