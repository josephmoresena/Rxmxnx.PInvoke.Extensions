namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents text as a sequence of UTF-8 code units.
/// </summary>
public sealed partial class CString : ICloneable, IEquatable<CString>, IEquatable<String>
{
    /// <summary>
    /// Represents the empty UTF-8 string. This field is read-only.
    /// </summary>
    public static readonly CString Empty = new(new Byte[] { default }, true);
    /// <summary>
    /// Represents the zero UTF-8 string. This field is read-only.
    /// </summary>
    public static readonly CString Zero = new(IntPtr.Zero, 0, true);

    /// <summary>
    /// Indicates whether the ending of text in the current <see cref="CString"/> includes 
    /// a null-termination character.
    /// </summary>
    public Boolean IsNullTerminated => this._isNullTerminated;
    /// <summary>
    /// Indicates whether the UTF-8 text is referenced by the current <see cref="CString"/> and 
    /// not contained by.
    /// </summary>
    public Boolean IsReference => !this._isLocal && !this._isFunction;
    /// <summary>
    /// Indicates whether the current instance is segmented.
    /// </summary>
    public Boolean IsSegmented => this._data.IsSegmented;
    /// <summary>
    /// Indicates whether the current instance is a function.
    /// </summary>
    public Boolean IsFunction => this._isFunction;

