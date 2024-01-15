namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/>
/// instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[SuppressMessage("csharpsquid", "S6640")]
public static unsafe partial class MemoryBlockExtensions
{
	/// <summary>
	/// Retrieves an unsafe <see cref="ValPtr{T}"/> pointer from <see cref="Span{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see langword="unmanaged"/> values contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The span from which the pointer is retrieved.</param>
	/// <returns>An <see cref="ValPtr{T}"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the span won't be moved or
	/// collected by garbage collector.
	/// The pointer will point to the address in memory the span had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValPtr<T> GetUnsafeValPtr<T>(this Span<T> span) where T : unmanaged
	{
		ref T refValue = ref MemoryMarshal.GetReference(span);
		void* ptr = Unsafe.AsPointer(ref refValue);
		return new(ptr);
	}
	/// <summary>
	/// Retrieves an unsafe <see cref="IntPtr"/> pointer from <see cref="Span{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see langword="unmanaged"/> values contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The span from which the pointer is retrieved.</param>
	/// <returns>An <see cref="IntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the span won't be moved or
	/// collected by garbage collector.
	/// The pointer will point to the address in memory the span had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr GetUnsafeIntPtr<T>(this Span<T> span) where T : unmanaged
	{
		ref T refValue = ref MemoryMarshal.GetReference(span);
		void* ptr = Unsafe.AsPointer(ref refValue);
		return (IntPtr)ptr;
	}
	/// <summary>
	/// Retrieves an unsafe <see cref="ReadOnlyValPtr{T}"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see langword="unmanaged"/> values contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The read-only span from which the pointer is retrieved.</param>
	/// <returns>An <see cref="ReadOnlyValPtr{T}"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the span won't be moved or
	/// collected by garbage collector.
	/// The pointer will point to the address in memory the span had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlyValPtr<T> GetUnsafeValPtr<T>(this ReadOnlySpan<T> span) where T : unmanaged
	{
		ref T refValue = ref MemoryMarshal.GetReference(span);
		void* ptr = Unsafe.AsPointer(ref refValue);
		return new(ptr);
	}
	/// <summary>
	/// Retrieves an unsafe <see cref="IntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see langword="unmanaged"/> values contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The read-only span from which the pointer is retrieved.</param>
	/// <returns>An <see cref="IntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the span won't be moved or
	/// collected by garbage collector.
	/// The pointer will point to the address in memory the span had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr GetUnsafeIntPtr<T>(this ReadOnlySpan<T> span) where T : unmanaged
	{
		ref T refValue = ref MemoryMarshal.GetReference(span);
		void* ptr = Unsafe.AsPointer(ref refValue);
		return (IntPtr)ptr;
	}

	/// <summary>
	/// Retrieves an unsafe <see cref="UIntPtr"/> pointer from <see cref="Span{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see langword="unmanaged"/> values contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The span from which the pointer is retrieved.</param>
	/// <returns>An <see cref="UIntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the span won't be moved or
	/// collected by garbage collector.
	/// The pointer will point to the address in memory the span had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr GetUnsafeUIntPtr<T>(this Span<T> span) where T : unmanaged
	{
		ref T refValue = ref MemoryMarshal.GetReference(span);
		void* ptr = Unsafe.AsPointer(ref refValue);
		return (UIntPtr)ptr;
	}
	/// <summary>
	/// Retrieves an unsafe <see cref="UIntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see langword="unmanaged"/> values contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The read-only span from which the pointer is retrieved.</param>
	/// <returns>An <see cref="UIntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the span won't be moved or
	/// collected by garbage collector.
	/// The pointer will point to the address in memory the span had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr GetUnsafeUIntPtr<T>(this ReadOnlySpan<T> span) where T : unmanaged
	{
		ref T refValue = ref MemoryMarshal.GetReference(span);
		void* ptr = Unsafe.AsPointer(ref refValue);
		return (UIntPtr)ptr;
	}

	/// <summary>
	/// Reinterprets <paramref name="span"/> as binary span.
	/// </summary>
	/// <typeparam name="TSource">The type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A span of <typeparamref name="TSource"/>.</param>
	/// <returns>A binary span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<Byte> AsBytes<TSource>(this Span<TSource> span) where TSource : unmanaged
		=> MemoryMarshal.AsBytes(span);
	/// <summary>
	/// Reinterprets <paramref name="span"/> as binary span.
	/// </summary>
	/// <typeparam name="TSource">The type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="TSource"/>.</param>
	/// <returns>A binary span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<Byte> AsBytes<TSource>(this ReadOnlySpan<TSource> span) where TSource : unmanaged
		=> MemoryMarshal.AsBytes(span);

