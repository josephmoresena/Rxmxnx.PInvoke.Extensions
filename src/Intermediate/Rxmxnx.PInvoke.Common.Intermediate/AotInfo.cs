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
		JitInfo.GetCompiledILBytes() == 0L && JitInfo.GetCompiledMethodCount() == 0 &&
		(AotInfo.IsDesktopTrimmedPlatform() || !EmitInfo.IsEmitAllowed);
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
	/// Indicates whether the current runtime supports the emission of dynamic IL code.
	/// </summary>
	public static Boolean IsCodeGenerationSupported
	{
		get
		{
#if NET6_0_OR_GREATER
			if (AotInfo.IsNativeAot)
				return false;
#endif
			return !AotInfo.IsReflectionDisabled && EmitInfo.IsEmitAllowed;
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
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
#if NET5_0_OR_GREATER
			return AotInfo.IsDesktopTrimmedPlatform() || AotInfo.IsMobileTrimmedPlatform() ||
				AotInfo.IsWebTrimmedPlatform();
#else
			return false;
#endif
		}
	}

	/// <summary>
	/// Internal UTF-8 empty text.
	/// </summary>
	/// <returns>A read-only byte span of UTF-8 null-characters.</returns>
	internal static ReadOnlySpan<Byte> EmptyUt8Text() => "\0\0\0"u8;
	/// <summary>
	/// Internal Windows New line UTF-8 sequence.
	/// </summary>
	/// <returns>A read-only byte span containing UTF-8 new line.</returns>
	internal static ReadOnlySpan<Byte> WindowsNewLine() => "\r\n"u8;
	/// <summary>
	/// Internal non-Windows New line UTF-8 sequence.
	/// </summary>
	/// <returns>A read-only byte span containing UTF-8 new line.</returns>
	internal static ReadOnlySpan<Byte> NonWindowsNewLine() => "\n"u8;
}