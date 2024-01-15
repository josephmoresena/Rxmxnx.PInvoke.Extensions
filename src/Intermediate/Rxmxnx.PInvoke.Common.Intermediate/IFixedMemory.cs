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
	/// Creates a new instance of <see cref="IFixedContext{Byte}"/> from the current instance.
	/// </summary>
	/// <returns>An instance of <see cref="IFixedContext{Byte}"/>.</returns>
	new IFixedContext<Byte> AsBinaryContext();

	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.Bytes;
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedMemory"/> object.
	/// </summary>
	public new interface IDisposable : IFixedMemory, IReadOnlyFixedMemory.IDisposable { }
}

/// <summary>
/// Interface representing a fixed block of memory for a specific type.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IFixedMemory<T> : IFixedMemory, IReadOnlyFixedMemory<T> where T : unmanaged
{
	/// <summary>
	/// Gets the value pointer to the fixed block of memory.
	/// </summary>
	new ValPtr<T> ValuePointer => (ValPtr<T>)this.Pointer;
	/// <summary>
	/// Gets a <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	new Span<T> Values { get; }

	ReadOnlyValPtr<T> IReadOnlyFixedMemory<T>.ValuePointer => this.ValuePointer;
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.Values;

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
	public new interface IDisposable : IFixedMemory<T>, IFixedMemory.IDisposable, IReadOnlyFixedMemory<T>.IDisposable;
}