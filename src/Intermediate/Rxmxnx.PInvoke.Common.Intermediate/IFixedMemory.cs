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

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedMemory"/> object.
	/// </summary>
	public new interface IDisposable : IFixedMemory, IReadOnlyFixedMemory.IDisposable
	{
		IFixedContext<Byte> IFixedMemory.AsBinaryContext() => this.AsBinaryContext();
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
		/// <inheritdoc cref="IReadOnlyFixedMemory.AsBinaryContext()"/>
		new IFixedContext<Byte>.IDisposable AsBinaryContext();
	}
}

/// <summary>
/// Interface representing a fixed block of memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IFixedMemory<T> : IFixedMemory, IReadOnlyFixedMemory<T> where T : unmanaged
{
	/// <summary>
	/// Gets a <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	new Span<T> Values { get; }

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedMemory{T}"/> object.
	/// </summary>
	public new interface IDisposable : IFixedMemory<T>, IFixedMemory.IDisposable, IReadOnlyFixedMemory<T>.IDisposable { }
}