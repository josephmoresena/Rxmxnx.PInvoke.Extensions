namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class that references a
	/// specified UTF-8 text.
	/// </summary>
	/// <param name="ptr">
	/// A pointer to an array of UTF-8 characters to reference in the new instance.
	/// </param>
	/// <param name="length">
	/// The number of <see cref="Byte"/> in the referenced array to use.
	/// </param>
	/// <param name="useFullLength">
	/// Indicates whether the total length of the referenced array should be used.
	/// </param>
	private CString(IntPtr ptr, Int32 length, Boolean useFullLength)
	{
		this._isLocal = false;
		this._data = ValueRegion<Byte>.Create(ptr, length);
		this.IsFunction = false;

		ReadOnlySpan<Byte> data = this._data;

		if (data.IsEmpty)
		{
			this._isNullTerminated = false;
			this._length = 0;
			return;
		}

		if (!useFullLength)
		{
			this._isNullTerminated = CString.IsNullTerminatedSpan(data, out this._length);
			return;
		}

		this._isNullTerminated = false;
		this._length = length;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class using the binary internal
	/// information given.
	/// </summary>
	/// <param name="bytes">Binary internal information representing the UTF-8 string.</param>
	/// <param name="isNullTerminated">
	/// Indicates whether <paramref name="bytes"/> is a null-terminated UTF-8 text.
	/// If this is <see langword="null"/> an internal function is used to determine the value.
	/// </param>
	private CString(Byte[] bytes, Boolean? isNullTerminated = default)
	{
		this._isLocal = true;
		this._data = ValueRegion<Byte>.Create(bytes);
		this.IsFunction = false;

		if (!isNullTerminated.HasValue)
		{
			this._isNullTerminated = CString.IsNullTerminatedSpan(bytes, out this._length);
			return;
		}

		this._length = bytes.Length;
		this._isNullTerminated = isNullTerminated.Value;
		if (this._isNullTerminated)
			this._length--;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class that contains the UTF-8 string
	/// returned by the specified <see cref="ReadOnlySpanFunc{Byte}"/>.
	/// </summary>
	/// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns the UTF-8 string.</param>
	/// <param name="isLiteral">Indicates whether returned span is from UTF-8 literal.</param>
	private CString(ReadOnlySpanFunc<Byte> func, Boolean isLiteral)
	{
		this._isLocal = false;
		this._data = ValueRegion<Byte>.Create(func);
		this.IsFunction = true;

		ReadOnlySpan<Byte> data = func();
		if (!isLiteral)
		{
			this._isNullTerminated = CString.IsNullTerminatedSpan(data, out this._length);
			return;
		}

		this._isNullTerminated = true;
		this._length = data.Length;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a
	/// specified sub-range of the <see cref="CString"/> instance.
	/// </summary>
	/// <param name="value">
	/// A <see cref="CString"/> value from which the sub-range is extracted.
	/// </param>
	/// <param name="startIndex">
	/// The zero-based starting index of the sub-range in <paramref name="value"/>.
	/// </param>
	/// <param name="length">The length of the sub-range.</param>
	private CString(CString value, Int32 startIndex, Int32 length)
	{
		this._isLocal = value._isLocal;
		this._length = length;
		this._data = value._data.InternalSlice(startIndex, value.GetDataLength(startIndex, length));
		this._isNullTerminated =
			(value is { IsFunction: true, _isNullTerminated: true, } && value._length - startIndex == length) ||
			this._data.AsSpan()[^1] == default;

		this.IsFunction = value.IsFunction;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value
	/// indicated by a specified UTF-16 text.
	/// </summary>
	/// <param name="utf16Text">The UTF-16 text to convert and assign to the new instance.</param>
	private CString(String utf16Text)
	{
		this._data = CString.CreateUtf8Region(utf16Text, out this._length);
		this._isLocal = true;
		this._isNullTerminated = true;

		this.IsFunction = true;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value
	/// indicated by <paramref name="data"/>
	/// </summary>
	/// <param name="data">Internal instance data.</param>
	/// <param name="isFunction">Indicates whether the UTF-8 data is retrieved using a function.</param>
	/// <param name="isNullTerminated">Indicates whether the UTF-8 text is null-terminated.</param>
	/// <param name="length">UTF-8 text length.</param>
	private CString(ValueRegion<Byte> data, Boolean isFunction, Boolean isNullTerminated, Int32 length)
	{
		this._isLocal = !isFunction && data is ValueRegion<Byte>.Memory;
		this._data = data;
		this._isNullTerminated = isNullTerminated;
		this._length = length;

		this.IsFunction = isFunction;
	}
#if !PACKAGE || NETCOREAPP
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class reading a String from <paramref name="reader"/>.
	/// </summary>
	/// <param name="reader">A <see cref="Utf8JsonReader"/> instance.</param>
	private CString(Utf8JsonReader reader)
	{
		this._isLocal = true;
		this._isNullTerminated = true;
		this._length = CString.Read(reader, out this._data);

		this.IsFunction = false;
	}
#endif
}