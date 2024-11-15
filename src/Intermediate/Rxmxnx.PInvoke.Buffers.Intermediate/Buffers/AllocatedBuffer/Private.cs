namespace Rxmxnx.PInvoke.Buffers;

public static partial class AllocatedBuffer
{
	/// <summary>
	/// Name of <see cref="IAllocatedBuffer{Object}.GetMetadata{TBuffer}()"/> method.
	/// </summary>
#pragma warning disable CA2252
	private const String getMetadataName = nameof(IAllocatedBuffer<Object>.GetMetadata);
#pragma warning restore CA2252
	/// <summary>
	/// Flags of <see cref="IAllocatedBuffer{Object}.GetMetadata{TBuffer}"/> method.
	/// </summary>
	private const BindingFlags getMetadataFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

	/// <summary>
	/// Type of <see cref="Composed{TBufferA,TBufferB,T}"/>.
	/// </summary>
	private static readonly Type typeofComposed = typeof(Composed<,,>);
	/// <summary>
	/// Flag to check if reflection is disabled.
	/// </summary>
	private static readonly Boolean disabledReflection = !typeof(String).ToString().Contains(nameof(String));

	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="action">Method to execute.</param>
	private static void AllocHeap<T>(UInt16 count, AllocatedBufferAction<T> action)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		action(buffer);
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Method to execute.</param>
	private static void AllocHeap<T, TState>(UInt16 count, in TState state, AllocatedBufferAction<T, TState> action)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		action(buffer, state);
	}
	/// <summary>
	/// Allocates a heap buffer of size of <paramref name="count"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Required buffer size.</param>
	/// <param name="func">Function to execute.</param>
	/// <returns><paramref name="func"/> result.</returns>
	private static TResult AllocHeap<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		return func(buffer);
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
	private static TResult AllocHeap<T, TState, TResult>(UInt16 count, in TState state,
		AllocatedBufferFunc<T, TState, TResult> func)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		return func(buffer, state);
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
	private static void AllocObject<T>(UInt16 count, AllocatedBufferAction<T> action, Boolean isMinimumCount)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			AllocatedBuffer.AllocHeap(count, action);
			return;
		}
		ActionState<T> stateT = new(action);
		metadata.Execute(stateT, ActionState<T>.Execute);
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
	private static void AllocObject<T, TState>(UInt16 count, in TState state, AllocatedBufferAction<T, TState> action,
		Boolean isMinimumCount)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			AllocatedBuffer.AllocHeap(count, state, action);
			return;
		}
		ActionState<T, TState> stateT = new(action, state);
		metadata.Execute(stateT, ActionState<T, TState>.Execute);
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
	private static TResult AllocObject<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func,
		Boolean isMinimumCount)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return AllocatedBuffer.AllocHeap(count, func);
		FunctionState<T, TResult> stateT = new(func);
		return metadata.Execute(stateT, FunctionState<T, TResult>.Execute);
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
	private static TResult AllocObject<T, TState, TResult>(UInt16 count, in TState state,
		AllocatedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return AllocatedBuffer.AllocHeap(count, state, func);
		FunctionState<T, TState, TResult> stateT = new(func, state);
		return metadata.Execute(stateT, FunctionState<T, TState, TResult>.Execute);
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
	private static void AllocValue<T>(UInt16 count, AllocatedBufferAction<T> action, Boolean isMinimumCount)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			AllocatedBuffer.AllocHeap(count, action);
			return;
		}
		metadata.Execute(action);
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
	private static void AllocValue<T, TState>(UInt16 count, in TState state, AllocatedBufferAction<T, TState> action,
		Boolean isMinimumCount)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
		{
			AllocatedBuffer.AllocHeap(count, state, action);
			return;
		}
		metadata.Execute(state, action);
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
	private static TResult AllocValue<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func,
		Boolean isMinimumCount)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return AllocatedBuffer.AllocHeap(count, func);
		return metadata.Execute(func);
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
	private static TResult AllocValue<T, TState, TResult>(UInt16 count, in TState state,
		AllocatedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null || (!isMinimumCount && metadata.Size != count))
			return AllocatedBuffer.AllocHeap(count, state, func);
		return metadata.Execute(state, func);
	}
	/// <summary>
	/// Retrieves the maximum value in the given binary space.
	/// </summary>
	/// <param name="space">Maximum binary power in the binary space.</param>
	/// <returns>The maximum value in the given binary space.</returns>
	private static UInt16 GetMaxValue(UInt16 space) => (UInt16)(space * 2 - 1);
}