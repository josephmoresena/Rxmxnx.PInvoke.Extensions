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
		TrimInfo.IsDesktopTrimmedPlatform() ?
			TrimInfo.ZeroIlBytes() :
			TrimInfo.IsMobileTrimmedXnu() || (OperatingSystem.IsAndroid() && TrimInfo.ZeroIlBytes()) ||
			(MonoInfo.MonoAssemblyNameType is not null && MemoryInspector.IsSupported && AotInfo.IsAotFrame()) ||
			!EmitInfo.IsEmitAllowed;
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
		get => TrimInfo.IsPlatformTrimmed();
	}

	/// <inheritdoc cref="EmitInfo.IsDynamicMethod(MethodBase)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean IsDynamicCode(MethodBase methodBase) => EmitInfo.IsDynamicMethod(methodBase);
	/// <summary>
	/// Indicates whether the function pointer of <paramref name="methodHandle"/> references to an R/RX memory section.
	/// </summary>
	/// <param name="methodHandle">A <see langword="RuntimeMethodHandle"/> value.</param>
	/// <returns>
	/// <see langword="true"/> if the function pointer references to an R/RX memory section; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
	[ExcludeFromCodeCoverage]
#endif
	internal static unsafe Boolean IsImageMethodUnsafe(RuntimeMethodHandle methodHandle)
	{
		RuntimeHelpers.PrepareMethod(methodHandle);
		return MemoryInspector.Instance.IsReadOnlyAddress(methodHandle.GetFunctionPointer().ToPointer());
	}
}