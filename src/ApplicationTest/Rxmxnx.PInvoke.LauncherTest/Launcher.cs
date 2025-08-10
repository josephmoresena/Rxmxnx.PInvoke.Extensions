namespace Rxmxnx.PInvoke.ApplicationTest;

public abstract partial class Launcher
{
	public DirectoryInfo OutputDirectory { get; }
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
				         $"ApplicationTest.*.{this.RuntimeIdentifierPrefix}-{Enum.GetName(arch)!.ToLower()}.*"))
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