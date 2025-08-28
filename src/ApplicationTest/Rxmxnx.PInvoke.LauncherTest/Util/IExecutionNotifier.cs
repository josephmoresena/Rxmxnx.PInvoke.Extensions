namespace Rxmxnx.PInvoke.ApplicationTest.Util;

public interface IExecutionNotifier
{
	Int32 RefreshTime { get; }
	void Begin(ProcessStartInfo info);
	void End(ProcessStartInfo info);
	void Print(String message, Boolean done = false);
	void PrintError(String message, Exception? ex);
	void Result(Int32 result, String executionName);
}