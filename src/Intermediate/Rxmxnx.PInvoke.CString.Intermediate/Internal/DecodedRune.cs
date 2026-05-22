namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a decoded <see cref="Rune"/> instance.
/// </summary>
/// <remarks>
/// Contains additional information about the decoded rune, including the number of units consumed
/// during decoding and the raw value read.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
internal readonly struct DecodedRune : IEquatable<DecodedRune>, IEquatable<Rune>, IEquatable<UInt32>
{
#if !PACKAGE
	/// <summary>
	/// The raw integer value that was read from the input to form the Rune.
	/// </summary>
	/// <remarks>This is required only for the tests.</remarks>
	private readonly Int32 _rawValue;
#endif
	/// <summary>
	/// The <see cref="Rune"/> instance decoded from the input.
	/// </summary>
	private readonly Rune _value;

	/// <summary>
	/// The number of code units that were consumed from the input to decode the Rune.
	/// </summary>
	public Int32 CharsConsumed { get; }
	/// <summary>
	/// The <see cref="Rune"/> instance decoded from the input.
	/// </summary>
	public Int32 Value => this._value.Value;
#if !PACKAGE
	/// <summary>
	/// The raw integer value that was read from the input to form the Rune.
	/// </summary>
	/// <remarks>This is required only for the tests.</remarks>
	public Int32 RawValue => this._rawValue;
#endif

#if !PACKAGE
	/// <summary>
	/// Initializes a new instance of the <see cref="DecodedRune"/> class.
	/// </summary>
	/// <param name="value">The <see cref="Rune"/> instance decoded from the source.</param>
	/// <param name="charsConsumed">The number of code units consumed from the source to decode the Rune.</param>
	/// <param name="source">The original source span from which the Rune was decoded.</param>
	/// <remarks>
	/// This constructor is private because the class provides a factory method <see cref="Decode(ReadOnlySpan{Byte})"/>
	/// for creating instances, which handles the Rune decoding internally.
	/// </remarks>
	private DecodedRune(Rune value, Int32 charsConsumed, ReadOnlySpan<Byte> source)
#else
	/// <summary>
	/// Initializes a new instance of the <see cref="DecodedRune"/> class.
	/// </summary>
	/// <param name="value">The <see cref="Rune"/> instance decoded from the source.</param>
	/// <param name="charsConsumed">The number of code units consumed from the source to decode the Rune.</param>
	private DecodedRune(Rune value, Int32 charsConsumed)
#endif
	{
		this._value = value;
		this.CharsConsumed = charsConsumed;
#if !PACKAGE
		DecodedRune.CopyRawValue(ref this._rawValue, source);
#endif
	}

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public Boolean Equals(DecodedRune other) => this.CharsConsumed == other.CharsConsumed && this.Equals(other._value);
	/// <inheritdoc/>
	public Boolean Equals(Rune other) => this._value == other;
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public Boolean Equals(UInt32 other) => this.Equals(new Rune(other));
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj)
	{
		return obj switch
		{
			DecodedRune decoded => this.Equals(decoded._value),
			Rune rune => this.Equals(rune),
			UInt32 uInt32 => this.Equals(uInt32),
			_ => false,
		};
	}
	/// <inheritdoc/>
	public override Int32 GetHashCode() => this._value.GetHashCode();
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override String ToString() => this._value.ToString();

	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-8 source buffer and
	/// wraps it into an <see cref="DecodedRune"/> instance. If the decoding fails, it returns null.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="Byte"/> that represents a text.</param>
	/// <returns>The decoded <see cref="Rune"/> or null if the decoding fails.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DecodedRune? Decode(ReadOnlySpan<Byte> source)
	{
		if (Rune.DecodeFromUtf8(source, out Rune result, out Int32 charsConsumed) != OperationStatus.Done)
			return default;
#if !PACKAGE
		DecodedRune decoded = new(result, charsConsumed, source[..charsConsumed]);
#else
		DecodedRune decoded = new(result, charsConsumed);
#endif
		return decoded;
	}
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided Unicode source buffer and
	/// wraps it into an <see cref="DecodedRune"/> instance. If the decoding fails, it returns null.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="Char"/> that represents a text.</param>
	/// <returns>The decoded <see cref="Rune"/> or null if the decoding fails.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DecodedRune? Decode(ReadOnlySpan<Char> source)
	{
		if (Rune.DecodeFromUtf16(source, out Rune result, out Int32 charsConsumed) != OperationStatus.Done)
			return default;
#if !PACKAGE
		DecodedRune decoded = new(result, charsConsumed, MemoryMarshal.AsBytes(source[..charsConsumed]));
#else
		DecodedRune decoded = new(result, charsConsumed);
#endif
		return decoded;
	}
#if !PACKAGE
	/// <summary>
	/// Retrieves the starting positions of individual runes decoded from the <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="Char"/> that represents a Unicode text.</param>
	/// <returns>A list of starting positions for each decoded rune in <paramref name="source"/>.</returns>
	public static IReadOnlyList<Int32> GetIndices(ReadOnlySpan<Char> source)
	{
		List<Int32> result = new(source.Length);
		Int32 length = default;

		while (length < source.Length)
		{
			DecodedRune? rune = DecodedRune.Decode(source[length..]);
			if (!rune.HasValue) break;
			result.Add(length);
			length += rune.Value.CharsConsumed;
		}

		return result.ToArray();
	}
	/// <summary>
	/// Retrieves the starting positions of individual runes decoded from the <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="Byte"/> that represents a UTF-8 text.</param>
	/// <returns>A list of starting positions for each decoded rune in <paramref name="source"/>.</returns>
	public static IReadOnlyList<Int32> GetIndices(ReadOnlySpan<Byte> source)
	{
		List<Int32> result = new(Encoding.UTF8.GetCharCount(source));
		Int32 length = default;

		while (length < source.Length)
		{
			DecodedRune? rune = DecodedRune.Decode(source[length..]);
			if (!rune.HasValue) break;
			result.Add(length);
			length += rune.Value.CharsConsumed;
		}

		return result.ToArray();
	}
	/// <summary>
	/// Copies the raw integer value from the source span into <paramref name="integerValue"/>.
	/// </summary>
	/// <param name="integerValue">Reference to an integer where the raw value will be copied.</param>
	/// <param name="source">Source span from which the raw value will be copied.</param>
	private static void CopyRawValue(ref Int32 integerValue, ReadOnlySpan<Byte> source)
	{
		Span<Int32> integers = MemoryMarshal.CreateSpan(ref integerValue, 1);
		Span<Byte> bytes = MemoryMarshal.AsBytes(integers);
		source.CopyTo(bytes);
	}
#endif

	/// <summary>
	/// Checks if two <see cref="DecodedRune"/> instances have the same value.
	/// </summary>
	/// <param name="left">The first instance to compare.</param>
	/// <param name="right">The second instance to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(DecodedRune left, DecodedRune right) => left._value == right._value;
	/// <summary>
	/// Checks if two <see cref="DecodedRune"/> instances have different values.
	/// </summary>
	/// <param name="left">The first instance to compare.</param>
	/// <param name="right">The second instance to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator !=(DecodedRune left, DecodedRune right) => !(left == right);
}