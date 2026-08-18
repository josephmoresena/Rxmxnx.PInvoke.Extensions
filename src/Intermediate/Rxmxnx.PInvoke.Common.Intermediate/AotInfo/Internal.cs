namespace Rxmxnx.PInvoke;

public static partial class AotInfo
{
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