    /// <summary>
    /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
    /// UTF-8 character repeated a specified number of times.
    /// </summary>
    /// <param name="c">A UTF-8 char.</param>
    /// <param name="count">The number of the times <paramref name="c"/> occours.</param>
    public CString(Byte c, Int32 count) : this(CreateRepeatedSequence(stackalloc Byte[] { c }, count), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
    /// UTF-8 sequence repeated a specified number of times.
    /// </summary>
    /// <param name="u0">The first UTF-8 unit.</param>
    /// <param name="u1">The second UTF-8 unit.</param>
    /// <param name="count">The number of the times the sequence occours.</param>
    public CString(Byte u0, Byte u1, Int32 count)
        : this(CreateRepeatedSequence(stackalloc Byte[] { u0, u1 }, count), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
    /// UTF-8 sequence repeated a specified number of times.
    /// </summary>
    /// <param name="u0">The first UTF-8 unit.</param>
    /// <param name="u1">The second UTF-8 unit.</param>
    /// <param name="u2">The third UTF-8 unit.</param>
    /// <param name="count">The number of the times the sequence occours.</param>
    public CString(Byte u0, Byte u1, Byte u2, Int32 count)
        : this(CreateRepeatedSequence(stackalloc Byte[] { u0, u1, u2 }, count), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
    /// UTF-8 sequence repeated a specified number of times.
    /// </summary>
    /// <param name="u0">The first UTF-8 unit.</param>
    /// <param name="u1">The second UTF-8 unit.</param>
    /// <param name="u2">The third UTF-8 unit.</param>
    /// <param name="u3">The fourth UTF-8 unit.</param>
    /// <param name="count">The number of the times the sequence occours.</param>
    public CString(Byte u0, Byte u1, Byte u2, Byte u3, Int32 count)
        : this(CreateRepeatedSequence(stackalloc Byte[] { u0, u1, u2, u3 }, count), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CString"/> class to the UTF-8 characters indicated in the specified
    /// read-only span.
    /// </summary>
    /// <param name="source">A read-only span of UTF-8 characters.</param>
    public CString(ReadOnlySpan<Byte> source) : this(CreateRepeatedSequence(source, 1))
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate.</param>
    public CString(ReadOnlySpanFunc<Byte> func) : this(func, true) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
    /// pointer to an array of UTF-8 characters and a length.
    /// </summary>
    /// <param name="ptr">A pointer to a array of UTF-8 characters.</param>
    /// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
    public CString(IntPtr ptr, Int32 length) : this(ptr, length, false) { }

    /// <summary>
    /// Copies the UTF-8 text into a new array.
    /// </summary>
    /// <returns>An array containing the data in the current UTF-8 text.</returns>
    public Byte[] ToArray() => this._data.ToArray()[..this._length];

    /// <summary>
    /// Retreives the internal or external information as <see cref="ReadOnlySpan{Byte}"/> object.
    /// </summary>
    /// <returns><see cref="ReadOnlySpan{Byte}"/> instance.</returns>
    public ReadOnlySpan<Byte> AsSpan() => this._data.AsSpan()[..this._length];

    /// <summary>
    /// Gets <see cref="String"/> representation of the current UTF-8 text as hexadecimal value.
    /// </summary>
    /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
    public String ToHexString() => Convert.ToHexString(this).ToLower();

    /// <summary>
    /// Retrieves an object that can iterate through the individual <see cref="Byte"/> in this 
    /// <see cref="CString"/>.
    /// </summary>
    /// <returns>An enumerator object.</returns>
    public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();

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
    public Boolean Equals([NotNullWhen(true)] CString? other)
        => other is CString otherNotNull && this._length == otherNotNull._length && equals(this, otherNotNull);

    /// <summary>
    /// Determines whether this <see cref="CString"/> and a specified <see cref="CString"/> object have the same value.
    /// A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="value">The string to compare this instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the values will be compared.</param>
    /// <returns>
    /// <see langword="true"/> if the value of the <paramref name="value"/> parameter is the same as this <see cref="CString"/>;
    /// otherwise, false.
    /// </returns>
    public Boolean Equals([NotNullWhen(true)] CString? value, StringComparison comparisonType)
        => value is CString valueNotNull && CStringUtf8Comparator.Create(comparisonType).TextEquals(this, valueNotNull);

    /// <inheritdoc/>
    public Boolean Equals([NotNullWhen(true)] String? other)
        => other is String otherNotNull && StringUtf8Comparator.Create().TextEquals(this, otherNotNull);

    /// <summary>
    /// Determines whether this <see cref="CString"/> and a specified <see cref="String"/> object have the same value.
    /// A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="value">The string to compare this instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the values will be compared.</param>
    /// <returns>
    /// <see langword="true"/> if the value of the <paramref name="value"/> parameter is the same as this <see cref="CString"/>;
    /// otherwise, false.
    /// </returns>
    public Boolean Equals([NotNullWhen(true)] String? value, StringComparison comparisonType)
        => value is String valueNotNull && StringUtf8Comparator.Create(comparisonType).TextEquals(this, valueNotNull);

    /// <inheritdoc/>
    public override Boolean Equals([NotNullWhen(true)] Object? obj)
        => obj is String str ? this.Equals(str) : obj is CString cstr && this.Equals(cstr);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override String ToString()
    {
        if (this._length == 0)
            return String.Empty;
        else
        {
            String result = Encoding.UTF8.GetString(this.AsSpan());
            return String.IsInterned(result) ?? result;
        }
    }

    /// <inheritdoc/>
    public override Int32 GetHashCode() => this.ToString().GetHashCode();

    /// <summary>
    /// Indicates whether the specified <see cref="CString"/> is <see langword="null"/> or an 
    /// empty UTF-8 text.
    /// </summary>
    /// <param name="value">The <see cref="CString"/> to test.</param>
    /// <returns>
    /// <see langword="true"/> if the value parameter is <see langword="null"/> or an empty UTF-8 text; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsNullOrEmpty([NotNullWhen(false)] CString? value)
        => value is null || value.Length == 0;

    /// <summary>
    /// Creates a new <see cref="CString"/> instance from <paramref name="ptr"/> and <paramref name="length"/>.
    /// </summary>
    /// <param name="ptr">A pointer to a array of UTF-8 characters.</param>
    /// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="ptr"/> and <paramref name="length"/>.</returns>
    public static CString Create(IntPtr ptr, Int32 length) => new(ptr, length, true);

    /// <summary>
    /// Creates a new <see cref="CString"/> instance from <paramref name="source"/>.
    /// </summary>
    /// <param name="source">A read-only span of UTF-8 characters.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="source"/>.</returns>
    public static CString Create(ReadOnlySpan<Byte> source) => new(source.ToArray(), false);

    /// <summary>
    /// Creates a new <see cref="CString"/> instance from <paramref name="func"/>.
    /// </summary>
    /// <param name="func">A <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns a Utf8 string non-literal.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="func"/>.</returns>
    [return: NotNullIfNotNull(nameof(func))]
    public static CString? Create(ReadOnlySpanFunc<Byte>? func) => func is not null ? new(func, false) : default;

    /// <summary>
    /// Creates a new <see cref="CString"/> instance from <paramref name="bytes"/>.
    /// </summary>
    /// <param name="bytes">Binary internal information.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="bytes"/>.</returns>
    [return: NotNullIfNotNull(nameof(bytes))]
    public static CString? Create(Byte[]? bytes) => bytes is not null ? new(bytes, false) : default;
}