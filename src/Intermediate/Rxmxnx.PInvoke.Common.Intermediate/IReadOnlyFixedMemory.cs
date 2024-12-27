namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a read-only fixed block of memory.
/// </summary>
public interface IReadOnlyFixedMemory : IFixedPointer
{
	/// <summary>
	/// Indicates whether current memory block is null-referenced or empty.
	/// </summary>
	Boolean IsNullOrEmpty { get; }
	/// <summary>
	/// Gets a read-only binary span over the fixed block of memory.
	/// </summary>
	ReadOnlySpan<Byte> Bytes { get; }
	/// <summary>
	/// Gets a read-only object span over the fixed block of memory.
	/// </summary>
	ReadOnlySpan<Object> Objects { get; }

	/// <summary>
	/// Creates a new instance of <see cref="IReadOnlyFixedContext{Byte}"/> from the current instance.
	/// </summary>
	/// <returns>An instance of <see cref="IReadOnlyFixedContext{Byte}"/>.</returns>
	IReadOnlyFixedContext<Byte> AsBinaryContext();
	/// <summary>
	/// Creates a new instance of <see cref="IReadOnlyFixedContext{Object}"/> from the current instance.
	/// </summary>
	/// <returns>An instance of <see cref="IReadOnlyFixedContext{Object}"/>.</returns>
	IReadOnlyFixedContext<Object> AsObjectContext();

	/// <summary>
	/// Interface representing a disposable <see cref="IReadOnlyFixedMemory"/> object for a
	/// read-only fixed block of memory.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	public new interface IDisposable : IReadOnlyFixedMemory, IFixedPointer.IDisposable { }
}

/// <summary>
/// Interface representing a read-only fixed block of memory for a specific type.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IReadOnlyFixedMemory<T> : IReadOnlyFixedMemory
{
	/// <summary>
	/// Gets the value pointer to the read-only fixed block of memory.
	/// </summary>
	ReadOnlyValPtr<T> ValuePointer => (ReadOnlyValPtr<T>)this.Pointer;
	/// <summary>
	/// Gets a read-only <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	ReadOnlySpan<T> Values { get; }

	/// <summary>
	/// Interface representing a disposable <see cref="IReadOnlyFixedMemory{T}"/> object for a
	/// read-only fixed block of memory with a specific type.
	/// This interface is used for managing fixed memory blocks that require explicit resource cleanup.
	/// </summary>
	/// <remarks>
	/// Implementing this interface allows for the encapsulation of unmanaged memory resources,
	/// ensuring that they are properly disposed of when no longer needed. It is crucial to call
	/// <see cref="System.IDisposable.Dispose"/> to release these unmanaged resources and avoid memory leaks.
	/// </remarks>
	public new interface IDisposable : IReadOnlyFixedMemory<T>, IReadOnlyFixedMemory.IDisposable;
}