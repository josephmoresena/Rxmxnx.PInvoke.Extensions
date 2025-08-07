namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the execution platform.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1121)]
#endif
public static partial class SystemInfo
{
	/// <summary>
	/// Indicates whether the current execution is occurring on a Windows-compatible platform.
	/// </summary>
	public static Boolean IsWindows
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsWindows();
#else
		=> SystemInfo.isWindows;
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on a Linux-compatible platform.
	/// </summary>
	public static Boolean IsLinux
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsLinux() || OperatingSystem.IsAndroid();
#else
		=> SystemInfo.isLinux;
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on a macOS-compatible platform.
	/// </summary>
	public static Boolean IsMac
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsMacOS() || OperatingSystem.IsIOS() || OperatingSystem.IsTvOS() ||
#if NET6_0_OR_GREATER
			OperatingSystem.IsMacCatalyst();
#else
			!SystemInfo.NotTrimmable &&
			(SystemInfo.isMacCatalyst ??= SystemInfo.IsOsPlatform(SystemInfo.macCatalystPlatform));
#endif
#else
		=> SystemInfo.isMac;
#endif

	/// <summary>
	/// Indicates whether the current execution is occurring on a Web engine.
	/// </summary>
	public static Boolean IsWebEngine
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsBrowser() ||
#if NET8_0_OR_GREATER
			OperatingSystem.IsWasi();
#else
			!SystemInfo.NotTrimmable && (SystemInfo.isWasi ??= SystemInfo.IsOsPlatform(SystemInfo.wPlatform));
#endif
#else
		=> SystemInfo.isWeb;
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on FreeBSD platform.
	/// </summary>
	public static Boolean IsFreeBsd
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsFreeBSD();
#else
		=> SystemInfo.isFreeBsd;
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on NetBSD platform.
	/// </summary>
	public static Boolean IsNetBsd
		=> !SystemInfo.NotTrimmable && (SystemInfo.isNetBsd ??= SystemInfo.IsOsPlatform(SystemInfo.netBsdPlatform));
	/// <summary>
	/// Indicates whether the current execution is occurring on NetBSD platform.
	/// </summary>
	public static Boolean IsSolaris
		=> !SystemInfo.NotTrimmable && (SystemInfo.isSolaris ??=
			SystemInfo.IsOsPlatform(SystemInfo.solarisPlatform, SystemInfo.illumosPlatform));

	/// <summary>
	/// Indicates whether the current application is running on one of the specified platforms.
	/// </summary>
	/// <param name="platforms">Case-insensitive platform names.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsOsPlatform(
#if !NET9_0_OR_GREATER
		params String[] platforms
#else
		String[] platforms
#endif
	)
		=> SystemInfo.IsOsPlatform(platforms.AsSpan());
	/// <summary>
	/// Indicates whether the current application is running on one of the specified platforms.
	/// </summary>
	/// <param name="platforms">Case-insensitive platform names.</param>
	public static Boolean IsOsPlatform(
#if NET9_0_OR_GREATER
		params ReadOnlySpan<String> platforms
#else
		ReadOnlySpan<String> platforms
#endif
	)
	{
		foreach (String platform in platforms)
		{
			if (SystemInfo.IsOsPlatform(platform))
				return true;
		}
		return false;
	}
	/// <summary>
	/// Indicates whether the current application is running on the specified platform.
	/// </summary>
	/// <param name="platform">Platform name.</param>
	public static Boolean IsOsPlatform(String platform)
#if !NET5_0_OR_GREATER
		=> RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform));
#else
#pragma warning disable CA1418
		=> OperatingSystem.IsOSPlatform(platform);
#pragma warning restore CA1418
#endif
}