// ReSharper disable ConvertToExtensionBlock

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
	public static async Task PatchMonoBundleSource(String sourceFilePath)
	{
		String text = await File.ReadAllTextAsync(sourceFilePath);
		String patched = text.Replace("#ifndef USE_COMPRESSED_ASSEMBLY", "#ifndef UNDEFINED")
		                     .Replace("mono_mkbundle_init();", "mono_mkbundle_init(); install_aot_modules();");
		await Utilities.SaveTextFile(sourceFilePath, patched);
	}
	public static async Task<String> ReadOutput(Process prog, CancellationToken cancellationToken)
	{
		OutputState state = new() { Builder = new(), Lock = new(), CancellationToken = cancellationToken, };
		await Task.WhenAll(Utilities.CopyOutput(state, prog.StandardOutput),
		                   Utilities.CopyOutput(state, prog.StandardError));
		return state.Builder.ToString();
	}
	public static async Task SaveTextFile(String filePath, String fileContent)
	{
		if (String.IsNullOrWhiteSpace(fileContent)) return;
		await File.WriteAllTextAsync(filePath, fileContent);
	}

	private static async Task CopyOutput(OutputState state, StreamReader reader)
	{
		while (await reader.ReadLineAsync(state.CancellationToken) is { } line)
		{
			using (state.Lock.EnterScope())
				state.Builder.AppendLine(line);
		}
	}
	private static String CombinePathArg(String flag, String path) => $"{flag}{path}";

	private readonly struct OutputState
	{
		public StringBuilder Builder { get; init; }
		public Lock Lock { get; init; }
		public CancellationToken CancellationToken { get; init; }
	}

	#region Extensions
	public static void AddArg(this Collection<String> args, String? argValue)
	{
		if (String.IsNullOrWhiteSpace(argValue)) return;
		args.Add(argValue.Trim());
	}
	public static void AddArgs(this Collection<String> args, IEnumerable<String?> argValues)
	{
		foreach (String? argValue in argValues)
			args.AddArg(argValue);
	}
	public static void AddIncludeArg(this Collection<String> args, String includeFlag, String? argValue)
	{
		if (String.IsNullOrWhiteSpace(argValue)) return;
		if (includeFlag.StartsWith('-'))
		{
			args.Add(Utilities.CombinePathArg(includeFlag.Trim(), argValue.Trim()));
			return;
		}
		args.Add(includeFlag.Trim());
		args.Add(argValue.Trim());
	}
	public static void AddWholeLibArg(this Collection<String> args, String beginWholeLink, String endWholeLink,
		String? argValue, out Boolean added)
	{
		if (String.IsNullOrWhiteSpace(beginWholeLink) || String.IsNullOrWhiteSpace(argValue))
		{
			added = false;
			return;
		}

		added = true;
		if (String.IsNullOrWhiteSpace(endWholeLink))
		{
			args.Add(Utilities.CombinePathArg(beginWholeLink.Trim(), argValue.Trim()));
			return;
		}
		args.Add(beginWholeLink.Trim());
		args.Add(argValue.Trim());
		args.Add(endWholeLink.Trim());
	}
	public static void AddOutputArg(this Collection<String> args, String outputFlag, String? argValue)
	{
		if (String.IsNullOrWhiteSpace(argValue)) return;
		if (!outputFlag.StartsWith('-'))
		{
			args.Add(Utilities.CombinePathArg(outputFlag.Trim(), argValue.Trim()));
			return;
		}
		args.Add(outputFlag.Trim());
		args.Add(argValue.Trim());
	}
	public static void AddLibPathArg(this Collection<String> args, String libPathFlag, String? argValue)
	{
		if (String.IsNullOrWhiteSpace(argValue)) return;
		args.Add(String.IsNullOrWhiteSpace(libPathFlag) ?
			         argValue.Trim() :
			         Utilities.CombinePathArg(libPathFlag.Trim(), argValue.Trim()));
	}
	#endregion
}