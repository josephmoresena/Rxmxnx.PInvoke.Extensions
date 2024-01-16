namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a pointer to a fixed block of memory.
/// </summary>
public interface IFixedPointer
{
	/// <summary>
	/// Gets the pointer to the fixed block of memory.
	/// </summary>
	IntPtr Pointer { get; }

	/// <summary>
	/// Interface representing a disposable <see cref="IFixedPointer"/> object.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	public interface IDisposable : IFixedPointer, System.IDisposable;
}