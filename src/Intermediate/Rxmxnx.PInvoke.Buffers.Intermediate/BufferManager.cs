namespace Rxmxnx.PInvoke;

/// <summary>
/// This class allows to allocate buffers on stack if possible.
/// </summary>
public static partial class BufferManager
{
	/// <summary>
	/// Indicates whether metadata for any required buffer is auto-composed.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static Boolean BufferAutoCompositionEnabled => !BufferManager.disabledReflection;
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
				BufferManager.AllocValue(count, action, isMinimumCount);
			else
				BufferManager.AllocObject(count, action, isMinimumCount);
		}
		catch (Exception)
		{
			BufferManager.AllocHeap(count, action);
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
				BufferManager.AllocValue(count, state, action, isMinimumCount);
			else
				BufferManager.AllocObject(count, state, action, isMinimumCount);
		}
		catch (Exception)
		{
			BufferManager.AllocHeap(count, state, action);
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
				BufferManager.AllocValue(count, func, isMinimumCount) :
				BufferManager.AllocObject(count, func, isMinimumCount);
		}
		catch (Exception)
		{
			return BufferManager.AllocHeap(count, func);
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
				BufferManager.AllocValue(count, state, func, isMinimumCount) :
				BufferManager.AllocObject(count, state, func, isMinimumCount);
		}
		catch (Exception)
		{
			return BufferManager.AllocHeap(count, state, func);
		}
	}
	/// <summary>
	/// Registers object buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void Register<TBuffer>() where TBuffer : struct, IManagedBuffer<Object>
		=> MetadataManager<Object>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of <typeparamref name="T"/> buffer.</typeparam>
	public static void Register<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T> where T : struct
		=> MetadataManager<T>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of <see name="Nullable{T}"/> buffer.</typeparam>
	public static void RegisterNullable<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T?> where T : struct
		=> MetadataManager<T?>.RegisterBuffer<TBuffer>();
	// /// <summary>
	// /// Registers object space.
	// /// </summary>
	// /// <typeparam name="TSpace">Type of object space.</typeparam>
	// public static void RegisterSpace<TSpace>() where TSpace : struct, IManagedBuffer<Object>
	// 	=> MetadataManager<Object>.RegisterBufferSpace<TSpace>();
	// /// <summary>
	// /// Registers <typeparamref name="T"/> space.
	// /// </summary>
	// /// <typeparam name="T">Type of items in the buffer.</typeparam>
	// /// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
	// public static void RegisterSpace<T, TSpace>() where TSpace : struct, IManagedBuffer<T> where T : struct
	// 	=> MetadataManager<T>.RegisterBufferSpace<TSpace>();
	// /// <summary>
	// /// Registers <typeparamref name="T"/> space.
	// /// </summary>
	// /// <typeparam name="T">Type of nullable items in the space.</typeparam>
	// /// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
	// public static void RegisterNullableSpace<T, TSpace>() where TSpace : struct, IManagedBuffer<T?> where T : struct
	// 	=> MetadataManager<T?>.RegisterBufferSpace<TSpace>();
}