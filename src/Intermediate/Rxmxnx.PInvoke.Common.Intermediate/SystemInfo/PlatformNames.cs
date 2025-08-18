namespace Rxmxnx.PInvoke;

public static partial class SystemInfo
{
	private const String solarisPlatform = "SOLARIS";
	private const String illumosPlatform = "ILLUMOS";
	private const String netBsdPlatform = "NETBSD";
#if !NET8_0_OR_GREATER
	private const String wPlatform = "WASI";
#endif
#if !NET6_0_OR_GREATER
	private const String macCatalystPlatform = "MACCATALYST";
#endif
#if !NETCOREAPP
	private const String freePlatform = "FREEBSD";
#endif
#if !NET5_0_OR_GREATER
	private const String androidPlatform = "ANDROID";
	private const String browserPlatform = "BROWSER";
	private const String iPlatform = "IOS";
	private const String tPlatform = "TVOS";
#endif
}