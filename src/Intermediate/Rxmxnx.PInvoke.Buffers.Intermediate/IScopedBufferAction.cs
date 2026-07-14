namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ScopedBuffer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ScopedBufferAction{T}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IScopedBufferAction<T>
{
	/// <summary>
	/// Performs an operation using the specified buffer.
	/// </summary>
	/// <param name="buffer">The <see cref="ScopedBuffer{T}"/> used by the operation.</param>
	void Invoke(ScopedBuffer<T> buffer);
}