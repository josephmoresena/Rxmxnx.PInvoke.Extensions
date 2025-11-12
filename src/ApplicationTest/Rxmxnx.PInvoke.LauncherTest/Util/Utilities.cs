namespace Rxmxnx.PInvoke.ApplicationTest.Util;

public static class Utilities
{
	public static Boolean ShowDiagnostics
		=> Boolean.TryParse(Environment.GetEnvironmentVariable("SHOW_DIAGNOSTICS"), out Boolean showDiagnostics) &&
			showDiagnostics;
	public static Boolean IsNativeAotSupported(Architecture arch, NetVersion netVersion)
		=> netVersion >= NetVersion.Net60 && arch switch
		{
			Architecture.X64 => true,
			Architecture.Arm64 => netVersion >= NetVersion.Net80 || OperatingSystem.IsWindows() ||
				OperatingSystem.IsLinux(),
			Architecture.X86 or Architecture.Arm or Architecture.Armv6 => netVersion >= NetVersion.Net90,
			_ => false,
		};
	public static async Task<Int32> Execute(ExecuteState state, CancellationToken cancellationToken = default)
	{
		ProcessStartInfo info = new(state.ExecutablePath)
		{
			RedirectStandardError = false, RedirectStandardOutput = false, CreateNoWindow = false,
		};
		state.AppendArgs?.Invoke(info.ArgumentList);
		state.AppendEnvs?.Invoke(info.EnvironmentVariables);
		if (!String.IsNullOrEmpty(state.WorkingDirectory)) info.WorkingDirectory = state.WorkingDirectory;
		state.Notifier?.Begin(info);
		using Process prog = Process.Start(info)!;
		await prog.WaitForExitAsync(cancellationToken);
		state.Notifier?.End(info);
		return prog.ExitCode;
	}
	public static async Task<Int32> Execute<TState>(ExecuteState<TState> state,
		CancellationToken cancellationToken = default)
	{
		ProcessStartInfo info = new(state.ExecutablePath)
		{
			RedirectStandardError = false, RedirectStandardOutput = false, CreateNoWindow = false,
		};
		state.AppendArgs?.Invoke(state.ArgState, info.ArgumentList);
		if (!String.IsNullOrEmpty(state.WorkingDirectory)) info.WorkingDirectory = state.WorkingDirectory;
		state.Notifier?.Begin(info);
		using Process prog = Process.Start(info)!;
		await prog.WaitForExitAsync(cancellationToken);
		state.Notifier?.End(info);
		return prog.ExitCode;
	}
	public static async Task<String> ExecuteWithOutput(ExecuteState state,
		CancellationToken cancellationToken = default)
	{
		ProcessStartInfo info = new(state.ExecutablePath)
		{
			RedirectStandardError = true, RedirectStandardOutput = true, CreateNoWindow = true,
		};
		state.AppendArgs?.Invoke(info.ArgumentList);
		state.AppendEnvs?.Invoke(info.EnvironmentVariables);
		if (!String.IsNullOrEmpty(state.WorkingDirectory)) info.WorkingDirectory = state.WorkingDirectory;
		state.Notifier?.Begin(info);
		using Process prog = Process.Start(info)!;
		String result = await Utilities.ReadOutput(prog, cancellationToken);
		await prog.WaitForExitAsync(cancellationToken);
		state.Notifier?.End(info);
		return result;
	}
	public static async Task<String> ExecuteWithOutput<TState>(ExecuteState<TState> state,
		CancellationToken cancellationToken = default)
	{
		ProcessStartInfo info = new(state.ExecutablePath)
		{
			RedirectStandardError = true, RedirectStandardOutput = true, CreateNoWindow = true,
		};
		state.AppendArgs?.Invoke(state.ArgState, info.ArgumentList);
		state.AppendEnvs?.Invoke(state.ArgState, info.EnvironmentVariables);
		if (!String.IsNullOrEmpty(state.WorkingDirectory)) info.WorkingDirectory = state.WorkingDirectory;
		state.Notifier?.Begin(info);
		using Process prog = Process.Start(info)!;
		String result = await Utilities.ReadOutput(prog, cancellationToken);
		await prog.WaitForExitAsync(cancellationToken);
		state.Notifier?.End(info);
		return result;
	}
	public static async Task<Int32> QemuExecute(QemuExecuteState state, CancellationToken cancellationToken = default)
	{
		ProcessStartInfo info = new(state.QemuExecutable)
		{
			ArgumentList = { "-L", state.QemuRoot, state.ExecutablePath, },
			RedirectStandardError = false,
			RedirectStandardOutput = false,
			CreateNoWindow = false,
		};
		state.AppendArgs?.Invoke(info.ArgumentList);
		state.AppendEnvs?.Invoke(info.EnvironmentVariables);
		if (!String.IsNullOrEmpty(state.WorkingDirectory)) info.WorkingDirectory = state.WorkingDirectory;
		state.Notifier?.Begin(info);
		using Process prog = Process.Start(info)!;
		await prog.WaitForExitAsync(cancellationToken);
		state.Notifier?.End(info);
		return prog.ExitCode;
	}

	private static async Task<String> ReadOutput(Process prog, CancellationToken cancellationToken)
	{
		OutputState state = new() { Builder = new(), Lock = new(), CancellationToken = cancellationToken, };
		await Task.WhenAll(Utilities.CopyOutput(state, prog.StandardOutput),
		                   Utilities.CopyOutput(state, prog.StandardError));
		return state.Builder.ToString();
	}
	private static async Task CopyOutput(OutputState state, StreamReader reader)
	{
		while (await reader.ReadLineAsync(state.CancellationToken) is { } line)
		{
			using (state.Lock.EnterScope())
				state.Builder.AppendLine(line);
		}
	}

	private readonly struct OutputState
	{
		public StringBuilder Builder { get; init; }
		public Lock Lock { get; init; }
		public CancellationToken CancellationToken { get; init; }
	}
}