namespace Rxmxnx.PInvoke.ApplicationTest.Util;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public readonly struct QemuExecuteState
{
	public String QemuExecutable { get; init; }
	public String QemuRoot { get; init; }
	public String ExecutablePath { get; init; }
	public String? WorkingDirectory { get; init; }
	public Action<Collection<String>>? AppendArgs { get; init; }
	public Action<StringDictionary>? AppendEnvs { get; init; }
	public IExecutionNotifier? Notifier { get; init; }
}