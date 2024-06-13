namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of null-terminated UTF-8 text strings.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
public sealed partial class CStringSequence : ICloneable, IEquatable<CStringSequence>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// from a collection of strings.
	/// </summary>
	/// <param name="values">The collection of strings.</param>
	public CStringSequence(params String?[] values)
	{
		List<CString?> list = new(values.Length);
		this._lengths = new Int32?[values.Length];
		for (Int32 i = 0; i < values.Length; i++)
		{
			CString? cstr = CStringSequence.GetCString(values[i]);
			list.Add(cstr);
			this._lengths[i] = cstr?.Length;
		}
		this._cache = CStringSequence.CreateCache(this._lengths);
		this._value = CStringSequence.CreateBuffer(list);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from a
	/// collection of <see cref="CString"/> instances.
	/// </summary>
	/// <param name="values">The collection of <see cref="CString"/> instances.</param>
	public CStringSequence(params CString?[] values)
	{
		this._lengths = CStringSequence.GetLengthArray(values);
		this._value = CStringSequence.CreateBuffer(values);
		this._cache = CStringSequence.CreateCache(this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from an
	/// enumerable collection of strings.
	/// </summary>
	/// <param name="values">The enumerable collection of strings.</param>
	public CStringSequence(IEnumerable<String?> values)
	{
		List<CString?> list = values.Select(CStringSequence.GetCString).ToList();
		this._lengths = CStringSequence.GetLengthArray(list);
		this._value = CStringSequence.CreateBuffer(list);
		this._cache = CStringSequence.CreateCache(this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from an
	/// enumerable collection of <see cref="CString"/> instances.
	/// </summary>
	/// <param name="values">The enumerable collection of <see cref="CString"/> instances.</param>
	public CStringSequence(IEnumerable<CString?> values) : this(CStringSequence.FromArray(values, out CString?[] arr))
	{
		this._lengths = CStringSequence.GetLengthArray(arr);
		this._value = CStringSequence.CreateBuffer(arr);
		this._cache = CStringSequence.CreateCache(this._lengths);
	}
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
		=> ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(this._value.AsSpan()));
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
	{
		Int32 length = CStringSequence.GetBufferLength(lengths);
		SequenceCreationState<TState> cState = new() { State = state, Action = action, Lengths = lengths, };
		String buffer = String.Create(length, cState, CStringSequence.CreateCStringSequence);
		return new(buffer, lengths);
	}
	/// <summary>
	/// Converts the buffer of a UTF-8 sequence to a <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="value">A buffer of a UTF-8 sequence.</param>
	/// <returns>A <see cref="CStringSequence"/> instance.</returns>
	public static CStringSequence Create(ReadOnlySpan<Char> value)
	{
		ReadOnlySpan<Byte> bufferSpan = CStringSequence.GetSourceBuffer(value, out _);
		return CStringSequence.CreateFrom(bufferSpan);
	}
	/// <summary>
	/// Converts the buffer of a UTF-8 sequence to a <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="value">A buffer of a UTF-8 sequence.</param>
	/// <returns>A <see cref="CStringSequence"/> instance.</returns>
	[return: NotNullIfNotNull(nameof(value))]
	public static CStringSequence? Parse(String? value)
	{
		if (value is null) return default;
		ReadOnlySpan<Byte> bufferSpan = CStringSequence.GetSourceBuffer(value, out Boolean newBuffer);
		return newBuffer ? CStringSequence.CreateFrom(bufferSpan) : CStringSequence.CreateFrom(value);
	}
	/// <summary>
	/// Converts the buffer of a UTF-8 sequence to a <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="value">A buffer of a UTF-8 sequence.</param>
	/// <param name="newBuffer">Output. Indicates whether resulting instance creates a new buffer..</param>
	/// <returns>A <see cref="CStringSequence"/> instance.</returns>
	[return: NotNullIfNotNull(nameof(value))]
	public static CStringSequence? Parse(String? value, out Boolean newBuffer)
	{
		if (value is null)
		{
			newBuffer = false;
			return default;
		}
		ReadOnlySpan<Byte> bufferSpan = CStringSequence.GetSourceBuffer(value, out newBuffer);
		return newBuffer ? CStringSequence.CreateFrom(bufferSpan) : CStringSequence.CreateFrom(value);
	}

	/// <summary>
	/// Retrieves a String representation from <paramref name="sequence"/>.
	/// </summary>
	/// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
	/// <returns>String representation from <paramref name="sequence"/>.</returns>
	[return: NotNullIfNotNull(nameof(sequence))]
	public static String? GetPrintable(CStringSequence? sequence)
		=> CStringSequence.GetPrintableBuffer(sequence?._value);
	/// <summary>
	/// Retrieves a String representation from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>String representation from <paramref name="buffer"/>.</returns>
	[return: NotNullIfNotNull(nameof(buffer))]
	public static String? GetPrintableBuffer(String? buffer)
	{
		if (String.IsNullOrEmpty(buffer)) return buffer;
		return buffer[^1] != default ? buffer : @$"{buffer.AsSpan()[..^1]}\0";
	}
	/// <summary>
	/// Retrieves a String representation from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>String representation from <paramref name="buffer"/>.</returns>
	[return: NotNullIfNotNull(nameof(buffer))]
	public static String? GetPrintableBuffer(ReadOnlySpan<Char> buffer)
	{
		if (buffer.Length == 0) return String.Empty;
		return buffer[^1] != default ? $"{buffer}" : @$"{buffer[..^1]}\0";
	}
}