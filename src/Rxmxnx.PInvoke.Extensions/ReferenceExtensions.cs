namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with memory references to <see cref="ValueType"/> 
/// <see langword="unmanaged"/> values.
/// </summary>
public static partial class ReferenceExtensions
{
    /// <summary>
    /// Retrieves an unsafe <see cref="IntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
    /// <param name="refValue">Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    public static unsafe IntPtr GetIntPtr<T>(ref this T refValue) where T : unmanaged
        => new IntPtr(Unsafe.AsPointer(ref refValue));

    /// <summary>
    /// Retrieves an unsafe <see cref="UIntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
    /// <param name="refValue">Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    public static unsafe UIntPtr GetUIntPtr<T>(ref this T refValue) where T : unmanaged
        => new UIntPtr(Unsafe.AsPointer(ref refValue));

    /// <summary>
    /// Creates a memory reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value from 
    /// an exising memory reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
    /// <typeparam name="TDestination"><see cref="ValueType"/> of the destination reference.</typeparam>
    /// <param name="refValue">Memory reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
    /// <returns>A memory reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value.</returns>
    /// <exception cref="InvalidOperationException"/>
    public static unsafe ref TDestination AsReferenceOf<TSource, TDestination>(this ref TSource refValue)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        if (sizeof(TDestination) != sizeof(TSource))
            throw new InvalidOperationException("The sizes of both source and destination unmanaged types must be equal.");
        return ref Unsafe.As<TSource, TDestination>(ref refValue);
    }

    /// <summary>
    /// Creates a <see cref="Span{T}"/> from an exising memory reference to a <typeparamref name="TSource"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
    /// <param name="refValue">>Memory reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
    /// <returns>
    /// A <see cref="Span{T}"/> from an exising memory reference to a <typeparamref name="TSource"/> 
    /// <see langword="unmanaged"/> value.
    /// </returns>
    public static unsafe Span<Byte> AsBytes<TSource>(this ref TSource refValue) where TSource : unmanaged
        => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref refValue, sizeof(TSource)));
}