#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER || NET461_OR_GREATER
namespace Rxmxnx.PInvoke;

public partial class NativeUtilities
{
	/// <summary>
	/// Determines whether all methods referenced by the specified <typeparamref name="TDelegate"/> delegate are backed
	/// by statically compiled image code rather than dynamically generated runtime code.
	/// </summary>
	/// <typeparam name="TDelegate">The delegate type.</typeparam>
	/// <param name="method">The delegate instance to evaluate.</param>
	/// <returns>
	/// <see langword="true"/> if all referenced methods are backed by image-compiled code; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	/// This API is primarily intended for Mono-based runtimes.
	/// Returns <see langword="false"/> if the delegate is <see langword="null"/>, if any referenced method is an open
	/// generic method, or if the current platform does not support memory inspection.
	/// In reflection-free runtimes, valid delegates are treated as image-backed.
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3267)]
#endif
	public static Boolean IsImageMethod<TDelegate>(TDelegate? method) where TDelegate : Delegate
	{
		if (!MemoryInspector.IsSupported || method is null) return false;
		if (AotInfo.IsReflectionDisabled) return true;
		try
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			foreach (Delegate d in NativeUtilities.GetInvocationSpan(method))
#else
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (Delegate d in method.GetInvocationList())
#endif
			{
				if (!NativeUtilities.IsImageMethodUnsafe(d.Method))
					return false;
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}
}
#endif