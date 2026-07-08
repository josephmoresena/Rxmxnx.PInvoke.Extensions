namespace Rxmxnx.PInvoke;

/// <summary>
/// Functional interface to replace <see cref="ScopedBufferAction{T}"/>
/// </summary>
public interface IScopedBufferAction<T>
{
	/// <summary>
	/// Performs an operation using <see cref="Buffer"/>.
	/// </summary>
	/// <param name="buffer">A <see cref="ScopedBuffer{T}"/> instance.</param>
	void Invoke(ScopedBuffer<T> buffer);
}