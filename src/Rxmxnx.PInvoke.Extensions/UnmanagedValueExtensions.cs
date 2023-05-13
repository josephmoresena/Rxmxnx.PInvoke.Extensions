namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="ValueType"/> <see langword="unmanaged"/> values.
/// </summary>
public static partial class UnmanagedValueExtensions
{
    /// <summary>
    /// Retrieves a <see cref="Byte"/> array from the given <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="value"><typeparamref name="T"/> value.</param>
    /// <returns><see cref="Byte"/> array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Byte[] ToBytes<T>(this T value) where T : unmanaged
    {
        ReadOnlySpan<T> intermediateSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
        ReadOnlySpan<Byte> bytes = MemoryMarshal.AsBytes(intermediateSpan);
        return bytes.ToArray();
    }

    /// <summary>
    /// Creates an array of <typeparamref name="TDestination"/> values from an array of 
    /// <typeparamref name="TSource"/> values. 
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="array">Array of <typeparamref name="TSource"/> values.</param>
    /// <returns>Array of <typeparamref name="TDestination"/> values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TDestination[]? ToValues<TSource, TDestination>(this TSource[] array)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        if (array is null)
            return default;

        if (!array.Any())
            return Array.Empty<TDestination>();

        return MemoryMarshal.Cast<TSource, TDestination>(array).ToArray();
    }
}