namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a context from a block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IFixedContext<T> : IReadOnlyFixedContext<T>, IFixedMemory<T>
{
	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as
	/// a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual memory from the transformation.</param>
	/// <returns>An instance of <see cref="IFixedContext{TDestination}"/>.</returns>
	IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual);

	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as
	/// a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual read-only memory from the transformation.</param>
	/// <returns>An instance of <see cref="IReadOnlyFixedContext{TDestination}"/>.</returns>
	new IFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual);

	/// <summary>
	/// Interface representing a disposable <see cref="IFixedContext{T}"/> object for a context
	/// of a fixed memory block with a specific type.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public new interface IDisposable : IFixedContext<T>, IFixedMemory<T>.IDisposable,
		IReadOnlyFixedContext<T>.IDisposable;
}