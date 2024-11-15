namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	private readonly struct ActionState<T>(AllocatedBufferAction<T> action)
	{
		private readonly AllocatedBufferAction<T> _action = action;
		public static void Execute(AllocatedBuffer<Object> buffer, in ActionState<T> state)
		{
			Span<T> span =
				MemoryMarshal.CreateSpan(ref Unsafe.As<Object, T>(ref MemoryMarshal.GetReference(buffer.Span)),
				                         buffer.Span.Length);
			AllocatedBuffer<T> bufferT = new(span, !buffer.InStack);
			state._action(bufferT);
		}
	}

	private readonly struct ActionState<T, TState>(AllocatedBufferAction<T, TState> action, TState state)
	{
		private readonly AllocatedBufferAction<T, TState> _action = action;
		private readonly TState _state = state;

		public static void Execute(AllocatedBuffer<Object> buffer, in ActionState<T, TState> state)
		{
			Span<T> span =
				MemoryMarshal.CreateSpan(ref Unsafe.As<Object, T>(ref MemoryMarshal.GetReference(buffer.Span)),
				                         buffer.Span.Length);
			AllocatedBuffer<T> bufferT = new(span, !buffer.InStack);
			state._action(bufferT, state._state);
		}
	}
}