	/// <summary>
	/// Reinterprets <paramref name="span"/> as a span of <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">The destination type of the <see langword="unmanaged"/> value.</typeparam>
	/// <typeparam name="TSource">The origin type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A span of <typeparamref name="TSource"/>.</param>
	/// <returns>A span of <typeparamref name="TDestination"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<TDestination> AsValues<TSource, TDestination>(this Span<TSource> span)
		where TSource : unmanaged where TDestination : unmanaged
		=> MemoryMarshal.Cast<TSource, TDestination>(span);
	/// <summary>
	/// Reinterprets <paramref name="span"/> as a read-only span of <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">The destination type of the <see langword="unmanaged"/> value.</typeparam>
	/// <typeparam name="TSource">The origin type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="TSource"/>.</param>
	/// <returns>A read-only span of <typeparamref name="TDestination"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<TDestination> AsValues<TSource, TDestination>(this ReadOnlySpan<TSource> span)
		where TSource : unmanaged where TDestination : unmanaged
		=> MemoryMarshal.Cast<TSource, TDestination>(span);
	/// <summary>
	/// Reinterprets <paramref name="span"/> as a span of <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">The destination type of the <see langword="unmanaged"/> value.</typeparam>
	/// <typeparam name="TSource">The origin type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A span of <typeparamref name="TSource"/>.</param>
	/// <param name="residual">
	/// Output parameter that holds the residual binary data that could not be converted into
	/// <typeparamref name="TDestination"/>.
	/// </param>
	/// <returns>A span of <typeparamref name="TDestination"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<TDestination> AsValues<TSource, TDestination>(this Span<TSource> span, out Span<Byte> residual)
		where TSource : unmanaged where TDestination : unmanaged
	{
		Span<TDestination> result = MemoryMarshal.Cast<TSource, TDestination>(span);
		Int32 offset = result.Length * sizeof(TDestination) / sizeof(TSource);
		residual = MemoryMarshal.AsBytes(span[offset..]);
		return result;
	}
	/// <summary>
	/// Reinterprets <paramref name="span"/> as a read-only span of <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">The destination type of the <see langword="unmanaged"/> value.</typeparam>
	/// <typeparam name="TSource">The origin type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A span of <typeparamref name="TSource"/>.</param>
	/// <param name="residual">
	/// Output parameter that holds the residual binary data that could not be converted into
	/// <typeparamref name="TDestination"/>.
	/// </param>
	/// <returns>A read-only span of <typeparamref name="TDestination"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<TDestination> AsValues<TSource, TDestination>(this Span<TSource> span,
		out ReadOnlySpan<Byte> residual) where TSource : unmanaged where TDestination : unmanaged
	{
		ReadOnlySpan<TDestination> result = MemoryMarshal.Cast<TSource, TDestination>(span);
		Int32 offset = result.Length * sizeof(TDestination) / sizeof(TSource);
		residual = MemoryMarshal.AsBytes(span[offset..]);
		return result;
	}
	/// <summary>
	/// Reinterprets <paramref name="span"/> as a read-only span of <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">The destination type of the <see langword="unmanaged"/> value.</typeparam>
	/// <typeparam name="TSource">The origin type of the <see langword="unmanaged"/> value.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="TSource"/>.</param>
	/// <param name="residual">
	/// Output parameter that holds the residual binary data that could not be converted into
	/// <typeparamref name="TDestination"/>.
	/// </param>
	/// <returns>A read-only span of <typeparamref name="TDestination"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<TDestination> AsValues<TSource, TDestination>(this ReadOnlySpan<TSource> span,
		out ReadOnlySpan<Byte> residual) where TSource : unmanaged where TDestination : unmanaged
	{
		ReadOnlySpan<TDestination> result = MemoryMarshal.Cast<TSource, TDestination>(span);
		Int32 offset = result.Length * sizeof(TDestination) / sizeof(TSource);
		residual = MemoryMarshal.AsBytes(span[offset..]);
		return result;
	}
	/// <summary>
	/// Creates an <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance by pinning the current
	/// <see cref="ReadOnlyMemory{T}"/> instance, providing a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type from which the contiguous region of memory will be read.
	/// </typeparam>
	/// <param name="mem">A <see cref="ReadOnlyMemory{T}"/> instance.</param>
	/// <returns>An <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	public static IReadOnlyFixedContext<T>.IDisposable GetFixedContext<T>(this ReadOnlyMemory<T> mem)
		where T : unmanaged
	{
		MemoryHandle handle = mem.Pin();
		return MemoryMarshal.TryGetMemoryManager<T, MemoryManager<T>>(mem, out _) ||
			MemoryMarshal.TryGetArray(mem, out _) ?
				new FixedContext<T>(handle.Pointer, mem.Length).ToDisposable(handle) :
				new ReadOnlyFixedContext<T>(handle.Pointer, mem.Length).ToDisposable(handle);
	}
	/// <summary>
	/// Creates an <see cref="IFixedContext{T}.IDisposable"/> instance by pinning the current
	/// <see cref="Memory{T}"/> instance, allowing safe access to the fixed memory region.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type of items in the <see cref="Memory{T}"/>.
	/// </typeparam>
	/// <param name="mem">A <see cref="Memory{T}"/> instance.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	public static IFixedContext<T>.IDisposable GetFixedContext<T>(this Memory<T> mem) where T : unmanaged
	{
		MemoryHandle handle = mem.Pin();
		return new FixedContext<T>(handle.Pointer, mem.Length).ToDisposable(handle);
	}
}