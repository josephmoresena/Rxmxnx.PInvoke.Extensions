namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a method whose memory address is fixed in memory.
/// </summary>
/// <typeparam name="TDelegate">Type of the delegate that corresponds to the fixed method.</typeparam>
public interface IFixedMethod<out TDelegate> : IFixedPointer where TDelegate : Delegate
{
	/// <summary>
	/// Gets the delegate that points to the fixed method in memory.
	/// </summary>
	TDelegate Method { get; }

	/// <summary>
	/// Interface representing a disposable <see cref="IFixedMethod{TDelegate}"/> object for a fixed function delegate.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	public new interface IDisposable : IFixedMethod<TDelegate>, IFixedPointer.IDisposable;
}