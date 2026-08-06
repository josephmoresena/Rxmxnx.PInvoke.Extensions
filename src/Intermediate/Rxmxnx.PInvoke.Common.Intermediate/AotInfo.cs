namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the Ahead-of-Time compilation.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
#if !UAP
public static partial class AotInfo
#else
public static class AotInfo
#endif
{
	/// <summary>
	/// Indicates whether the current runtime is ahead-of-time.
	/// </summary>
	private static readonly Boolean isAotRuntime =
#if UAP
		// .NET Native -> Empty non-literal.
		!MemoryInspector.Instance.IsLiteral(TrimInfo.EmptyUt8Text());
#elif !NET6_0_OR_GREATER
		!AotInfo.IsJitEnabled();
#else
		TrimInfo.IsMobileTrimmedXnu() || // iOS, tvOS, watchOS, macCatalyst
		TrimInfo.ZeroIlBytes() && AotInfo.IsDesktopOrAndroid() || AotInfo.IsMonoAot() ||
		!AotInfo.IsDesktopOrAndroid() && !EmitInfo.IsEmitAllowed;
#endif
	/// <summary>
	/// Indicates whether runtime reflection is disabled.
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

	/// <summary>
	/// Indicates whether <paramref name="methodBase"/> is dynamic.
	/// </summary>
	/// <param name="methodBase">A <see cref="MethodBase"/> instance.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="methodBase"/> is a dynamic method or its assembly is dynamic;
	/// otherwise <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean IsDynamicCode(MethodBase methodBase)
#if !UAP10_0
		=> EmitInfo.IsDynamicMethod(methodBase);
#else
		=> false;
#endif
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
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
#endif
}