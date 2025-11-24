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
	/// Indicates whether the current execution is running on a Windows-compatible platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("windows")]
#endif
	public static Boolean IsWindows
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsWindows();
#else
		=> SystemInfo.isWindows;
#endif
	/// <summary>
	/// Indicates whether the current execution is running on a Linux-compatible platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("linux")]
	[SupportedOSPlatformGuard("android")]
#endif
	public static Boolean IsLinux
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return
#if NET5_0_OR_GREATER
				OperatingSystem.IsLinux() || OperatingSystem.IsAndroid()
#else
				SystemInfo.isLinux
#endif
				;
		}
	}
	/// <summary>
	/// Indicates whether the current execution is running on a macOS-compatible platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("osx")]
	[SupportedOSPlatformGuard("macos")]
	[SupportedOSPlatformGuard("tvos")]
	[SupportedOSPlatformGuard("maccatalyst")]
#endif
	public static Boolean IsMac
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return
#if NET5_0_OR_GREATER
				OperatingSystem.IsMacOS() || OperatingSystem.IsIOS() || OperatingSystem.IsTvOS() ||
#if NET6_0_OR_GREATER
				OperatingSystem.IsMacCatalyst()
#else
				!AotInfo.IsPlatformTrimmed &&
				(SystemInfo.isMacCatalyst ??= SystemInfo.IsOsPlatform(SystemInfo.macCatalystPlatform))
#endif
#else
				SystemInfo.isMac
#endif
				;
		}
	}
	/// <summary>
	/// Indicates whether the current execution is running on FreeBSD platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("freebsd")]
#endif
	public static Boolean IsFreeBsd
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsFreeBSD();
#else
		=> SystemInfo.isFreeBsd;
#endif
	/// <summary>
	/// Indicates whether the current execution is running on NetBSD platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("netbsd")]
#endif
	public static Boolean IsNetBsd
		=> !AotInfo.IsPlatformTrimmed && (SystemInfo.isNetBsd ??= SystemInfo.IsOsPlatform(SystemInfo.netBsdPlatform));
	/// <summary>
	/// Indicates whether the current execution is running on NetBSD platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("solaris")]
	[SupportedOSPlatformGuard("sunos")]
#endif
	public static Boolean IsSolaris
		=> !AotInfo.IsPlatformTrimmed && (SystemInfo.isSolaris ??=
			SystemInfo.IsOsPlatform(SystemInfo.solarisPlatform, SystemInfo.illumosPlatform));

	/// <summary>
	/// Indicates whether the current execution is running on a Web engine.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("browser")]
	[SupportedOSPlatformGuard("wasi")]
	[SupportedOSPlatformGuard("wasm")]
#endif
	public static Boolean IsWebRuntime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return
#if NET5_0_OR_GREATER
				OperatingSystem.IsBrowser() ||
#if NET8_0_OR_GREATER
				OperatingSystem.IsWasi()
#else
				!AotInfo.IsPlatformTrimmed && (SystemInfo.isWasi ??= SystemInfo.IsOsPlatform(SystemInfo.wPlatform))
#endif
#else
				SystemInfo.isWebRuntime
#endif
				;
		}
	}
	/// <summary>
	/// Indicates whether the current execution is running on Mono Runtime.
	/// </summary>
	public static Boolean IsMonoRuntime
		=> !AotInfo.IsReflectionDisabled &&
			(!SystemInfo.IsWebRuntime ? MonoInfo.IsEmptyNonLiteral : AotInfo.IsCodeGenerationSupported);

	/// <summary>
	/// Indicates whether the current application is running on the specified platform.
	/// </summary>
	/// <param name="platform">Platform name.</param>
	public static Boolean IsOsPlatform(String? platform)
		=> !String.IsNullOrWhiteSpace(platform) &&
#if !NET5_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform));
#else
#pragma warning disable CA1418
			OperatingSystem.IsOSPlatform(platform);
#pragma warning restore CA1418
#endif
	/// <summary>
	/// Indicates whether the current application is running on one of the specified platforms.
	/// </summary>
	/// <param name="platforms">Case-insensitive platform names.</param>
	public static Boolean IsOsPlatform(
#if NET9_0_OR_GREATER
		params ReadOnlySpan<String?> platforms
#else
		ReadOnlySpan<String?> platforms
#endif
	)
	{
		foreach (String? platform in platforms)
		{
			if (SystemInfo.IsOsPlatform(platform))
				return true;
		}
		return false;
	}
	/// <summary>
	/// Indicates whether the current application is running on one of the specified platforms.
	/// </summary>
	/// <param name="platforms">Case-insensitive platform names.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsOsPlatform(
#if !NET9_0_OR_GREATER
		params String?[] platforms
#else
		String?[] platforms
#endif
	)
		=> SystemInfo.IsOsPlatform(platforms.AsSpan());
}