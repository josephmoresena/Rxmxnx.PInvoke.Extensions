namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a read-only fixed block of memory.
/// </summary>
public interface IReadOnlyFixedMemory : IFixedPointer
{
	/// <summary>
	/// Gets a read-only binary span over the fixed block of memory.
	/// </summary>
	ReadOnlySpan<Byte> Bytes { get; }

	/// <summary>
	/// Creates a new instance of <see cref="IReadOnlyFixedContext{Byte}"/> from the current instance.
	/// </summary>
	/// <returns>An instance of <see cref="IReadOnlyFixedContext{Byte}"/>.</returns>
	IReadOnlyFixedContext<Byte> AsBinaryContext();

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IReadOnlyFixedMemory"/> object.
	/// </summary>
	public new interface IDisposable : IReadOnlyFixedMemory, IFixedPointer.IDisposable
	{
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
		/// <inheritdoc cref="IReadOnlyFixedMemory.AsBinaryContext()"/>
		new IReadOnlyFixedContext<Byte>.IDisposable AsBinaryContext();
	}
}

/// <summary>
/// Interface representing a read-only fixed block of memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IReadOnlyFixedMemory<T> : IReadOnlyFixedMemory where T : unmanaged
{
	/// <summary>
	/// Gets a read-only <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	ReadOnlySpan<T> Values { get; }

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IReadOnlyFixedMemory{T}"/> object.
	/// </summary>
	public new interface IDisposable : IReadOnlyFixedMemory<T>, IReadOnlyFixedMemory.IDisposable { }
}