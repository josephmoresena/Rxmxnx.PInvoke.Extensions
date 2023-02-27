namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents text as a sequence of UTF-8 code units.
/// </summary>
public sealed partial class CString : ICloneable, IEquatable<CString>
{
    /// <summary>
    /// Represents the empty UTF-8 string. This field is read-only.
    /// </summary>
    public static readonly CString Empty = new(empty!);

    /// <summary>
    /// Gets the number of bytes in the current <see cref="CString"/> object.
    /// </summary>
    /// <returns>
    /// The number of characters in the current string.
    /// </returns>
    public Int32 Length => this._length;
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
    public Byte[] ToArray() => this._data.ToArray();

    /// <summary>
    /// Retreives the internal or external information as <see cref="ReadOnlySpan{Byte}"/> object.
    /// </summary>
    /// <returns><see cref="ReadOnlySpan{Byte}"/> instance.</returns>
    public ReadOnlySpan<Byte> AsSpan() => this._data.AsSpan()[..this._length];

    /// <summary>
    /// Gets <see cref="String"/> representation of the current UTF-8 text as hexadecimal value.
    /// </summary>
    /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
    public String AsHexString() => Convert.ToHexString(this).ToLower();

    /// <summary>
    /// Retrieves an object that can iterate through the individual <see cref="Byte"/> in this 
    /// <see cref="CString"/>.
    /// </summary>
    /// <returns>An enumerator object.</returns>
    public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this._data.GetEnumerator();

    /// <summary>
    /// Returns a reference to this instance of <see cref="CString"/>.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public Object Clone()
    {
        Byte[] bytes;
        if (this._isNullTerminated && !this._isFunction)
            bytes = this._data.ToArray();
        else
        {
            ReadOnlySpan<Byte> data = this._data;
            bytes = new Byte[this._length + 1];
            data.CopyTo(bytes);
        }
        return new CString(bytes);
    }

    /// <summary>
    /// Indicates whether the current <see cref="CString"/> is equal to another <see cref="CString"/> 
    /// instance.
    /// </summary>
    /// <param name="other">A <see cref="CString"/> to compare with this object.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="CString"/> is equal to the <paramref name="other"/> 
    /// parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public Boolean Equals(CString? other)
        => other is CString otherNotNull && this._length == otherNotNull._length && equals(this, other);

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true"/> if the specified object is equal to the current object; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public override Boolean Equals(Object? obj) => obj is CString cstr && Equals(cstr);

    /// <summary>
    /// Returns a <see cref="String"/> that represents the current instance.
    /// </summary>
    /// <returns>A string that represents the current UTF-8 text.</returns>
    public override String ToString()
    {
        if (this._length == 0)
            return String.Empty;
        else
            return Encoding.UTF8.GetString(this.AsSpan());
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override Int32 GetHashCode() => this.GetHashCodeLengthNullMatch() ?? new CStringSequence(this).GetHashCode();

    /// <summary>
    /// Writes the sequence of bytes to the given <see cref="Stream"/> and advances
    /// the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <param name="writeNullTermination">
    /// Indicates whether the UTF-8 text must be written with a null-termination character 
    /// into the <see cref="Stream"/>.
    /// </param>
    public void Write([DisallowNull] Stream strm, Boolean writeNullTermination)
    {
        strm.Write(this.AsSpan());
        if (writeNullTermination)
            strm.Write(empty);
    }

    /// <summary>
    /// Asynchronously writes the sequence of bytes to the given <see cref="Stream"/> and advances
    /// the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <param name="writeNullTermination">
    /// Indicates whether the UTF-8 text must be written with a null-termination character 
    /// into the <see cref="Stream"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync([DisallowNull] Stream strm, Boolean writeNullTermination)
    {
        await this.GetWriteTask(strm).ConfigureAwait(false);
        if (writeNullTermination)
            await strm.WriteAsync(empty);
    }

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
    /// Retrieves the internal binary data from a given <see cref="CString"/>.
    /// </summary>
    /// <param name="value">A non-reference <see cref="CString"/> instance.</param>
    /// <returns>A <see cref="Byte"/> array with UTF-8 text.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="value"/> does not contains the UTF-8 text.</exception>
    public static Byte[] GetBytes([DisallowNull] CString value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if ((Byte[]?)value._data is Byte[] array)
            return array;
        else
            throw new InvalidOperationException(nameof(value) + " does not contains the UTF-8 text.");
    }

    /// <summary>
    /// Creates a new <see cref="CString"/> instance from <paramref name="func"/>.
    /// </summary>
    /// <param name="func">A <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns a Utf8 string non-literal.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="func"/>.</returns>
    public static CString? Create(ReadOnlySpanFunc<Byte>? func) => func is not null ? new(func, false) : default;
}