namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// State for transformation object.
	/// </summary>
	/// <typeparam name="T">Type of transformation object.</typeparam>
	/// <param name="action">Action to execute.</param>
	private readonly struct TransformationState<T>(ScopedBufferAction<T> action)
	{
		/// <summary>
		/// Action to execute.
		/// </summary>
		private readonly ScopedBufferAction<T> _action = action;

		/// <summary>
		/// Executes <paramref name="state"/> internal method.
		/// </summary>
		/// <param name="buffer">Object buffer.</param>
		/// <param name="state">State object.</param>
		public static void Execute(ScopedBuffer<Object> buffer, TransformationState<T> state)
		{
			ref Object refObject = ref MemoryMarshal.GetReference(buffer.Span);
			ref T refT = ref Unsafe.As<Object, T>(ref refObject);
			Span<T> span = MemoryMarshal.CreateSpan(ref refT, buffer.Span.Length);
			ScopedBuffer<T> bufferT = new(span, !buffer.InStack, buffer.FullLength, buffer.BufferMetadata);
			state._action(bufferT);
		}
	}

	/// <summary>
	/// State for transformation object.
	/// </summary>
	/// <typeparam name="T">Type of transformation object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">Function to execute.</param>
	private readonly struct TransformationState<T, TResult>(ScopedBufferFunc<T, TResult> func)
	{
		/// <summary>
		/// Function to execute.
		/// </summary>
		private readonly ScopedBufferFunc<T, TResult> _func = func;

		/// <summary>
		/// Executes <paramref name="state"/> internal method.
		/// </summary>
		/// <param name="buffer">Object buffer.</param>
		/// <param name="state">State object.</param>
		public static TResult Execute(ScopedBuffer<Object> buffer, TransformationState<T, TResult> state)
		{
			ref Object refObject = ref MemoryMarshal.GetReference(buffer.Span);
			ref T refT = ref Unsafe.As<Object, T>(ref refObject);
			Span<T> span = MemoryMarshal.CreateSpan(ref refT, buffer.Span.Length);
			ScopedBuffer<T> bufferT = new(span, !buffer.InStack, buffer.FullLength, buffer.BufferMetadata);
			return state._func(bufferT);
		}
	}
}