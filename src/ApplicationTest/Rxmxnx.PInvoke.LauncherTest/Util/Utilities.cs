namespace Rxmxnx.PInvoke.ApplicationTest.Util;

public static class Utilities
{
	public static Boolean IsNativeAotSupported(Architecture arch, NetVersion netVersion)
		=> netVersion switch
		{
			NetVersion.Net70 => arch is Architecture.X64 && (OperatingSystem.IsWindows() || OperatingSystem.IsLinux()),
			NetVersion.Net80 => arch is not (Architecture.X86 or Architecture.Arm or Architecture.Armv6),
			>= NetVersion.Net90 => true,
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
			lock (state.Lock)
				state.Builder.AppendLine(line);
		}
	}

	private readonly struct OutputState
	{
		public StringBuilder Builder { get; init; }
		public Object Lock { get; init; }
		public CancellationToken CancellationToken { get; init; }
	}
}