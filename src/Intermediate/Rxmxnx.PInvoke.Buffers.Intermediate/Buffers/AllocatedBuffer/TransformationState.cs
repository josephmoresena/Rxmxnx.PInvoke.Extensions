namespace Rxmxnx.PInvoke.Buffers;

public static partial class AllocatedBuffer
{
	/// <summary>
	/// State for transformation object.
	/// </summary>
	/// <typeparam name="T">Type of transformation object.</typeparam>
	/// <param name="action">Action to execute.</param>
	private readonly struct TransformationState<T>(AllocatedBufferAction<T> action)
	{
		/// <summary>
		/// Action to execute.
		/// </summary>
		private readonly AllocatedBufferAction<T> _action = action;

		/// <summary>
		/// Executes <paramref name="state"/> internal method.
		/// </summary>
		/// <param name="buffer">Object buffer.</param>
		/// <param name="state">State object.</param>
		public static void Execute(AllocatedBuffer<Object> buffer, in TransformationState<T> state)
		{
			Span<T> span =
				MemoryMarshal.CreateSpan(ref Unsafe.As<Object, T>(ref MemoryMarshal.GetReference(buffer.Span)),
				                         buffer.Span.Length);
			AllocatedBuffer<T> bufferT = new(span, !buffer.InStack, buffer.FullLength);
			state._action(bufferT);
		}
	}

	/// <summary>
	/// State for transformation object.
	/// </summary>
	/// <typeparam name="T">Type of transformation object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">Function to execute.</param>
	private readonly struct TransformationState<T, TResult>(AllocatedBufferFunc<T, TResult> func)
	{
		/// <summary>
		/// Function to execute.
		/// </summary>
		private readonly AllocatedBufferFunc<T, TResult> _func = func;

		/// <summary>
		/// Executes <paramref name="state"/> internal method.
		/// </summary>
		/// <param name="buffer">Object buffer.</param>
		/// <param name="state">State object.</param>
		public static TResult Execute(AllocatedBuffer<Object> buffer, in TransformationState<T, TResult> state)
		{
			Span<T> span =
				MemoryMarshal.CreateSpan(ref Unsafe.As<Object, T>(ref MemoryMarshal.GetReference(buffer.Span)),
				                         buffer.Span.Length);
			AllocatedBuffer<T> bufferT = new(span, !buffer.InStack, buffer.FullLength);
			return state._func(bufferT);
		}
	}
}