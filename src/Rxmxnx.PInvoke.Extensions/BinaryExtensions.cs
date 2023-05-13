namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Byte"/> instances.
/// </summary>
public static partial class BinaryExtensions
{
    /// <summary>
    /// Retrieves a <typeparamref name="T"/> value from the given <see cref="Byte"/> array.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="array"><see cref="Byte"/> array.</param>
    /// <returns><typeparamref name="T"/> value read.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToValue<T>(this Byte[] array) where T : unmanaged => array.AsSpan().ToValue<T>();

    /// <summary>
    /// Retrieves a <typeparamref name="T"/> value from the given <see cref="Byte"/> array.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span"><see cref="Byte"/> span.</param>
    /// <returns><typeparamref name="T"/> value read.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToValue<T>(this Span<Byte> span) where T : unmanaged => ToValue<T>((ReadOnlySpan<Byte>)span);

    /// <summary>
    /// Retrieves a <typeparamref name="T"/> value from the given <see cref="Byte"/> array.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span"><see cref="Byte"/> read-only span.</param>
    /// <returns><typeparamref name="T"/> value read.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ToValue<T>(this ReadOnlySpan<Byte> span) where T : unmanaged
    {
        Span<Byte> result = stackalloc Byte[sizeof(T)];
        Int32 bytesToCopy = Math.Min(result.Length, span.Length);

        span[..bytesToCopy].CopyTo(result);
        return MemoryMarshal.Read<T>(span);
    }

    /// <summary>
    /// Retrieves a <typeparamref name="T"/> read-only reference from <paramref name="span"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span"><see cref="Byte"/> read-only span.</param>
    /// <returns><typeparamref name="T"/> read-only reference.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T AsValue<T>(this ReadOnlySpan<Byte> span) where T : unmanaged
    {
        ThrowIfInvalidSize<T>(span.Length, nameof(span));
        return ref MemoryMarshal.Cast<Byte, T>(span)[0];
    }

    /// <summary>
    /// Retrieves a <typeparamref name="T"/> reference from <paramref name="span"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="span"><see cref="Byte"/> span.</param>
    /// <returns><typeparamref name="T"/> reference.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T AsValue<T>(this Span<Byte> span) where T : unmanaged
    {
        ThrowIfInvalidSize<T>(span.Length, nameof(span));
        return ref MemoryMarshal.Cast<Byte, T>(span)[0];
    }

    /// <summary>
    /// Gets <see cref="String"/> representation of <see cref="Byte"/> array hexadecimal value.
    /// </summary>
    /// <param name="bytes"><see cref="Byte"/> array.</param>
    /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static String AsHexString(this Byte[] bytes) => String.Concat(bytes.Select(b => b.AsHexString()));

    /// <summary>
    /// Gets <see cref="String"/> representation of <see cref="Byte"/> hexadecimal value.
    /// </summary>
    /// <param name="value"><see cref="Byte"/> value.</param>
    /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static String AsHexString(this Byte value) => value.ToString("X2").ToLower();

    /// <summary>
    /// Validates the <paramref name="length"/> in order to read exactly one <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="length">Length of a binary span.</param>
    /// <param name="spanName">Name of the span.</param>
    /// <exception cref="ArgumentException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void ThrowIfInvalidSize<T>(Int32 length, String spanName) where T : unmanaged
    {
        Int32 typeSize = sizeof(T);
        if (length != typeSize)
            throw new InsufficientMemoryException($"The length of parameter {spanName} must be equals to {typeSize}.");
    }
}

