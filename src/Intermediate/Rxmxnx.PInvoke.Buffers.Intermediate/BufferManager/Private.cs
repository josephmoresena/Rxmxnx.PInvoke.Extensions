namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static partial class BufferManager<T>
{
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> reference elements.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocObject<TAction>(UInt16 count, in TAction action, Boolean isMinimumCount)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		BufferTypeMetadata<Object>? metadata = BufferManager.Storage.GetMetadata<Object>(count);
		Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count || count == 0);
#if !PACKAGE
		BufferManager.Storage.PrintMetadata<Object>(!stackAlloc);
#endif
		if (stackAlloc)
		{
			Debug.Assert(metadata is not null);
			metadata.Execute<T, TAction>(action, count);
			return;
		}

		BufferManager<T>.AllocHeap(count, action);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> reference elements.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocObject<TFunction, TResult>(UInt16 count, in TFunction func, Boolean isMinimumCount,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		BufferTypeMetadata<Object>? metadata = BufferManager.Storage.GetMetadata<Object>(count);
		Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count || count == 0);
#if !PACKAGE
		BufferManager.Storage.PrintMetadata<Object>(!stackAlloc);
#endif
		if (!stackAlloc)
		{
			BufferManager<T>.AllocHeap(count, func, out result);
			return;
		}
		Debug.Assert(metadata is not null);
		result = metadata.Execute<T, TFunction, TResult>(func, count);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocValue<TAction>(UInt16 count, in TAction action, Boolean isMinimumCount)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			BufferManager<T>.StackAlloc(count, action);
			return;
		}

		BufferTypeMetadata<T>? metadata = BufferManager.Storage.GetMetadata<T>(count);
		Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count || count == 0);
#if !PACKAGE
		BufferManager.Storage.PrintMetadata<T>(!stackAlloc);
#endif
		if (stackAlloc)
		{
			Debug.Assert(metadata is not null);
			metadata.Execute(action, count);
		}
		else
		{
			BufferManager<T>.AllocHeap(count, action);
		}
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="isMinimumCount">
	/// Indicates whether <paramref name="count"/> is just the minimum limit.
	/// </param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocValue<TFunction, TResult>(UInt16 count, in TFunction func, Boolean isMinimumCount,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			BufferManager<T>.StackAlloc(count, in func, out result);
			return;
		}

		BufferTypeMetadata<T>? metadata = BufferManager.Storage.GetMetadata<T>(count);
		Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count || count == 0);
#if !PACKAGE
		BufferManager.Storage.PrintMetadata<T>(!stackAlloc);
#endif
		if (!stackAlloc)
		{
			BufferManager<T>.AllocHeap(count, in func, out result);
			return;
		}
		Debug.Assert(metadata is not null);
		result = metadata.Execute<TFunction, TResult>(func, count);
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocHeap<TAction>(UInt16 count, TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		try
		{
			Span<T> span = arr.AsSpan()[..count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);

			span.Clear();
			action.Invoke(buffer);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="result">Output. Function result.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocHeap<TFunction, TResult>(UInt16 count, in TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		try
		{
			Span<T> span = arr.AsSpan()[..count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);

			span.Clear();
			result = func.Invoke(buffer);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS8500
	private static unsafe void StackAlloc<TAction>(UInt16 count, in TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		Int32 sizeOfT = sizeof(T);
		Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, count);
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		action.Invoke(buffer);
	}
	/// <summary>
	/// Allocates a stack buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void StackAlloc<TFunction, TResult>(UInt16 count, in TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		Int32 sizeOfT = sizeof(T);
		Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, count);
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		result = func.Invoke(buffer);
	}
}