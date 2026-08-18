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
			return AotInfo.reflectionDisabled ??= !TrimInfo.StringTypeNameContainsString();
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
			if (TrimInfo.ZeroIlBytes() && AotInfo.IsDesktopOrAndroid())
				return false;
#endif
#if !UAP10_0
			return !AotInfo.IsReflectionDisabled && EmitInfo.IsEmitAllowed;
#else
			return !AotInfo.isAotRuntime;
#endif
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
		get => TrimInfo.IsPlatformTrimmed();
	}
}