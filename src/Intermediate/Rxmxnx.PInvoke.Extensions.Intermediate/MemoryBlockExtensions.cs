namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> instances.
/// </summary>
public static partial class MemoryBlockExtensions
{
    /// <summary>
    /// Retrieves an unsafe <see cref="IntPtr"/> pointer from <see cref="Span{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">The span from which the pointer is retrieved.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe IntPtr GetIntPtr<T>(this Span<T> span) where T : unmanaged
    {
        ref T refValue = ref MemoryMarshal.GetReference(span);
        void* ptr = Unsafe.AsPointer(ref refValue);
        return (IntPtr)ptr;
    }

    /// <summary>
    /// Retrieves an unsafe <see cref="IntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">The read-only span from which the pointer is retrieved.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe IntPtr GetIntPtr<T>(this ReadOnlySpan<T> span) where T : unmanaged
    {
        ref T refValue = ref MemoryMarshal.GetReference(span);
        void* ptr = Unsafe.AsPointer(ref refValue);
        return (IntPtr)ptr;
    }

    /// <summary>
    /// Retrieves an unsafe <see cref="UIntPtr"/> pointer from <see cref="Span{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">The span from which the pointer is retrieved.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe UIntPtr GetUIntPtr<T>(this Span<T> span) where T : unmanaged
    {
        ref T refValue = ref MemoryMarshal.GetReference(span);
        void* ptr = Unsafe.AsPointer(ref refValue);
        return (UIntPtr)ptr;
    }

    /// <summary>
    /// Retrieves an unsafe <see cref="UIntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">The read-only span from which the pointer is retrieved.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe UIntPtr GetUIntPtr<T>(this ReadOnlySpan<T> span) where T : unmanaged
    {
        ref T refValue = ref MemoryMarshal.GetReference(span);
        void* ptr = Unsafe.AsPointer(ref refValue);
        return (UIntPtr)ptr;
    }

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as binary span.
    /// </summary>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> span.</param>
    /// <returns>A binary span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Byte> AsBytes<TSource>(this Span<TSource> span) where TSource : unmanaged
        => MemoryMarshal.AsBytes(span);

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as binary read-only span.
    /// </summary>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> span.</param>
    /// <returns>A binary read-only span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Byte> AsBytes<TSource>(this ReadOnlySpan<TSource> span) where TSource : unmanaged
        => MemoryMarshal.AsBytes(span);

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as <typeparamref name="TDestination"/> span.
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> span.</param>
    /// <returns>A <typeparamref name="TDestination"/> span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<TDestination> AsValues<TSource, TDestination>(this Span<TSource> span)
        where TSource : unmanaged
        where TDestination : unmanaged
        => MemoryMarshal.Cast<TSource, TDestination>(span);

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as <typeparamref name="TDestination"/> read-only span.
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> read-only span.</param>
    /// <returns>A <typeparamref name="TDestination"/> read-only span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<TDestination> AsValues<TSource, TDestination>(this ReadOnlySpan<TSource> span)
        where TSource : unmanaged
        where TDestination : unmanaged
        => MemoryMarshal.Cast<TSource, TDestination>(span);

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as <typeparamref name="TDestination"/> span.
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> span.</param>
    /// <param name="residual">The residual binary span of the reinterpretation.</param>
    /// <returns>A <typeparamref name="TDestination"/> span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<TDestination> AsValues<TSource, TDestination>(this Span<TSource> span, out Span<Byte> residual)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        Span<TDestination> result = MemoryMarshal.Cast<TSource, TDestination>(span);
        Int32 offset = result.Length * sizeof(TDestination) / sizeof(TSource);
        residual = MemoryMarshal.AsBytes(span[offset..]);
        return result;
    }

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as <typeparamref name="TDestination"/> read-only span.
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> span.</param>
    /// <param name="residual">The residual binary read-only span of the reinterpretation.</param>
    /// <returns>A <typeparamref name="TDestination"/> read-only span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ReadOnlySpan<TDestination> AsValues<TSource, TDestination>(this Span<TSource> span, out ReadOnlySpan<Byte> residual)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        ReadOnlySpan<TDestination> result = MemoryMarshal.Cast<TSource, TDestination>(span);
        Int32 offset = result.Length * sizeof(TDestination) / sizeof(TSource);
        residual = MemoryMarshal.AsBytes(span[offset..]);
        return result;
    }

    /// <summary>
    /// Reinterprets a <paramref name="span"/> as <typeparamref name="TDestination"/> read-only span.
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span">A <typeparamref name="TSource"/> read-only span.</param>
    /// <param name="residual">The residual binary read-only span of the reinterpretation.</param>
    /// <returns>A <typeparamref name="TDestination"/> read-only span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ReadOnlySpan<TDestination> AsValues<TSource, TDestination>(this ReadOnlySpan<TSource> span, out ReadOnlySpan<Byte> residual)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        ReadOnlySpan<TDestination> result = MemoryMarshal.Cast<TSource, TDestination>(span);
        Int32 offset = result.Length * sizeof(TDestination) / sizeof(TSource);
        residual = MemoryMarshal.AsBytes(span[offset..]);
        return result;
    }
}