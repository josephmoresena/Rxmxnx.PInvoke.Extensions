#if PACKAGE && !NETCOREAPP
using IEnumerator = System.Collections.IEnumerator;
using IEnumerable = System.Collections.IEnumerable;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents the metadata of a managed buffer type.
/// </summary>
public abstract partial class BufferTypeMetadata : IEnumerableSequence<BufferTypeMetadata>
{
	/// <summary>
	/// Indicates whether current type is binary space.
	/// </summary>
	public Boolean IsBinary { get; }
	/// <summary>
	/// Buffer capacity.
	/// </summary>
	public UInt16 Size { get; }
	/// <summary>
	/// Number of components.
	/// </summary>
	public abstract Int32 ComponentCount { get; }
	/// <summary>
	/// Buffer type.
	/// </summary>
	public abstract Type BufferType { get; }
	/// <summary>
	/// Retrieves a component from current metadata at the specified zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the component to retrieve.</param>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when <paramref name="index"/> is less than zero or greater than or equal to
	/// the count of the components.
	/// </exception>
	/// <returns>The component at the specified index within the buffer metadata.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[IndexerName("Item")]
	public abstract BufferTypeMetadata this[Int32 index] { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	/// <param name="capacity">Buffer's capacity.</param>
	private protected BufferTypeMetadata(Boolean isBinary, UInt16 capacity)
	{
		this.IsBinary = isBinary;
		this.Size = capacity;
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	BufferTypeMetadata IEnumerableSequence<BufferTypeMetadata>.GetItem(Int32 index) => this[index];
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Int32 IEnumerableSequence<BufferTypeMetadata>.GetSize() => this.ComponentCount;
#if PACKAGE && !NETCOREAPP
	IEnumerator<BufferTypeMetadata> IEnumerable<BufferTypeMetadata>.GetEnumerator() 
		=> IEnumerableSequence.CreateEnumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => IEnumerableSequence.CreateEnumerator(this);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Execute<T, TBuffer, TAction>(in TAction action, BufferTypeMetadata metadata, Int32 spanLength)
		where TBuffer : struct
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, metadata.Size, metadata);
		action.Invoke(scoped);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static TResult Execute<T, TBuffer, TFunction, TResult>(in TFunction func, BufferTypeMetadata metadata,
		Int32 spanLength) where TBuffer : struct
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, metadata.Size, metadata);
		return func.Invoke(scoped);
	}
}

/// <summary>
/// Represents the metadata of a managed buffer type.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public abstract class BufferTypeMetadata<T> : BufferTypeMetadata
{
	/// <summary>
	/// Current buffer components.
	/// </summary>
	internal ReadOnlyMemory<BufferTypeMetadata<T>> Components { get; }

	/// <inheritdoc/>
	public override BufferTypeMetadata this[Int32 index] => this.Components.Span[index];
	/// <inheritdoc/>
	public override Int32 ComponentCount => this.Components.Length;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	/// <param name="components">Buffer's components.</param>
	/// <param name="capacity">Buffer's capacity.</param>
	private protected BufferTypeMetadata(Boolean isBinary, BufferTypeMetadata<T>[] components, UInt16 capacity) :
		base(isBinary, capacity)
		=> this.Components = components;

	/// <summary>
	/// Appends all components from current buffer type.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal virtual void AppendComponent(IMetadataStorage storage) { }
	/// <summary>
	/// Composes a new buffer using twice the current buffer type.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal BufferTypeMetadata<T>? Double(IMetadataStorage storage) => this.Compose(storage, this);
	/// <summary>
	/// Composes a new buffer using current buffer type and <paramref name="otherMetadata"/>.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <param name="otherMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal abstract BufferTypeMetadata<T>? Compose(IMetadataStorage storage, BufferTypeMetadata<T> otherMetadata);
	/// <summary>
	/// Composes a new buffer using current buffer type and <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Other buffer type.</typeparam>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal abstract BufferTypeMetadata<T>? Compose<
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>(IMetadataStorage storage)
		where TBuffer : struct, IManagedBuffer<T>;
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/> interface.</typeparam>
	/// <param name="action">A <see cref="IScopedBufferAction{T}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract void Execute<TAction>(in TAction action, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>;
#else
		where TAction : IScopedBufferAction<T>, allows ref struct;
#endif
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, Result}"/> interface.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="IScopedBufferFunction{T,TResult}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract TResult Execute<TFunction, TResult>(in TFunction func, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>;
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct;
#endif
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TU">Type of elements exposed to the operation.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/> interface.</typeparam>
	/// <param name="action">A <see cref="IScopedBufferAction{T}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract void Execute<TU, TAction>(in TAction action, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<TU>;
#else
		where TAction : IScopedBufferAction<TU>, allows ref struct;
#endif
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TU">Type of elements exposed to the operation.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, Result}"/> interface.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="IScopedBufferFunction{T,TResult}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract TResult Execute<TU, TFunction, TResult>(in TFunction func, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<TU, TResult>;
#else
		where TFunction : IScopedBufferFunction<TU, TResult>, allows ref struct;
#endif
}