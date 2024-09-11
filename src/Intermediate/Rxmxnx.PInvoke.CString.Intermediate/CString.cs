namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of UTF-8 encoded characters.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[DebuggerTypeProxy(typeof(CStringDebugView))]
public sealed partial class CString : ICloneable, IEquatable<CString>, IEquatable<String>
{
	/// <summary>
	/// Represents an empty UTF-8 string. This field is read-only.
	/// </summary>
	public static readonly CString Empty = new([default,], true);
	/// <summary>
	/// Represents a null-pointer UTF-8 string. This field is read-only.
	/// </summary>
	public static readonly CString Zero = new(IntPtr.Zero, 0, true);

	/// <summary>
	/// Gets a value indicating whether the text in the current <see cref="CString"/> instance
	/// ends with a null-termination character.
	/// </summary>
	public Boolean IsNullTerminated => this._isNullTerminated;
	/// <summary>
	/// Gets a value indicating whether the UTF-8 text is referenced by, and not contained within,
	/// the current <see cref="CString"/> instance.
	/// </summary>
	public Boolean IsReference => !this._isLocal && !this._isFunction;
	/// <summary>
	/// Gets a value indicating whether the current <see cref="CString"/> instance is a segment
	/// (or slice) of another <see cref="CString"/> instance.
	/// </summary>
	public Boolean IsSegmented => this._data.IsMemorySlice;
	/// <summary>
	/// Gets a value indicating whether the current <see cref="CString"/> instance is a function.
	/// </summary>
	public Boolean IsFunction => this._isFunction;

	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 character repeated a specified number of times.
	/// </summary>
	/// <param name="c">A UTF-8 char.</param>
	/// <param name="count">The number of times <paramref name="c"/> is repeated to form the UTF-8 string.</param>
	public CString(Byte c, Int32 count) :
		this(CString.CreateRepeatedSequence(stackalloc Byte[] { c, }, count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-8 unit.</param>
	/// <param name="u1">The second UTF-8 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Byte u0, Byte u1, Int32 count) : this(
		CString.CreateRepeatedSequence(stackalloc Byte[] { u0, u1, }, count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-8 unit.</param>
	/// <param name="u1">The second UTF-8 unit.</param>
	/// <param name="u2">The third UTF-8 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Byte u0, Byte u1, Byte u2, Int32 count) : this(
		CString.CreateRepeatedSequence(stackalloc Byte[] { u0, u1, u2, }, count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-8 unit.</param>
	/// <param name="u1">The second UTF-8 unit.</param>
	/// <param name="u2">The third UTF-8 unit.</param>
	/// <param name="u3">The fourth UTF-8 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Byte u0, Byte u1, Byte u2, Byte u3, Int32 count) : this(
		CString.CreateRepeatedSequence(stackalloc Byte[] { u0, u1, u2, u3, }, count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class using the UTF-8 characters
	/// indicated in the specified read-only span.
	/// </summary>
	/// <param name="source">A read-only span of UTF-8 characters to initialize the new instance.</param>
	public CString(ReadOnlySpan<Byte> source) : this(CString.CreateRepeatedSequence(source, 1)) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class that contains the UTF-8 string
	/// returned by the specified <see cref="ReadOnlySpanFunc{Byte}"/>.
	/// </summary>
	/// <param name="func">
	/// <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns the UTF-8 string to initialize
	/// the new instance.
	/// </param>
	public CString(ReadOnlySpanFunc<Byte> func) : this(func, true) { }
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Object Clone()
	{
		ReadOnlySpan<Byte> source = this;
		Byte[] bytes = new Byte[this._length + 1];
		source.CopyTo(bytes);
		return new CString(bytes, true);
	}

	/// <inheritdoc/>
	public Boolean Equals([NotNullWhen(true)] CString? other) => other is not null && CString.equals(this, other);
	/// <summary>
	/// Determines whether the current <see cref="CString"/> and a specified <see cref="String"/>
	/// have the same value.
	/// </summary>
	/// <param name="other">The <see cref="String"/> to compare to the current instance.</param>
	/// <returns>
	/// <see langword="true"/> if the value of the <paramref name="other"/> parameter is the same
	/// as this <see cref="CString"/>, otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Equals([NotNullWhen(true)] String? other)
		=> other is not null && StringUtf8Comparator.Create().TextEquals(this, other);
	/// <summary>
	/// Determines whether the current <see cref="CString"/> and a specified <see cref="CString"/>
	/// have the same value.
	/// A parameter specifies the culture, case, and sort rules used in the comparison.
	/// </summary>
	/// <param name="value">The <see cref="CString"/> to compare to the current instance.</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies how the strings will be compared.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the value of the <paramref name="value"/> parameter is the same
	/// as this <see cref="CString"/>, otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Equals([NotNullWhen(true)] CString? value, StringComparison comparisonType)
		=> value is not null && CStringUtf8Comparator.Create(comparisonType).TextEquals(this, value);
	/// <summary>
	/// Determines whether the current <see cref="CString"/> and a specified <see cref="String"/>
	/// have the same value.
	/// A parameter specifies the culture, case, and sort rules used in the comparison.
	/// </summary>
	/// <param name="value">The <see cref="String"/> to compare to the current instance.</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies how the strings will be compared.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the value of the <paramref name="value"/> parameter is the same as this
	/// <see cref="CString"/>, otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Equals([NotNullWhen(true)] String? value, StringComparison comparisonType)
		=> value is not null && StringUtf8Comparator.Create(comparisonType).TextEquals(this, value);

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? obj)
		=> obj is String str ? this.Equals(str) : obj is CString cstr && this.Equals(cstr);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override String ToString()
	{
		if (this._length == 0) return String.Empty;
		String result = Encoding.UTF8.GetString(this.AsSpan());
		return String.IsInterned(result) ?? result;
	}
	/// <inheritdoc/>
	public override Int32 GetHashCode() => this.ToString().GetHashCode();

	/// <summary>
	/// Returns a reference to the first UTF-8 unit of the <see cref="CString"/>.
	/// </summary>
	/// <returns>A reference to the first UTF-8 unit of the <see cref="CString"/>.</returns>
	/// <remarks>
	/// This method is used to support the use of a <see cref="CString"/> within a fixed statement.
	/// It should not be used in typical code.
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ref readonly Byte GetPinnableReference() => ref MemoryMarshal.GetReference(this._data.AsSpan());
	/// <summary>
	/// Copies the UTF-8 text of the current <see cref="CString"/> instance into a new byte array.
	/// </summary>
	/// <returns>
	/// A new <see cref="Byte"/> array containing the UTF-8 units of the current <see cref="CString"/>.
	/// </returns>
	public Byte[] ToArray() => this._data.ToArray()[..this._length];
	/// <summary>
	/// Retrieves the UTF-8 units of the current <see cref="CString"/> as a read-only span of bytes.
	/// </summary>
	/// <returns>
	/// A read-only span of bytes representing the UTF-8 units of the current <see cref="CString"/>.
	/// </returns>
	public ReadOnlySpan<Byte> AsSpan() => this._data.AsSpan()[..this._length];
	/// <summary>
	/// Returns a <see cref="String"/> that represents the current UTF-8 text as a hexadecimal value.
	/// </summary>
	/// <returns>
	/// A <see cref="String"/> that represents the hexadecimal value of the current UTF-8 text.
	/// </returns>
	public String ToHexString() => Convert.ToHexString(this).ToLower();
	/// <summary>
	/// Returns an enumerator that iterates through the UTF-8 units of the current <see cref="CString"/>.
	/// </summary>
	/// <returns>
	/// An enumerator that can be used to iterate through the UTF-8 units of the current
	/// <see cref="CString"/>.
	/// </returns>
	public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();

	/// <summary>
	/// Determines whether the specified <see cref="CString"/> is <see langword="null"/>
	/// or an empty UTF-8 string.
	/// </summary>
	/// <param name="value">The <see cref="CString"/> to test.</param>
	/// <returns>
	/// <see langword="true"/> if the value parameter is <see langword="null"/> or an empty UTF-8 string;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean IsNullOrEmpty([NotNullWhen(false)] CString? value) => value is null || value.Length == 0;
	/// <summary>
	/// Constructs a new instance of the <see cref="CString"/> class using the UTF-8 characters provided
	/// in the specified read-only span.
	/// </summary>
	/// <param name="source">A read-only span of UTF-8 characters.</param>
	/// <returns>A new instance of the <see cref="CString"/> class.</returns>
	public static CString Create(ReadOnlySpan<Byte> source) => new(source.ToArray(), false);
	/// <summary>
	/// Constructs a new instance of the <see cref="CString"/> class using the
	/// <see cref="ReadOnlySpanFunc{Byte}"/> delegate provided.
	/// </summary>
	/// <param name="func">
	/// A <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns a Utf8 string non-literal.
	/// </param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class, if the func is not <see langword="null"/>;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[return: NotNullIfNotNull(nameof(func))]
	public static CString? Create(ReadOnlySpanFunc<Byte>? func) => func is not null ? new(func, false) : default;
	/// <summary>
	/// Constructs a new instance of the <see cref="CString"/> class using the binary internal
	/// information provided.
	/// </summary>
	/// <param name="bytes">Binary internal information.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class, if the bytes are not <see langword="null"/>;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[return: NotNullIfNotNull(nameof(bytes))]
	public static CString? Create(Byte[]? bytes) => bytes is not null ? new(bytes, false) : default;
	/// <summary>
	/// Constructs a new instance of the <see cref="CString"/> class using <paramref name="state"/>.
	/// </summary>
	/// <param name="state">Function state parameter.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class.
	/// </returns>
#pragma warning disable CA2252
	public static CString Create<TState>(TState state) where TState : struct, IUtf8FunctionState<TState>
	{
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(state, TState.GetSpan);
		Int32 length = TState.GetLength(state);
		return new(data, true, state.IsNullTerminated, length);
	}
#pragma warning restore CA2252
	/// <summary>
	/// Constructs a new instance of the <see cref="CString"/> class using the pointer to a UTF-8
	/// character array and length provided.
	/// </summary>
	/// <param name="ptr">A pointer to an array of UTF-8 characters.</param>
	/// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
	/// <param name="useFullLength">Indicates whether the total length should be used.</param>
	/// <returns>A new instance of the <see cref="CString"/> class.</returns>
	public static CString CreateUnsafe(IntPtr ptr, Int32 length, Boolean useFullLength = false)
		=> new(ptr, length, useFullLength);
}