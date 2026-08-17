namespace Rxmxnx.PInvoke;

public partial class BufferTypeMetadata
{
	/// <summary>
	/// Indicates whether the current composition has errors.
	/// </summary>
	/// <param name="typeofT">The type of items in the buffer.</param>
	/// <param name="size">Buffer capacity.</param>
	/// <returns>
	/// <see langword="true"/> if current composition has errors; otherwise <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean HasError(Type typeofT, UInt16 size) => BufferTypeMetadata.HasError(new(typeofT, size));

	/// <summary>
	/// Indicates whether the current composition has errors.
	/// </summary>
	/// <param name="composition">A <see cref="Composition"/> instance.</param>
	/// <returns>
	/// <see langword="true"/> if current composition has errors; otherwise <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static Boolean HasError(Composition composition)
	{
		using ReadScope scope = BufferTypeMetadata.rwLock;
		return BufferTypeMetadata.errors.Contains(composition);
	}
	/// <summary>
	/// Sets as error the current composition.
	/// </summary>
	/// <param name="composition">A <see cref="Composition"/> instance.</param>
	private protected static void SetError(Composition composition)
	{
		using WriteScope scope = BufferTypeMetadata.rwLock;
		BufferTypeMetadata.errors.Add(composition);
	}
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/> interface.</typeparam>
	/// <param name="action">A <see cref="IScopedBufferAction{T}"/> instance.</param>
	/// <param name="metadata">A <see cref="BufferTypeMetadata"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static void Execute<T, TBuffer, TAction>(in TAction action, BufferTypeMetadata metadata,
		Int32 spanLength) where TBuffer : struct
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		TBuffer buffer = new();
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, metadata.Size, metadata);
		action.Accept(scoped);
#else
		Span<T> memMarshal = BufferTypeMetadata.CreateSpan<T, TBuffer>(ref buffer, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, metadata.Size, metadata);
		action.Accept(scoped);
		BufferTypeMetadata.Clear<T, TBuffer>(ref buffer, spanLength);
#endif
	}
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, Result}"/> interface.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="IScopedBufferFunction{T,TResult}"/> instance.</param>
	/// <param name="metadata">A <see cref="BufferTypeMetadata"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static TResult Execute<T, TBuffer, TFunction, TResult>(in TFunction func,
		BufferTypeMetadata metadata, Int32 spanLength) where TBuffer : struct
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		TBuffer buffer = new();
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, metadata.Size, metadata);
		return func.Apply(scoped);
#else
		Span<T> memMarshal = BufferTypeMetadata.CreateSpan<T, TBuffer>(ref buffer, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, metadata.Size, metadata);
		TResult result = func.Apply(scoped);
		BufferTypeMetadata.Clear<T, TBuffer>(ref buffer, spanLength);
		return result;
#endif
	}
}