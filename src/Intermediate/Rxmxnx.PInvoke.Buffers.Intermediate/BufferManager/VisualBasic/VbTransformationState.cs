namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	public static partial class VisualBasic
	{
		/// <summary>
		/// State for transformation object.
		/// </summary>
		/// <typeparam name="T">Type of transformation object.</typeparam>
		/// <param name="action">Action to execute.</param>
		private readonly struct VbTransformationState<T>(VbScopedBufferAction<T> action)
		{
			/// <summary>
			/// Action to execute.
			/// </summary>
			private readonly VbScopedBufferAction<T> _action = action;

			/// <summary>
			/// Executes <paramref name="state"/> internal method.
			/// </summary>
			/// <param name="buffer">Object buffer.</param>
			/// <param name="state">State object.</param>
			public static void Execute(ScopedBuffer<Object> buffer, VbTransformationState<T> state)
			{
				ref Object refObject = ref MemoryMarshal.GetReference(buffer.Span);
				ref T refT = ref Unsafe.As<Object, T>(ref refObject);
				VbScopedBuffer<T> bufferT = new(ref refT, (UInt16)buffer.Span.Length, buffer.BufferMetadata);
				state._action(bufferT);
			}
			/// <summary>
			/// Executes <paramref name="state"/> internal method.
			/// </summary>
			/// <param name="buffer">Object buffer.</param>
			/// <param name="state">State object.</param>
			public static void Execute(ScopedBuffer<T> buffer, VbTransformationState<T> state)
			{
				ref T refT = ref MemoryMarshal.GetReference(buffer.Span);
				VbScopedBuffer<T> bufferT = new(ref refT, (UInt16)buffer.Span.Length, buffer.BufferMetadata);
				state._action(bufferT);
			}
		}

		/// <summary>
		/// State for transformation object.
		/// </summary>
		/// <typeparam name="T">Type of transformation object.</typeparam>
		/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
		/// <param name="func">Function to execute.</param>
		private readonly struct VbTransformationState<T, TResult>(VbScopedBufferFunc<T, TResult> func)
		{
			/// <summary>
			/// Function to execute.
			/// </summary>
			private readonly VbScopedBufferFunc<T, TResult> _func = func;

			/// <summary>
			/// Executes <paramref name="state"/> internal method.
			/// </summary>
			/// <param name="buffer">Object buffer.</param>
			/// <param name="state">State object.</param>
			public static TResult Execute(ScopedBuffer<Object> buffer, VbTransformationState<T, TResult> state)
			{
				ref Object refObject = ref MemoryMarshal.GetReference(buffer.Span);
				ref T refT = ref Unsafe.As<Object, T>(ref refObject);
				VbScopedBuffer<T> bufferT = new(ref refT, (UInt16)buffer.Span.Length, buffer.BufferMetadata);
				return state._func(bufferT);
			}
			/// <summary>
			/// Executes <paramref name="state"/> internal method.
			/// </summary>
			/// <param name="buffer">Object buffer.</param>
			/// <param name="state">State object.</param>
			public static TResult Execute(ScopedBuffer<T> buffer, VbTransformationState<T, TResult> state)
			{
				ref T refT = ref MemoryMarshal.GetReference(buffer.Span);
				VbScopedBuffer<T> bufferT = new(ref refT, (UInt16)buffer.Span.Length, buffer.BufferMetadata);
				return state._func(bufferT);
			}
		}
	}
}