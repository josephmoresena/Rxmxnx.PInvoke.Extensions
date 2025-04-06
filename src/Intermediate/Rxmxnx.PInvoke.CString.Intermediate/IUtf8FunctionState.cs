namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a value state for functional <see cref="CString"/> creation.
/// </summary>
/// <typeparam name="TSelf">Current state type.</typeparam>
public interface IUtf8FunctionState<TSelf> where TSelf : struct, IUtf8FunctionState<TSelf>
{
	/// <summary>
	/// Function that allocates a <typeparamref name="TSelf"/> instance.
	/// </summary>
	static virtual Func<TSelf, GCHandleType, GCHandle>? Alloc => default;

	/// <summary>
	/// Indicates whether resulting UTF-8 text is null-terminated.
	/// </summary>
	Boolean IsNullTerminated { get; }

	/// <summary>
	/// Retrieves the span from <paramref name="state"/>.
	/// </summary>
	/// <param name="state">Current item sequence state.</param>
	/// <returns>The binary span for the specified state.</returns>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	static abstract ReadOnlySpan<Byte> GetSpan(TSelf state);
	/// <summary>
	/// Retrieves the span length from <paramref name="state"/>.
	/// </summary>
	/// <param name="state">Current item sequence state.</param>
	/// <returns>The binary span length for the specified state.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	static virtual Int32 GetLength(in TSelf state) => TSelf.GetSpan(state).Length;
}