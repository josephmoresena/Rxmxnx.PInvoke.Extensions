namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with references to <see cref="ValueType"/> 
/// <see langword="unmanaged"/> values.
/// </summary>
public static partial class ReferenceExtensions
{
    /// <summary>
    /// Retrieves an unsafe <see cref="IntPtr"/> pointer from a reference to a <typeparamref name="T"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
    /// <param name="refValue">A reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe IntPtr GetUnsafeIntPtr<T>(ref this T refValue) where T : unmanaged
    {
        void* ptr = Unsafe.AsPointer(ref refValue);
        return (IntPtr)ptr;
    }

    /// <summary>
    /// Retrieves an unsafe <see cref="UIntPtr"/> pointer from a reference to a <typeparamref name="T"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
    /// <param name="refValue">A reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe UIntPtr GetUnsafeUIntPtr<T>(ref this T refValue) where T : unmanaged
    {
        void* ptr = Unsafe.AsPointer(ref refValue);
        return (UIntPtr)ptr;
    }

    /// <summary>
    /// Creates a reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value from 
    /// an exising reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
    /// <typeparam name="TDestination"><see cref="ValueType"/> of the destination reference.</typeparam>
    /// <param name="refValue">A reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
    /// <returns>A reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value.</returns>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref TDestination Transform<TSource, TDestination>(this ref TSource refValue)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        ValidationUtilities.ThrowIfInvalidCastType<TSource, TDestination>();
        return ref Unsafe.As<TSource, TDestination>(ref refValue);
    }

    /// <summary>
    /// Creates a <see cref="Span{Byte}"/> from an exising reference to a <typeparamref name="TSource"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
    /// <param name="refValue">A reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
    /// <returns>
    /// A <see cref="Span{Byte}"/> from an exising reference to a <typeparamref name="TSource"/> 
    /// <see langword="unmanaged"/> value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Byte> AsBytes<TSource>(this ref TSource refValue) where TSource : unmanaged
    {
        Span<TSource> span = MemoryMarshal.CreateSpan(ref refValue, 1);
        return MemoryMarshal.AsBytes(span);
    }
}