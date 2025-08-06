#if !NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public static partial class SystemInfo
{
	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static SystemInfo()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			SystemInfo.isWindows = true;
			return;
		}
#if NETCOREAPP
		if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
#else
		if (SystemInfo.IsOsPlatform(SystemInfo.freePlatform))
#endif
		{
			SystemInfo.isFreeBsd = true;
			return;
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || SystemInfo.IsOsPlatform(SystemInfo.androidPlatform))
		{
			SystemInfo.isLinux = true;
			return;
		}
		if (SystemInfo.IsOsPlatform([SystemInfo.browserPlatform, SystemInfo.wPlatform,]))
		{
			SystemInfo.isWeb = true;
			return;
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
		    SystemInfo.IsOsPlatform([SystemInfo.iPlatform, SystemInfo.tPlatform, SystemInfo.macCatalystPlatform,]))
		{
			SystemInfo.isMac = true;
		}
	}
}
#endif