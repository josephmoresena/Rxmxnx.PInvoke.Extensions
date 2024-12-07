namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a read-only reference to a fixed memory location.
/// </summary>
/// <typeparam name="T">Type of the value referenced in memory.</typeparam>
public interface IReadOnlyFixedReference<T> : IReadOnlyReferenceable<T>, IReadOnlyFixedMemory
{
	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory reference as a
	/// <typeparamref name="TDestination"/> memory reference.
	/// </summary>
	/// <typeparam name="TDestination">Type of the reinterpreted memory reference.</typeparam>
	/// <param name="residual">Output. Residual read-only memory left after the transformation.</param>
	/// <returns>
	/// A <see cref="IReadOnlyFixedReference{TDestination}"/> instance.
	/// </returns>
	IReadOnlyFixedReference<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual);

	/// <summary>
	/// Interface representing a disposable <see cref="IReadOnlyFixedReference{T}"/> object for a read-only
	/// fixed memory reference with a specific type.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	public new interface IDisposable : IReadOnlyFixedReference<T>, IReadOnlyFixedMemory.IDisposable;
}