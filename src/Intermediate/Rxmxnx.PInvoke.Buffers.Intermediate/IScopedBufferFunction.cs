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
	/// Performs an operation using the specified buffer and returns a result.
	/// </summary>
	/// <param name="buffer">The <see cref="ScopedBuffer{T}"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	TResult Invoke(ScopedBuffer<T> buffer);
}