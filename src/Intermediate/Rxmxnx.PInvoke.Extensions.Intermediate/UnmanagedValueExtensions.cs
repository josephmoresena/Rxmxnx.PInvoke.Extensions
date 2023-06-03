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
        => NativeUtilities.ToBytes(value);

    /// <summary>
    /// Creates a binary array from an array of <typeparamref name="TSource"/> values. 
    /// </summary>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="array">Array of <typeparamref name="TSource"/> values.</param>
    /// <returns>A binary array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(array))]
    public static Byte[]? ToBytes<TSource>(this TSource[]? array) where TSource : unmanaged
    {
        if (array is null)
            return default;
        else if (!array.Any())
            return Array.Empty<Byte>();

        return array.AsSpan().AsBytes().ToArray();
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
    [return: NotNullIfNotNull(nameof(array))]
    public static TDestination[]? ToValues<TSource, TDestination>(this TSource[]? array)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        if (array is null)
            return default;
        else if (!array.Any())
            return Array.Empty<TDestination>();

        return array.AsSpan().AsValues<TSource, TDestination>().ToArray();
    }

    /// <summary>
    /// Creates an array of <typeparamref name="TDestination"/> values from an array of 
    /// <typeparamref name="TSource"/> values. 
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="array">Array of <typeparamref name="TSource"/> values.</param>
    /// <param name="residual">The residual binary array of the reinterpretation.</param>
    /// <returns>Array of <typeparamref name="TDestination"/> values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(array))]
    public static TDestination[]? ToValues<TSource, TDestination>(this TSource[]? array, [NotNullIfNotNull(nameof(array))] out Byte[]? residual)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        if (array is null)
        {
            residual = default;
            return default;
        }
        else if (!array.Any())
        {
            residual = Array.Empty<Byte>();
            return Array.Empty<TDestination>();
        }

        TDestination[] result = array.AsSpan().AsValues<TSource, TDestination>(out Span<Byte> rSpan).ToArray();
        residual = rSpan.ToArray();
        return result;
    }
}