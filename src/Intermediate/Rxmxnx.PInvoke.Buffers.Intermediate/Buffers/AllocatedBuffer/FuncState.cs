namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	private readonly struct FunctionState<T, TResult>(AllocatedBufferFunc<T, TResult> func)
	{
		private readonly AllocatedBufferFunc<T, TResult> _func = func;
		public static TResult Execute(AllocatedBuffer<Object> buffer, FunctionState<T, TResult> state)
		{
			Span<T> span =
				MemoryMarshal.CreateSpan(ref Unsafe.As<Object, T>(ref MemoryMarshal.GetReference(buffer.Span)),
				                         buffer.Span.Length);
			AllocatedBuffer<T> bufferT = new(span, !buffer.InStack);
			return state._func(bufferT);
		}
	}

	private readonly struct FunctionState<T, TState, TResult>(
		AllocatedBufferFunc<T, TState, TResult> func,
		TState state)
	{
		private readonly AllocatedBufferFunc<T, TState, TResult> _func = func;
		private readonly TState _state = state;

		public static TResult Execute(AllocatedBuffer<Object> buffer, FunctionState<T, TState, TResult> state)
		{
			Span<T> span =
				MemoryMarshal.CreateSpan(ref Unsafe.As<Object, T>(ref MemoryMarshal.GetReference(buffer.Span)),
				                         buffer.Span.Length);
			AllocatedBuffer<T> bufferT = new(span, !buffer.InStack);
			return state._func(bufferT, state._state);
		}
	}
}