namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents the metadata of a managed buffer type.
/// </summary>
public abstract partial class BufferTypeMetadata
{
	/// <summary>
	/// Indicates whether the current type is binary space.
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
	/// The size in bytes of the buffer.
	/// </summary>
	public abstract Int32 SizeOf { get; }
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
}

/// <summary>
/// Represents the metadata of a managed buffer type.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public abstract partial class BufferTypeMetadata<T> : BufferTypeMetadata
{
	/// <inheritdoc/>
	public sealed override BufferTypeMetadata this[Int32 index]
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get => this.Components.Span[index];
	}
	/// <inheritdoc/>
	public sealed override Int32 ComponentCount
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get => this.Components.Length;
	}

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
#if !NETCOREAPP3_0_OR_GREATER
		=> BuffersHelper.GetStaticMetadata<T, TBuffer>();
#else
		=> IManagedBuffer<T>.GetMetadata<TBuffer>();
#endif
}