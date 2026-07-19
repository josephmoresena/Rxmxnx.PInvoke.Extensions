namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ScopedBuffer{T}"/> and returns a result.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer.</typeparam>
/// <typeparam name="TResult">The type of the returned result.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ScopedBufferFunc{T, TResult}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IScopedBufferFunction<T, out TResult>
{
	/// <summary>
	/// Indicates whether <see cref="Count"/> is the minimum limit and not the exact limit for memory allocation.
	/// </summary>
	/// <remarks>The additional elements allocated are not accessible from <see cref="Apply"/>.</remarks>
	Boolean IsMinimalCount => false;

	/// <summary>
	/// Number of <typeparamref name="T"/> elements required for <see cref="Apply"/> execution.
	/// </summary>
	protected internal UInt16 Count { get; }

	/// <summary>
	/// Performs an operation using the specified buffer and returns a result.
	/// </summary>
	/// <param name="buffer">The <see cref="ScopedBuffer{T}"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	protected internal TResult Apply(scoped ScopedBuffer<T> buffer);
}