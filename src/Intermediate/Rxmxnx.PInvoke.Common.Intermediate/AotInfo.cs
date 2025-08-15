namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the Ahead-of-Time compilation.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
public static partial class AotInfo
{
	/// <summary>
	/// Indicates whether the current runtime is ahead-of-time.
	/// </summary>
	private static readonly Boolean isAotRuntime =
#if !NET6_0_OR_GREATER
		!AotInfo.IsJitEnabled();
#else
		JitInfo.GetCompiledILBytes() == 0L && JitInfo.GetCompiledMethodCount() == 0;
#endif

	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	private static Boolean? reflectionDisabled;

	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	public static Boolean IsReflectionDisabled
	{
		get
		{
#if NET6_0_OR_GREATER
			if (!AotInfo.IsNativeAot)
				return false;
#endif
			return AotInfo.reflectionDisabled ??= !AotInfo.StringTypeNameContainsString();
		}
	}
	/// <summary>
	/// Indicates whether the current runtime is Native AOT.
	/// </summary>
	public static Boolean IsNativeAot => AotInfo.isAotRuntime;
	/// <summary>
	/// Indicates whether the current runtime has been trimmed for the platform.
	/// </summary>
	public static Boolean IsPlatformTrimmed
		=>
#if NET5_0_OR_GREATER
			OperatingSystem.IsLinux() || OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() ||
			OperatingSystem.IsFreeBSD() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() ||
			OperatingSystem.IsTvOS() || OperatingSystem.IsWatchOS() || OperatingSystem.IsBrowser()
#if NET6_0_OR_GREATER
			|| OperatingSystem.IsMacCatalyst()
#endif
#if NET8_0_OR_GREATER
			|| OperatingSystem.IsWasi()
#endif
#else
			false
#endif
	;

	/// <summary>
	/// Internal UTF-8 empty text.
	/// </summary>
	/// <returns>A read-only byte span of UTF-8 null-characters.</returns>
	internal static ReadOnlySpan<Byte> EmptyUt8Text() => "\0\0\0"u8;
}