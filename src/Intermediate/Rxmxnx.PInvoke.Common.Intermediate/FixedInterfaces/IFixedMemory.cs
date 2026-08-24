namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a fixed block of memory.
/// </summary>
public interface IFixedMemory : IReadOnlyFixedMemory
{
	/// <summary>
	/// Gets a binary span over the fixed block of memory.
	/// </summary>
	new Span<Byte> Bytes { get; }
	/// <summary>
	/// Gets an object span over the fixed block of memory.
	/// </summary>
	new Span<Object> Objects { get; }

	/// <summary>
	/// Creates a new instance of <see cref="IFixedContext{Byte}"/> from the current instance.
	/// </summary>
	/// <returns>An instance of <see cref="IFixedContext{Byte}"/>.</returns>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterface, ObsoleteConstants.ErrorFixedInterface)]
#endif
	new IFixedContext<Byte> AsBinaryContext();
	/// <summary>
	/// Creates a new instance of <see cref="IFixedContext{Object}"/> from the current instance.
	/// </summary>
	/// <returns>An instance of <see cref="IFixedContext{Object}"/>.</returns>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterface, ObsoleteConstants.ErrorFixedInterface)]
#endif
	new IFixedContext<Object> AsObjectContext();

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedMemory"/> object.
	/// </summary>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public new interface IDisposable : IFixedMemory, IReadOnlyFixedMemory.IDisposable;
#endif
}

/// <summary>
/// Interface representing a fixed block of memory for a specific type.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IFixedMemory<T> : IFixedMemory, IReadOnlyFixedMemory<T>
{
	/// <summary>
	/// Gets the value pointer to the fixed block of memory.
	/// </summary>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	new ValPtr<T> ValuePointer => (ValPtr<T>)this.Pointer;
#else
	new ValPtr<T> ValuePointer { get; }
#endif
	/// <summary>
	/// Gets a <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	new Span<T> Values { get; }

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Interface representing a disposable <see cref="IFixedMemory{T}"/> object for a
	/// fixed block of memory with a specific type.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public new interface IDisposable : IFixedMemory<T>, IFixedMemory.IDisposable, IReadOnlyFixedMemory<T>.IDisposable;
#endif
}