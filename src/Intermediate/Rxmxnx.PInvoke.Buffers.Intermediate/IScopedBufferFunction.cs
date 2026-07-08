namespace Rxmxnx.PInvoke;

/// <summary>
/// Functional interface to replace <see cref="ScopedBufferAction{T}"/>
/// </summary>
public interface IScopedBufferFunction<T, out TResult>
{
	/// <summary>
	/// Performs an operation using <see cref="Buffer"/> and returns a <typeparamref name="TResult"/>.
	/// </summary>
	/// <param name="buffer">A <see cref="ScopedBuffer{T}"/> instance.</param>
	TResult Invoke(ScopedBuffer<T> buffer);
}