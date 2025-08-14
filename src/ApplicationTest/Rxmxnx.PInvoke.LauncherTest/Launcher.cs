namespace Rxmxnx.PInvoke.ApplicationTest;

public abstract partial class Launcher
{
	public DirectoryInfo OutputDirectory { get; }
	public DirectoryInfo MonoOutputDirectory { get; }
	public Architecture CurrentArch { get; }
	public abstract String RuntimeIdentifierPrefix { get; }
	public abstract String MonoMsbuildPath { get; }
	public abstract String MonoExecutablePath { get; }
	public abstract String MonoFrameworkPath { get; }

	public abstract Architecture[] Architectures { get; }
	public async Task Execute()
	{
		Dictionary<String, Int32> results = new();

		try
		{
			foreach (Architecture arch in this.Architectures)
			foreach (FileInfo appFile in this.OutputDirectory.GetFiles(
				         $"*ApplicationTest.*.{this.RuntimeIdentifierPrefix}-{Enum.GetName(arch)!.ToLower()}.*"))
			{
				String executionName = $"{Path.GetRelativePath(this.OutputDirectory.FullName, appFile.FullName)}";
				results.Add(executionName, await this.RunAppFile(appFile, arch, executionName));
			}
			foreach (FileInfo appFile in this.MonoOutputDirectory.GetFiles("*ApplicationTest.*.exe"))
			{
				String executionName = $"{Path.GetRelativePath(this.OutputDirectory.FullName, appFile.FullName)}";
				ExecuteState<String> state = new()
				{
					ExecutablePath = this.MonoExecutablePath,
					ArgState = executionName,
					WorkingDirectory = this.MonoOutputDirectory.FullName,
					AppendArgs = static (s, c) => _ = c.Append(s),
					Notifier = ConsoleNotifier.Notifier,
				};
				Int32 result = await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
				ConsoleNotifier.Notifier.Result(result, executionName);
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
		foreach (FileInfo assemblyFile in this.OutputDirectory.GetFiles())
		{
			if (!assemblyFile.Extension.Contains(".exe") && !assemblyFile.Extension.Contains(".dll")) continue;
			String assemblyName = $"{Path.GetRelativePath(this.MonoOutputDirectory.FullName, assemblyFile.FullName)}";
			ExecuteState<String> state = new()
			{
				ExecutablePath = this.MonoExecutablePath,
				ArgState = assemblyName,
				WorkingDirectory = this.MonoOutputDirectory.FullName,
				AppendArgs = static (s, c) =>
				{
					_ = c.Append("--aot=full,hybrid");
					_ = c.Append("-O=all");
					_ = c.Append(s);
				},
				Notifier = ConsoleNotifier.Notifier,
			};
			String result = await Utilities.ExecuteWithOutput(state);
			await File.WriteAllTextAsync($"{assemblyFile.Name}.Mono.AOT.log", result);
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