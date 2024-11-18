namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This class allows to allocate buffers on stack if possible.
/// </summary>
#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	/// <summary>
	/// Indicates whether metadata for any required buffer is auto-composed.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static Boolean BufferAutoCompositionEnabled
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
			=> !AllocatedBuffer.disabledReflection && // In reflection-free mode this feature is unavailable.
				(!AppContext.TryGetSwitch("PInvoke.DisableBufferAutoComposition", out Boolean disable) || !disable);
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="action">Action to perform with allocated buffer.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	public static void Alloc<T>(UInt16 count, AllocatedBufferAction<T> action, Boolean isMinimumCount = false)
	{
		try
		{
			if (typeof(T).IsValueType)
				AllocatedBuffer.AllocValue(count, action, isMinimumCount);
			else
				AllocatedBuffer.AllocObject(count, action, isMinimumCount);
		}
		catch (Exception)
		{
			AllocatedBuffer.AllocHeap(count, action);
		}
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Action to perform with allocated buffer.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	public static void Alloc<T, TState>(UInt16 count, in TState state, AllocatedBufferAction<T, TState> action,
		Boolean isMinimumCount = false)
	{
		try
		{
			if (typeof(T).IsValueType)
				AllocatedBuffer.AllocValue(count, state, action, isMinimumCount);
			else
				AllocatedBuffer.AllocObject(count, state, action, isMinimumCount);
		}
		catch (Exception)
		{
			AllocatedBuffer.AllocHeap(count, state, action);
		}
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="func">Function to execute with allocated buffer.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <returns><paramref name="func"/> result.</returns>
	public static TResult Alloc<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func,
		Boolean isMinimumCount = false)
	{
		try
		{
			return typeof(T).IsValueType ?
				AllocatedBuffer.AllocValue(count, func, isMinimumCount) :
				AllocatedBuffer.AllocObject(count, func, isMinimumCount);
		}
		catch (Exception)
		{
			return AllocatedBuffer.AllocHeap(count, func);
		}
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="state">State object.</param>
	/// <param name="func">Function to execute with allocated buffer.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <returns><paramref name="func"/> result.</returns>
	public static TResult Alloc<T, TState, TResult>(UInt16 count, in TState state,
		AllocatedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount = false)
	{
		try
		{
			return typeof(T).IsValueType ?
				AllocatedBuffer.AllocValue(count, state, func, isMinimumCount) :
				AllocatedBuffer.AllocObject(count, state, func, isMinimumCount);
		}
		catch (Exception)
		{
			return AllocatedBuffer.AllocHeap(count, state, func);
		}
	}
	/// <summary>
	/// Registers object buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void Register<TBuffer>() where TBuffer : struct, IManagedBuffer<Object>
		=> MetadataCache<Object>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void Register<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T> where T : struct
		=> MetadataCache<T>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void RegisterNullable<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T?> where T : struct
		=> MetadataCache<T?>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object buffer.</typeparam>
	public static void RegisterSpace<TSpace>() where TSpace : struct, IManagedBuffer<Object>
		=> MetadataCache<Object>.RegisterBufferSpace<TSpace>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void RegisterSpace<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T> where T : struct
		=> MetadataCache<T>.RegisterBufferSpace<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TBuffer">Type of object space.</typeparam>
	public static void RegisterNullableSpace<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T?> where T : struct
		=> MetadataCache<T?>.RegisterBufferSpace<TBuffer>();
}
#pragma warning restore CA2252

/// <summary>
/// Provides a safe representation of allocated buffer of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public readonly ref struct AllocatedBuffer<T>
{
	/// <summary>
	/// Indicates whether current buffer is heap allocated.
	/// </summary>
	private readonly Boolean _heapAllocated;

	/// <summary>
	/// Current buffer span.
	/// </summary>
	public Span<T> Span { get; }
	/// <summary>
	/// Indicates whether current buffer is stack allocated.
	/// </summary>
	public Boolean InStack => !this._heapAllocated;
	/// <summary>
	/// Allocated buffer full length.
	/// </summary>
	public Int32 FullLength { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="span">Buffer span.</param>
	/// <param name="heapAllocated">Indicates whether current buffer is heap allocated.</param>
	/// <param name="fullLength">Allocated buffer full length.</param>
	internal AllocatedBuffer(Span<T> span, Boolean heapAllocated, Int32 fullLength)
	{
		this.Span = span;
		this._heapAllocated = heapAllocated;
		this.FullLength = fullLength;
	}

	/// <summary>
	/// Defines an implicit conversion of a given span to <see cref="AllocatedBuffer{T}"/>.
	/// </summary>
	/// <param name="span">A <typeparamref name="T"/> span to implicitly convert.</param>
	public static implicit operator AllocatedBuffer<T>(Span<T> span) => new(span, true, span.Length);
}