namespace Rxmxnx.PInvoke;

public static partial class AotInfo
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
}