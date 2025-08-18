namespace Rxmxnx.PInvoke.ApplicationTest.Util;

public interface IPlatformNotifier
{
	void BeginDetection();
	void EndDetection(OSPlatform platform, Architecture arch);
	void Initialization(OSPlatform platform, Architecture arch);
}