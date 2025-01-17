namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public static partial class BufferManager
{
	/// <summary>
	/// Name of <see cref="IManagedBuffer{T}.GetMetadata{TBuffer}()"/> method.
	/// </summary>
#pragma warning disable CA2252
	private const String GetMetadataName = nameof(IManagedBuffer<Object>.GetMetadata);
#pragma warning restore CA2252
	/// <summary>
	/// Flags of <see cref="IManagedBuffer{T}.GetMetadata{TBuffer}"/> method.
	/// </summary>
	private const BindingFlags GetMetadataFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

	/// <summary>
	/// Type of <see cref="Composite{TBufferA,TBufferB,T}"/>.
	/// </summary>
	private static readonly Type typeofComposite = typeof(Composite<,,>);
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	[ExcludeFromCodeCoverage]
	private static void AllocHeap<T>(UInt16 count, ScopedBufferAction<T> action)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		try
		{
			Span<T> span = arr.AsSpan()[..count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);
			action(buffer);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Method to execute.</param>
	[ExcludeFromCodeCoverage]
	private static void AllocHeap<T, TState>(UInt16 count, TState state, ScopedBufferAction<T, TState> action)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		try
		{
			Span<T> span = arr.AsSpan()[..count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);
			action(buffer, state);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[ExcludeFromCodeCoverage]
	private static TResult AllocHeap<T, TResult>(UInt16 count, ScopedBufferFunc<T, TResult> func)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		try
		{
			Span<T> span = arr.AsSpan()[..count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);
			return func(buffer);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="func">Function to execute.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[ExcludeFromCodeCoverage]
	private static TResult AllocHeap<T, TState, TResult>(UInt16 count, TState state,
		ScopedBufferFunc<T, TState, TResult> func)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		try
		{
			Span<T> span = arr.AsSpan()[..count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);
			return func(buffer, state);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> reference elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	private static void AllocObject<T>(UInt16 count, ScopedBufferAction<T> action, Boolean isMinimumCount)
	{
		BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			BufferManager.AllocHeap(count, action);
			return;
		}
		TransformationState<T> stateT = new(action);
#if !PACKAGE
		MetadataManager<Object>.PrintMetadata();
#endif
		metadata.Execute(stateT, TransformationState<T>.Execute, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> reference elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Method to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	private static void AllocObject<T, TState>(UInt16 count, TState state, ScopedBufferAction<T, TState> action,
		Boolean isMinimumCount)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			BufferManager.AllocHeap(count, state, action);
			return;
		}
#if !PACKAGE
		MetadataManager<Object>.PrintMetadata();
#endif
		metadata.Execute(state, action, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> reference elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <returns><paramref name="func"/> result.</returns>
	private static TResult AllocObject<T, TResult>(UInt16 count, ScopedBufferFunc<T, TResult> func,
		Boolean isMinimumCount)
	{
		BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return BufferManager.AllocHeap(count, func);
		TransformationState<T, TResult> stateT = new(func);
#if !PACKAGE
		MetadataManager<Object>.PrintMetadata();
#endif
		return metadata.Execute(stateT, TransformationState<T, TResult>.Execute, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> reference elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <returns><paramref name="func"/> result.</returns>
	private static TResult AllocObject<T, TState, TResult>(UInt16 count, TState state,
		ScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return BufferManager.AllocHeap(count, state, func);
#if !PACKAGE
		MetadataManager<Object>.PrintMetadata();
#endif
		return metadata.Execute(state, func, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	private static void AllocValue<T>(UInt16 count, ScopedBufferAction<T> action, Boolean isMinimumCount)
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			BufferManager.AllocStack(count, action);
			return;
		}

		BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			BufferManager.AllocHeap(count, action);
			return;
		}
#if !PACKAGE
		MetadataManager<T>.PrintMetadata();
#endif
		metadata.Execute(action, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Method to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	private static void AllocValue<T, TState>(UInt16 count, TState state, ScopedBufferAction<T, TState> action,
		Boolean isMinimumCount)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			BufferManager.AllocStack(count, state, action);
			return;
		}

		BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			BufferManager.AllocHeap(count, state, action);
			return;
		}
#if !PACKAGE
		MetadataManager<T>.PrintMetadata();
#endif
		metadata.Execute<TState>(state, action, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <returns><paramref name="func"/> result.</returns>
	private static TResult AllocValue<T, TResult>(UInt16 count, ScopedBufferFunc<T, TResult> func,
		Boolean isMinimumCount)
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			return BufferManager.AllocStack(count, func);

		BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return BufferManager.AllocHeap(count, func);
#if !PACKAGE
		MetadataManager<T>.PrintMetadata();
#endif
		return metadata.Execute(func, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <returns><paramref name="func"/> result.</returns>
	private static TResult AllocValue<T, TState, TResult>(UInt16 count, TState state,
		ScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			return BufferManager.AllocStack(count, state, func);

		BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return BufferManager.AllocHeap(count, state, func);
#if !PACKAGE
		MetadataManager<T>.PrintMetadata();
#endif
		return metadata.Execute<TState, TResult>(state, func, count);
	}
	/// <summary>
	/// Retrieves the maximum value in the given binary space.
	/// </summary>
	/// <param name="space">Maximum binary power in the binary space.</param>
	/// <returns>The maximum value in the given binary space.</returns>
	private static UInt16 GetMaxValue(UInt16 space) => (UInt16)(space * 2 - 1);
	/// <summary>
	/// Retrieves the components sizes for given <paramref name="count"/>.
	/// </summary>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <returns>Enumeration of components sizes.</returns>
	private static IEnumerable<UInt16> GetBinaryComponents(UInt16 count)
	{
		for (UInt16 i = 0; i < 16; i++)
		{
			UInt16 mask = (UInt16)(1 << i);
			if ((count & mask) != 0) yield return mask;
		}
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
#pragma warning disable CS8500
	[ExcludeFromCodeCoverage]
	private static unsafe void AllocStack<T>(UInt16 count, ScopedBufferAction<T> action)
	{
		Int32 sizeOfT = sizeof(T);
		Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, count);
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		action(buffer);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Method to execute.</param>
	[ExcludeFromCodeCoverage]
	private static unsafe void AllocStack<T, TState>(UInt16 count, TState state, ScopedBufferAction<T, TState> action)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		Int32 sizeOfT = sizeof(T);
		Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, count);
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		action(buffer, state);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[ExcludeFromCodeCoverage]
	private static unsafe TResult AllocStack<T, TResult>(UInt16 count, ScopedBufferFunc<T, TResult> func)
	{
		Int32 sizeOfT = sizeof(T);
		Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, count);
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		return func(buffer);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="func">Function to execute.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[ExcludeFromCodeCoverage]
	private static unsafe TResult AllocStack<T, TState, TResult>(UInt16 count, TState state,
		ScopedBufferFunc<T, TState, TResult> func)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		Int32 sizeOfT = sizeof(T);
		Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, count);
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		return func(buffer, state);
	}
#pragma warning restore CS8500
}