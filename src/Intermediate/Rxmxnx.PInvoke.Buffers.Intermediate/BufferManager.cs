namespace Rxmxnx.PInvoke;

/// <summary>
/// This class allows to allocate buffers on stack if possible.
/// </summary>
public static partial class BufferManager
{
	/// <summary>
	/// Indicates whether metadata for any required buffer is auto-composed.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean BufferAutoCompositionEnabled
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
			=> !AotInfo.IsReflectionDisabled &&
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
	public static void Alloc<T>(UInt16 count, ScopedBufferAction<T> action, Boolean isMinimumCount = false)
	{
		if (typeof(T).IsValueType)
			BufferManager.AllocValue(count, action, isMinimumCount);
		else
			BufferManager.AllocObject(count, action, isMinimumCount);
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
	public static void Alloc<T, TState>(UInt16 count, TState state, ScopedBufferAction<T, TState> action,
		Boolean isMinimumCount = false)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		if (typeof(T).IsValueType)
			BufferManager.AllocValue(count, state, action, isMinimumCount);
		else
			BufferManager.AllocObject(count, state, action, isMinimumCount);
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
	public static TResult Alloc<T, TResult>(UInt16 count, ScopedBufferFunc<T, TResult> func,
		Boolean isMinimumCount = false)
		=> typeof(T).IsValueType ?
			BufferManager.AllocValue(count, func, isMinimumCount) :
			BufferManager.AllocObject(count, func, isMinimumCount);
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
	public static TResult Alloc<T, TState, TResult>(UInt16 count, TState state,
		ScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount = false)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
		=> typeof(T).IsValueType ?
			BufferManager.AllocValue(count, state, func, isMinimumCount) :
			BufferManager.AllocObject(count, state, func, isMinimumCount);

	/// <summary>
	/// Registers object buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Register<TBuffer>() where TBuffer : struct, IManagedBuffer<Object>
		=> MetadataManager<Object>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of <typeparamref name="T"/> buffer.</typeparam>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Register<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T> where T : struct
	{
		// If unmanaged type, stackalloc should be used.
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>()) return;
		MetadataManager<T>.RegisterBuffer<TBuffer>();
	}
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of <see name="Nullable{T}"/> buffer.</typeparam>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullable<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T?> where T : struct
	{
		// If unmanaged type, stackalloc should be used.
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>()) return;
		MetadataManager<T?>.RegisterBuffer<TBuffer>();
	}
	/// <summary>
	/// Prepares the binary buffer metadata needed to allocate <paramref name="count"/> objects.
	/// </summary>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <exception cref="InvalidOperationException">Throw if missing metadata for any buffer component.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void PrepareBinaryBuffer(UInt16 count) => MetadataManager<Object>.PrepareBinaryMetadata(count);
	/// <summary>
	/// Prepares the binary buffer metadata needed to allocate <paramref name="count"/> <typeparamref name="T"/> items.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <exception cref="InvalidOperationException">Throw if missing metadata for any buffer component.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void PrepareBinaryBuffer<T>(UInt16 count) where T : struct
	{
		// If unmanaged type, stackalloc should be used.
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>()) return;
		MetadataManager<T>.PrepareBinaryMetadata(count);
	}
	/// <summary>
	/// Prepares the binary buffer metadata needed to allocate <paramref name="count"/> nullable <typeparamref name="T"/>
	/// items.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <exception cref="InvalidOperationException">Throw if missing metadata for any buffer component.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void PrepareBinaryBufferNullable<T>(UInt16 count) where T : struct
	{
		// If unmanaged type, stackalloc should be used.
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>()) return;
		MetadataManager<T?>.PrepareBinaryMetadata(count);
	}
}