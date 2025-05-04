namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of null-terminated UTF-8 text strings.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
public sealed partial class CStringSequence : ICloneable, IEquatable<CStringSequence>
{
	/// <summary>
	/// Represents an empty sequence.
	/// </summary>
	public static readonly CStringSequence Empty = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// from a collection of strings.
	/// </summary>
	/// <param name="values">The collection of strings.</param>
	public CStringSequence(
#if !NET9_0_OR_GREATER
		params
#endif
			String?[] values) : this(values.AsSpan()) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from a
	/// collection of UTF-8 strings.
	/// </summary>
	/// <param name="values">The collection of <see cref="CString"/> instances.</param>
	public CStringSequence(
#if !NET9_0_OR_GREATER
		params
#endif
			CString?[] values) : this(values.AsSpan()) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from a
	/// read-only span of UTF-8 strings.
	/// </summary>
	/// <param name="values">The collection of <see cref="CString"/> instances.</param>
	public CStringSequence(
#if NET9_0_OR_GREATER
		params
#endif
		ReadOnlySpan<CString?> values)
	{
		this._lengths = CStringSequence.GetLengthArray(values);
		this._value = CStringSequence.CreateBuffer(values);
		this._cache = CStringSequence.CreateCache(this._lengths, out this._nonEmptyCount);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from a
	/// read-only span of strings.
	/// </summary>
	/// <param name="values">The collection of strings.</param>
	public CStringSequence(
#if NET9_0_OR_GREATER
		params
#endif
		ReadOnlySpan<String?> values)
	{
		CString?[] list = new CString?[values.Length];
		this._lengths = new Int32?[values.Length];
		for (Int32 i = 0; i < values.Length; i++)
		{
			CString? cstr = CStringSequence.GetCString(values[i]);
			list[i] = cstr;
			this._lengths[i] = cstr?.Length;
		}
		this._cache = CStringSequence.CreateCache(this._lengths, out this._nonEmptyCount);
		this._value = CStringSequence.CreateBuffer(list);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from an
	/// enumeration of strings.
	/// </summary>
	/// <param name="values">The enumerable collection of strings.</param>
	public CStringSequence(IEnumerable<String?> values) : this(values.Select(CStringSequence.GetCString).ToArray()) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from an
	/// enumeration of UTF-8 strings.
	/// </summary>
	/// <param name="values">The enumerable collection of <see cref="CString"/> instances.</param>
	public CStringSequence(IEnumerable<CString?> values) : this(values.ToArray()) { }
	/// <summary>
	/// Creates a copy of this instance of <see cref="CStringSequence"/>.
	/// </summary>
	/// <returns>A new object that is a copy of this instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Object Clone() => new CStringSequence(this);
	/// <summary>
	/// Determines whether the current <see cref="CStringSequence"/> is equal to another
	/// <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="other">The <see cref="CStringSequence"/> to compare with this object.</param>
	/// <returns>
	/// <see langword="true"/> if the current <see cref="CStringSequence"/> is equal to the
	/// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean Equals(CStringSequence? other)
		=> other is not null && this._value.Equals(other._value) && this._lengths.SequenceEqual(other._lengths);

	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => obj is CStringSequence cstr && this.Equals(cstr);
	/// <inheritdoc/>
	public override String ToString() => this._value;
	/// <inheritdoc/>
	public override Int32 GetHashCode() => this._value.GetHashCode();

	/// <summary>
	/// Returns a reference to the first UTF-8 byte of the <see cref="CStringSequence"/>.
	/// </summary>
	/// <remarks>
	/// Required to support the use of a <see cref="CStringSequence"/> within a fixed statement.
	/// It should not be used in typical code.
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ref readonly Byte GetPinnableReference()
	{
		ReadOnlySpan<Char> chars = this._value.AsSpan();
		ReadOnlySpan<Byte> bytes = MemoryMarshal.AsBytes(chars);
		return ref MemoryMarshal.GetReference(bytes);
	}
	/// <summary>
	/// Returns a <see cref="CString"/> that represents the current sequence.
	/// </summary>
	/// <returns>A <see cref="CString"/> that represents the current sequence.</returns>
	public CString ToCString()
	{
		this.CalculateSubRange(0, this._lengths.Length, out _, out Int32 bytesLength);
		Byte[] result = new Byte[bytesLength + 1];
		this.WithSafeTransform(result, CStringSequence.BinaryCopyTo);
		return result;
	}

	/// <summary>
	/// Creates a new UTF-8 text sequence with specific lengths, and initializes each
	/// UTF-8 text string in it after creation using the specified callback.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the <paramref name="action"/>.</typeparam>
	/// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
	/// <param name="state">The element to pass to the <paramref name="action"/>.</param>
	/// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
	/// <returns>The created UTF-8 text sequence.</returns>
	public static CStringSequence Create<TState>(TState state, CStringSequenceCreationAction<TState> action,
		params Int32?[] lengths)
#if NET9_0_OR_GREATER
	where TState : allows ref struct
#endif
	{
		Int32 length = CStringSequence.GetBufferLength(lengths);
		SequenceCreationHelper<TState> helper = new() { State = state, Action = action, Lengths = lengths, };
		String buffer = String.Create(length, helper, CStringSequence.CreateCStringSequence);
		return new(buffer, lengths);
	}
	/// <summary>
	/// Creates a new <see cref="CStringSequence"/> instance from a UTF-8 buffer.
	/// </summary>
	/// <param name="value">A buffer of a UTF-8 sequence.</param>
	/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
	/// <list type="bullet">
	///     <item>Any UTF-8 null characters at the beginning or end of the buffer will be ignored.</item>
	///     <item>Any non-consecutive UTF-8 null character will be considered an element separator.</item>
	///     <item>Any consecutive UTF-8 null characters will be considered part of the next element.</item>
	/// </list>
	public static CStringSequence Create(ReadOnlySpan<Char> value)
	{
		Boolean isParsable = false;
		ReadOnlySpan<Byte> bufferSpan = CStringSequence.GetSourceBuffer(value, ref isParsable);
		return CStringSequence.CreateFrom(bufferSpan);
	}
	/// <summary>
	/// Converts the buffer of a UTF-8 sequence to a <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="value">A buffer of a UTF-8 sequence.</param>
	/// <returns>A <see cref="CStringSequence"/> instance.</returns>
	/// <remarks>
	///     <list type="bullet">
	///         <item>Any UTF-8 null characters at the beginning or end of the buffer will be ignored.</item>
	///         <item>Any non-consecutive UTF-8 null character will be considered an element separator.</item>
	///         <item>Any consecutive UTF-8 null characters will be considered part of the next element.</item>
	///         <item>
	///         <paramref name="value"/> will be used as a buffer if and only if it does not start with UTF-8 null
	///         characters and if it ends with at least one UTF-8 null character.
	///         </item>
	///     </list>
	/// </remarks>
	[return: NotNullIfNotNull(nameof(value))]
	public static CStringSequence? Parse(String? value)
	{
		if (value is null) return default;
		Boolean isParsable = true;
		ReadOnlySpan<Byte> bufferSpan = CStringSequence.GetSourceBuffer(value, ref isParsable);
		return !isParsable ? CStringSequence.CreateFrom(bufferSpan) : CStringSequence.CreateFrom(value);
	}
}