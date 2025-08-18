namespace Rxmxnx.PInvoke.ApplicationTest.Util;

public interface IExecutionNotifier
{
	Int32 RefreshTime { get; }
	void Begin(ProcessStartInfo info);
	void End(ProcessStartInfo info);
	void Result(Int32 result, String executionName);
}