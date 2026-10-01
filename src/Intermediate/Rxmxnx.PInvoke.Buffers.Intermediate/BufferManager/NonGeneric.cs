using Rxmxnx.PInvoke.Internal.FrameworkCompat;

namespace Rxmxnx.PInvoke;

public static partial class BufferManager<T>
{
	/// <summary>
	/// Represents a non-generic wrapper around an <see cref="IScopedBufferAction{T}"/> implementation.
	/// </summary>
	[Preserve(AllMembers = true)]
	internal readonly struct NonGenericAction : IScopedBufferAction<T>
	{
		/// <summary>
		/// Internal value.
		/// </summary>
		private IScopedBufferAction<T> Value { get; init; }

		Boolean IScopedBufferAction<T>.IsMinimalCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.Value.IsMinimalCount;
		}
		UInt16 IScopedBufferAction<T>.Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.Value.Count;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void IScopedBufferAction<T>.Accept(scoped ScopedBuffer<T> buffer) => this.Value.Accept(buffer);

		/// <summary>
		/// Creates a new <see cref="NonGenericAction"/> instance.
		/// </summary>
		/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
		/// <param name="action">Action to perform with the allocated buffer.</param>
		/// <returns>A new <see cref="NonGenericAction"/> instance.</returns>
		public static NonGenericAction Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
			where TAction : IScopedBufferAction<T>
		{
			Debug.Assert(!typeof(TAction).IsValueType);
			return new() { Value = action, };
		}
#else
			where TAction : IScopedBufferAction<T>, allows ref struct
		{
			Debug.Assert(!typeof(TAction).IsValueType);
			return new() { Value = Unsafe.As<TAction, IScopedBufferAction<T>>(ref action), };
		}
#endif
	}

	/// <summary>
	/// Represents a non-generic wrapper around an <see cref="IScopedBufferFunction{T, TResult}"/> implementation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
	[Preserve(AllMembers = true)]
	internal readonly struct NonGenericFunction<TResult> : IScopedBufferFunction<T, TResult>
	{
		/// <summary>
		/// Internal value.
		/// </summary>
		private IScopedBufferFunction<T, TResult> Value { get; init; }

		Boolean IScopedBufferFunction<T, TResult>.IsMinimalCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.Value.IsMinimalCount;
		}
		UInt16 IScopedBufferFunction<T, TResult>.Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.Value.Count;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		TResult IScopedBufferFunction<T, TResult>.Apply(scoped ScopedBuffer<T> buffer) => this.Value.Apply(buffer);

		/// <summary>
		/// Creates a new <see cref="NonGenericFunction{TResult}"/> instance.
		/// </summary>
		/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TResult}"/>.</typeparam>
		/// <param name="func">Action to perform with the allocated buffer.</param>
		/// <returns>A new <see cref="NonGenericFunction{TResult}"/> instance.</returns>
		public static NonGenericFunction<TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
			where TFunction : IScopedBufferFunction<T, TResult>
		{
			Debug.Assert(!typeof(TFunction).IsValueType);
			return new() { Value = func, };
		}
#else
			where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
		{
			Debug.Assert(!typeof(TFunction).IsValueType);
			return new() { Value = Unsafe.As<TFunction, IScopedBufferFunction<T, TResult>>(ref func), };
		}
#endif
	}
}