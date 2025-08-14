namespace Rxmxnx.PInvoke;

public static partial class SystemInfo
{
#if !NET5_0_OR_GREATER
	/// <inheritdoc cref="IsWindows"/>
	private static readonly Boolean isWindows;
	/// <inheritdoc cref="IsLinux"/>
	private static readonly Boolean isLinux;
	/// <inheritdoc cref="IsMac"/>
	private static readonly Boolean isMac;
	/// <inheritdoc cref="IsWebRuntime"/>
	private static readonly Boolean isWebRuntime;
	/// <inheritdoc cref="IsFreeBsd"/>
	private static readonly Boolean isFreeBsd;
#elif !NET8_0_OR_GREATER
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether the current execution is occurring on Mac Catalyst platform.
	/// </summary>
	private static Boolean? isMacCatalyst;
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on a WASI-compatible platform.
	/// </summary>
	private static Boolean? isWasi;
#endif
	/// <inheritdoc cref="IsNetBsd"/>
	private static Boolean? isNetBsd;
	/// <inheritdoc cref="IsSolaris"/>
	private static Boolean? isSolaris;

	/// <summary>
	/// Indicates whether the current property is not trimmable.
	/// </summary>
	private static Boolean NotTrimmable
		=>
#if NET5_0_OR_GREATER
			!OperatingSystem.IsLinux() && !OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS() &&
			!OperatingSystem.IsFreeBSD() && !OperatingSystem.IsAndroid() && !OperatingSystem.IsIOS() &&
			!OperatingSystem.IsTvOS() && !OperatingSystem.IsWatchOS() && !OperatingSystem.IsBrowser()
#if NET6_0_OR_GREATER
			&& !OperatingSystem.IsMacCatalyst()
#endif
#if NET8_0_OR_GREATER
			&& !OperatingSystem.IsWasi()
#endif
	;
#else
			true;
#endif
}