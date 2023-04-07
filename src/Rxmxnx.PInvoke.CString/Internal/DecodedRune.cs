namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents an decoded <see cref="Rune"/> instance.
/// </summary>
internal sealed class DecodedRune : IWrapper<Rune>
{
    /// <summary>
    /// Internal <see cref="Rune"/> instance.
    /// </summary>
    private readonly Rune _value;
    /// <summary>
    /// The number of units read to create this instance.
    /// </summary>
    private readonly Int32 _charsConsumed;
    /// <summary>
    /// The integer value read at the creation of this instance.
    /// </summary>
    private readonly Int32 _rawValue;

    /// <summary>
    /// Internal <see cref="Rune"/> instance.
    /// </summary>
    public Rune Value => this._value;
    /// <summary>
    /// The number of units read to create the resulting <see cref="Rune"/>.
    /// </summary>
    public Int32 CharsConsumed => this._charsConsumed;
    /// <summary>
    /// Indicates whether this instance was decoded as a single-byte unit.
    /// </summary>
    public Boolean IsSingleUnicode => this._value.IsAscii || this._rawValue < Byte.MaxValue;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value"><see cref="Rune"/> instance.</param>
    /// <param name="charsConsumed">The number of units read to create <paramref name="value"/>.</param>
    /// <param name="source">A read-only span of <see cref="Byte"/> that represents an unicode text.</param>
    private DecodedRune(Rune value, Int32 charsConsumed, ReadOnlySpan<Byte> source)
    {
        this._value = value;
        this._charsConsumed = charsConsumed;
        CopyRawValue(ref this._rawValue, source);
    }

    /// <inheritdoc/>
    public override Boolean Equals(Object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        else if (obj is DecodedRune decoded)
            return this.Equals(decoded);
        else if (obj is Rune rune)
            return this._value.Equals(rune);
        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override Int32 GetHashCode() => this._value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => this._value.ToString();

    /// <summary>
    /// Decodes the <see cref="Rune"/> at the beginning of the provided unicode source buffer and
    /// wraps it into an <see cref="DecodedRune"/> instance.
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Byte"/> that represents a text.</param>
    /// <returns>Decoded <see cref="Rune"/>.</returns>
    public static DecodedRune? Decode(ReadOnlySpan<Byte> source)
    {
        if (Rune.DecodeFromUtf8(source, out Rune result, out Int32 charsConsumed) == OperationStatus.Done)
            return new(result, charsConsumed, source[..charsConsumed]);
        return default;
    }

    /// <summary>
    /// Decodes the <see cref="Rune"/> at the beginning of the provided unicode source buffer and
    /// wraps it into an <see cref="DecodedRune"/> instance.
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Char"/> that represents a text.</param>
    /// <returns>Decoded <see cref="DecodedRune"/>.</returns>
    public static DecodedRune? Decode(ReadOnlySpan<Char> source)
    {
        if (Rune.DecodeFromUtf16(source, out Rune result, out Int32 charsConsumed) == OperationStatus.Done)
            return new(result, charsConsumed, MemoryMarshal.AsBytes(source[..charsConsumed]));
        return default;
    }

    /// <summary>
    /// Retrieves the indices from individuals runes decoded in <paramref name="source"/>.
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Char"/> that represents a text.</param>
    /// <returns>The indices from individuals runes decoded in <paramref name="source"/>.</returns>
    public static IReadOnlyList<Int32> GetIndices(ReadOnlySpan<Char> source)
    {
        List<Int32> result = new(source.Length);
        Int32 length = default;

        while (length < source.Length)
        {
            result.Add(length);
            if (DecodedRune.Decode(source[length..]) is not DecodedRune rune)
                break;
            length += rune.CharsConsumed;
        }

        return result.ToArray();
    }

    /// <summary>
    /// Retrieves the indices from individuals runes decoded in <paramref name="source"/>.
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Byte"/> that represents a text.</param>
    /// <returns>The indices from individuals runes decoded in <paramref name="source"/>.</returns>
    public static IReadOnlyList<Int32> GetIndices(ReadOnlySpan<Byte> source)
    {
        List<Int32> result = new(Encoding.UTF8.GetCharCount(source));
        Int32 length = default;

        while (length < source.Length)
        {
            result.Add(length);
            if (DecodedRune.Decode(source[length..]) is not DecodedRune rune)
                break;
            length += rune.CharsConsumed;
        }

        return result.ToArray();
    }

    /// <summary>
    /// Copies the raw value from source span.
    /// </summary>
    /// <param name="integerValue"></param>
    /// <param name="source"></param>
    private static void CopyRawValue(ref Int32 integerValue, ReadOnlySpan<Byte> source)
    {
        Span<Byte> bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref integerValue, 1));
        source.CopyTo(bytes);
    }

    /// <summary>
    /// Operator. <see cref="DecodedRune"/> -> <see cref="Rune"/>
    /// </summary>
    /// <param name="rune">A <see cref="DecodedRune"/> instance.</param>
    public static implicit operator Rune(DecodedRune rune) => rune.Value;

    /// <summary>
    /// Returns a value that indicates whether two Rune instances have equal values.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have the same value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator ==(DecodedRune left, DecodedRune right) => left._value == right._value;

    /// <summary>
    /// Returns a value that indicates whether two Rune instances have different values.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have the different values;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator !=(DecodedRune left, DecodedRune right) => left._value != right._value;
}

