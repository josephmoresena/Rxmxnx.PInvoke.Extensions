namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the execution platform.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1121)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe partial class SystemInfo
{
	/// <summary>
	/// Indicates whether the current execution is utilizing the built-in implementation of <see cref="Span{T}"/> and
	/// <see cref="ReadOnlySpan{T}"/>.
	/// </summary>
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	public static Boolean UsesNativeSpan => true;
#else
#pragma warning disable CS8500
	public static Boolean UsesNativeSpan => sizeof(Span<Byte>) <= 2 * sizeof(IntPtr);
#pragma warning restore CS8500
#endif
	/// <summary>
	/// Indicates whether the current execution is running on a Windows-compatible platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("windows")]
#endif
	public static Boolean IsWindows
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsWindows();
#elif !UAP10_0
		=> SystemInfo.isWindows;
#else
		=> true;
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
#elif !UAP10_0
				SystemInfo.isLinux
#else
				false
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
				(!TrimInfo.IsPlatformTrimmed() && RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
#endif
#elif !UAP10_0
				SystemInfo.isMac
#else
				false
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
#elif !UAP10_0
		=> SystemInfo.isFreeBsd;
#else
		=> false;
#endif
	/// <summary>
	/// Indicates whether the current execution is running on NetBSD platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("netbsd")]
#endif
	public static Boolean IsNetBsd
#if !UAP10_0
		=> !TrimInfo.IsPlatformTrimmed() &&
			(SystemInfo.isNetBsd ??= SystemInfo.IsOsPlatform(SystemInfo.netBsdPlatform));
#else
		=> false;
#endif
	/// <summary>
	/// Indicates whether the current execution is running on NetBSD platform.
	/// </summary>
#if NET6_0_OR_GREATER
	[SupportedOSPlatformGuard("solaris")]
	[SupportedOSPlatformGuard("illumos")]
#endif
	public static Boolean IsSolaris
#if !UAP10_0
		=> !TrimInfo.IsPlatformTrimmed() && (SystemInfo.isSolaris ??=
			SystemInfo.IsOsPlatform(SystemInfo.solarisPlatform, SystemInfo.illumosPlatform, SystemInfo.sunosPlatform));
#else
		=> false;
#endif

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
				OperatingSystem.IsBrowser() || RuntimeInformation.ProcessArchitecture == TrimInfo.WasmArch ||
#if NET8_0_OR_GREATER
				OperatingSystem.IsWasi()
#else
				(!TrimInfo.IsPlatformTrimmed() && (SystemInfo.isWasi ??= SystemInfo.IsOsPlatform(SystemInfo.wPlatform)))
#endif
#elif !UAP10_0
				SystemInfo.isWebRuntime
#else
				false
#endif
				;
		}
	}
	/// <summary>
	/// Indicates whether the current execution is running on Mono Runtime.
	/// </summary>
	public static Boolean IsMonoRuntime
#if !UAP10_0
	{
		get
		{
			if (MonoInfo.MonoRuntimeType is not null || MonoInfo.MonoAssemblyNameType is not null)
				return true; // Mono.Framework, Xamarin.*, Unity or Microsoft.NETCore.App.Runtime.Mono.*

			return AotInfo.IsReflectionDisabled && MonoInfo.IsEmptyNonLiteral; // Microsoft.NETCore.App (CLR)
		}
	}
#else
		=> false;
#endif

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