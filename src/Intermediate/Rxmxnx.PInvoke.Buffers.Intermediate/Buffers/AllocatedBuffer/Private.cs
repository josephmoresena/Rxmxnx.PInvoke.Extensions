namespace Rxmxnx.PInvoke.Buffers;

public static partial class AllocatedBuffer
{
	private static void AllocHeap<T>(UInt16 count, AllocatedBufferAction<T> action)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		action(buffer);
	}
	private static void AllocHeap<T, TState>(UInt16 count, TState state, AllocatedBufferAction<T, TState> action)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		action(buffer, state);
	}
	private static TResult AllocHeap<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		return func(buffer);
	}
	private static TResult AllocHeap<T, TState, TResult>(UInt16 count, TState state,
		AllocatedBufferFunc<T, TState, TResult> func)
	{
		T[] arr = ArrayPool<T>.Shared.Rent(count);
		Span<T> span = arr.AsSpan()[..count];
		AllocatedBuffer<T> buffer = new(span, true);
		return func(buffer, state);
	}
	private static void AllocObject<T>(UInt16 count, AllocatedBufferAction<T> action)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null)
		{
			AllocatedBuffer.AllocHeap(count, action);
			return;
		}
		ActionState<T> stateT = new(action);
		metadata.Execute(stateT, ActionState<T>.Execute);
	}
	private static void AllocObject<T, TState>(UInt16 count, TState state, AllocatedBufferAction<T, TState> action)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null)
		{
			AllocatedBuffer.AllocHeap(count, state, action);
			return;
		}
		ActionState<T, TState> stateT = new(action, state);
		metadata.Execute(stateT, ActionState<T, TState>.Execute);
	}
	private static TResult AllocObject<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null)
			return AllocatedBuffer.AllocHeap(count, func);
		FunctionState<T, TResult> stateT = new(func);
		return metadata.Execute(stateT, FunctionState<T, TResult>.Execute);
	}
	private static TResult AllocObject<T, TState, TResult>(UInt16 count, TState state,
		AllocatedBufferFunc<T, TState, TResult> func)
	{
		IBufferTypeMetadata<Object>? metadata = MetadataCache<Object>.GetMetadata(count);
		if (metadata is null)
			return AllocatedBuffer.AllocHeap(count, state, func);
		FunctionState<T, TState, TResult> stateT = new(func, state);
		return metadata.Execute(stateT, FunctionState<T, TState, TResult>.Execute);
	}
	private static void AllocValue<T>(UInt16 count, AllocatedBufferAction<T> action)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null)
		{
			AllocatedBuffer.AllocHeap(count, action);
			return;
		}
		metadata.Execute(action);
	}
	private static void AllocValue<T, TState>(UInt16 count, TState state, AllocatedBufferAction<T, TState> action)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null)
		{
			AllocatedBuffer.AllocHeap(count, state, action);
			return;
		}
		metadata.Execute(state, action);
	}
	private static TResult AllocValue<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null)
			return AllocatedBuffer.AllocHeap(count, func);
		return metadata.Execute(func);
	}
	private static TResult AllocValue<T, TState, TResult>(UInt16 count, TState state,
		AllocatedBufferFunc<T, TState, TResult> func)
	{
		IBufferTypeMetadata<T>? metadata = MetadataCache<T>.GetMetadata(count);
		if (metadata is null)
			return AllocatedBuffer.AllocHeap(count, state, func);
		return metadata.Execute(state, func);
	}
	private static UInt16 GetMaxValue(UInt16 space) => (UInt16)(space * 2 - 1);
}