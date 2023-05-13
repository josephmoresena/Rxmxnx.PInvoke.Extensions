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
        => GetConditionalIntPtrZero(span.IsEmpty) ??
        new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)));

    /// <summary>
    /// Retrieves an unsafe <see cref="IntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="readonlySpan">The read-only span from which the pointer is retrieved.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe IntPtr GetIntPtr<T>(this ReadOnlySpan<T> readonlySpan) where T : unmanaged
        => GetConditionalIntPtrZero(readonlySpan.IsEmpty) ??
        new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(readonlySpan)));

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
        => GetConditionalUIntPtrZero(span.IsEmpty) ??
        new UIntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)));

    /// <summary>
    /// Retrieves an unsafe <see cref="UIntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="readonlySpan">The read-only span from which the pointer is retrieved.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    public static unsafe UIntPtr GetUIntPtr<T>(this ReadOnlySpan<T> readonlySpan) where T : unmanaged
        => GetConditionalUIntPtrZero(readonlySpan.IsEmpty) ??
        new UIntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(readonlySpan)));

    /// <summary>
    /// Gets a <see cref="Nullable{IntPtr}"/> from a given condition. 
    /// </summary>
    /// <param name="condition">Indicates whether the <see cref="IntPtr.Zero"/> must be returned.</param>
    /// <returns>
    /// <see cref="IntPtr.Zero"/> if <paramref name="condition"/> is <see langword="true"/>; otherwise,
    /// <see langword="null"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntPtr? GetConditionalIntPtrZero(Boolean condition) => condition ? IntPtr.Zero : default(IntPtr?);

    /// <summary>
    /// Gets a <see cref="Nullable{UIntPtr}"/> from a given condition. 
    /// </summary>
    /// <param name="condition">Indicates whether the <see cref="UIntPtr.Zero"/> must be returned.</param>
    /// <returns>
    /// <see cref="UIntPtr.Zero"/> if <paramref name="condition"/> is <see langword="true"/>; otherwise,
    /// <see langword="null"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static UIntPtr? GetConditionalUIntPtrZero(Boolean condition) => condition ? UIntPtr.Zero : default(UIntPtr?);
}