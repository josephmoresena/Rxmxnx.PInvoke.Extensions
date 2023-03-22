namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents text as a sequence of UTF-8 code units.
/// </summary>
public sealed partial class CString : ICloneable, IEquatable<CString>, IEquatable<String>
{
    /// <summary>
    /// Represents the empty UTF-8 string. This field is read-only.
    /// </summary>
    public static readonly CString Empty = new(new Byte[] { default });

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
    /// <param name="c"></param>
    /// <param name="count"></param>
    public CString(Byte c, Int32 count) : this(Enumerable.Repeat(c, count).Concat(empty).ToArray()) { }

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
    public CString(IntPtr ptr, Int32 length)
    {
        this._isLocal = false;
        this._data = ValueRegion<Byte>.Create(ptr, length);
        ReadOnlySpan<Byte> data = this._data;
        if (data.IsEmpty)
        {
            this._isNullTerminated = false;
            this._length = 0;
        }
        else
        {
            this._isNullTerminated = data[^1] == default;
            this._length = length - (this._isNullTerminated ? 1 : 0);
        }
    }

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
    public Object Clone()
    {
        ReadOnlySpan<Byte> source = this;
        Byte[] bytes = new Byte[this._length + 1];
        source.CopyTo(bytes);
        return new CString(bytes);
    }

    /// <inheritdoc/>
    public Boolean Equals(CString? other)
        => other is CString otherNotNull && this._length == otherNotNull._length && equals(this, otherNotNull);

    /// <inheritdoc/>
    public Boolean Equals(String? other)
        => other is String otherNotNull && this.ToString() == otherNotNull;

    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) =>
        obj is String str ? this.Equals(str) : obj is CString cstr && Equals(cstr);

    /// <inheritdoc/>
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
        => value is not CString valueNotNull || valueNotNull._length == 0;

    /// <summary>
    /// Creates a new <see cref="CString"/> instance from <paramref name="func"/>.
    /// </summary>
    /// <param name="func">A <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns a Utf8 string non-literal.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="func"/>.</returns>
    [return: NotNullIfNotNull(nameof(func))]
    public static CString? Create(ReadOnlySpanFunc<Byte>? func) => func is not null ? new(func, false) : default;
}