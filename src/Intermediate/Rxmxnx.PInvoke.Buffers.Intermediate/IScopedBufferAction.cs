namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ScopedBuffer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ScopedBufferAction{T}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ScopedBuffer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer.</typeparam>
#endif
public interface IScopedBufferAction<T>
{
	/// <summary>
	/// Indicates whether <see cref="Count"/> is the minimum limit and not the exact limit for memory allocation.
	/// </summary>
	/// <remarks>The additional elements allocated are not accessible from <see cref="Accept"/>.</remarks>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	Boolean IsMinimalCount
	{
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		get => false;
	}
#else
	Boolean IsMinimalCount { get; }
#endif

	/// <summary>
	/// Number of <typeparamref name="T"/> elements required for <see cref="Accept"/> execution.
	/// </summary>
	UInt16 Count { get; }

	/// <summary>
	/// Performs an operation using the specified buffer.
	/// </summary>
	/// <param name="buffer">The <see cref="ScopedBuffer{T}"/> used by the operation.</param>
	void Accept(scoped ScopedBuffer<T> buffer);
}