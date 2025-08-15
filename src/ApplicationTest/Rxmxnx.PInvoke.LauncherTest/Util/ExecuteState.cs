namespace Rxmxnx.PInvoke.ApplicationTest.Util;

public readonly struct ExecuteState
{
	public String ExecutablePath { get; init; }
	public String? WorkingDirectory { get; init; }
	public Action<Collection<String>>? AppendArgs { get; init; }
	public Action<StringDictionary>? AppendEnvs { get; init; }
	public IExecutionNotifier? Notifier { get; init; }
}

public readonly struct ExecuteState<TState>
{
	public String ExecutablePath { get; init; }
	public String? WorkingDirectory { get; init; }
	public TState ArgState { get; init; }
	public Action<TState, Collection<String>>? AppendArgs { get; init; }
	public Action<TState, StringDictionary>? AppendEnvs { get; init; }
	public IExecutionNotifier? Notifier { get; init; }
}