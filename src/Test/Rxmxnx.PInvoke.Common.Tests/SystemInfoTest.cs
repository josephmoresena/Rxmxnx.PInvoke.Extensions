namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class SystemInfoTest
{
	[Fact]
	public static void Test()
	{
		PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), SystemInfo.IsWindows);
		PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.Linux), SystemInfo.IsLinux);
		PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.OSX), SystemInfo.IsMac);
#if NETCOREAPP
		PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD), SystemInfo.IsFreeBsd);
#else
		PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.Create("FREEBSD")), SystemInfo.IsFreeBsd);
#endif
		PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(OSPlatform.Create("NETBSD")), SystemInfo.IsNetBsd);
		PInvokeAssert.Equal(
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("SOLARIS")) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("ILLUMOS")), SystemInfo.IsSolaris);
		OSPlatform[] platforms = [OSPlatform.Windows, OSPlatform.Linux, OSPlatform.OSX,];
		String?[] platformNames = platforms.Select(p =>
		{
			String result = p.ToString();
			PInvokeAssert.Equal(RuntimeInformation.IsOSPlatform(p), SystemInfo.IsOsPlatform(result));
			return result;
		}).ToArray();
		PInvokeAssert.Equal(platforms.Any(RuntimeInformation.IsOSPlatform), SystemInfo.IsOsPlatform(platformNames));
		PInvokeAssert.Equal(platforms.Any(RuntimeInformation.IsOSPlatform),
		                    SystemInfo.IsOsPlatform(platformNames.AsSpan()));
	}
}