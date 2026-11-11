#if NET7_0_OR_GREATER
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
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[Obsolete("Implement TryAlloc method instead.")]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1133)]
#endif
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
	static abstract ReadOnlySpan<Byte> GetSpan(TSelf state);
	/// <summary>
	/// Retrieves the span length from <paramref name="state"/>.
	/// </summary>
	/// <param name="state">Current item sequence state.</param>
	/// <returns>The binary span length for the specified state.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static virtual Int32 GetLength(in TSelf state) => TSelf.GetSpan(state).Length;

	/// <summary>
	/// Tries to create a new <see cref="GCHandle"/> from <paramref name="state"/>.
	/// </summary>
	/// <param name="state">Current item sequence state.</param>
	/// <param name="type">The type of <see cref="GCHandle"/> to create.</param>
	/// <param name="handle">Output. Created <see cref="GCHandle"/> that protects the value region.</param>
	/// <returns>
	/// <see langword="true"/> if a <paramref name="handle"/> was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>The current implementation forwards <paramref name="type"/> to <see cref="Alloc"/>.</remarks>
	static virtual Boolean TryAlloc(TSelf state, GCHandleType type, out GCHandle handle)
	{
#pragma warning disable CS0618
		if (TSelf.Alloc is null)
		{
			handle = default;
			return false;
		}

		handle = TSelf.Alloc(state, type);
#pragma warning restore CS0618
		return handle.IsAllocated;
	}
}
#endif