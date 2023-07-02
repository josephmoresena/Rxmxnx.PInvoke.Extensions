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
		ReadOnlySpan<Byte> data = this._data;
		if (data.IsEmpty)
		{
			this._isNullTerminated = false;
			this._length = 0;
		}
		else if (useFullLength)
		{
			this._isNullTerminated = false;
			this._length = length;
		}
		else
		{
			this._isNullTerminated = CString.IsNullTerminatedSpan(data, out Int32 textLength);
			this._length = textLength;
		}
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
		Int32 textLength = bytes.Length;
		this._isLocal = true;
		this._data = ValueRegion<Byte>.Create(bytes);
		this._isNullTerminated = isNullTerminated ?? CString.IsNullTerminatedSpan(bytes, out textLength);
		this._length = textLength - (isNullTerminated.GetValueOrDefault() ? 1 : 0);
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
		this._isFunction = true;
		this._data = ValueRegion<Byte>.Create(func);

		ReadOnlySpan<Byte> data = func();
		if (isLiteral)
		{
			this._isNullTerminated = true;
			this._length = data.Length;
		}
		else
		{
			this._isNullTerminated = CString.IsNullTerminatedSpan(data, out Int32 textLength);
			this._length = textLength;
		}
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
		this._isFunction = value._isFunction;
		this._length = length;
		this._data = value._data.InternalSlice(startIndex, value.GetDataLength(startIndex, length));
		this._isNullTerminated =
			(value is { _isFunction: true, _isNullTerminated: true } && value._length - startIndex == length) ||
			this._data.AsSpan()[^1] == default;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value
	/// indicated by a specified UTF-16 text.
	/// </summary>
	/// <param name="utf16Text">The UTF-16 text to convert and assign to the new instance.</param>
	private CString(String utf16Text)
	{
		Utf16ConversionHelper helper = new(utf16Text, out this._length);
		this._isLocal = true;
		this._isFunction = true;
		this._data = helper.AsRegion();
		this._isNullTerminated = true;
	}

	/// <summary>
	/// Helper class for UTF-16 to UTF-8 conversion.
	/// </summary>
	private sealed record Utf16ConversionHelper
	{
		/// <summary>
		/// Internal UTF-16 text which serves as buffer.
		/// </summary>
		private readonly String _utf8String;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="utf8Text">UTF-16 text to be converter.</param>
		/// <param name="length">Output. Length of UTF-8 text.</param>
		public Utf16ConversionHelper(String utf8Text, out Int32 length)
			=> this._utf8String = Utf16ConversionHelper.GetUtf8String(utf8Text, out length);

		/// <summary>
		/// Creates a <see cref="Utf16ConversionHelper"/> instance from current instance.
		/// </summary>
		/// <returns>A <see cref="ValueRegion{Byte}"/> created from current instance.</returns>
		public ValueRegion<Byte> AsRegion() => ValueRegion<Byte>.Create(this.GetUtf8Bytes);

		/// <summary>
		/// Retrieves the internal read-only span of UTF-8 bytes.
		/// </summary>
		/// <returns>A read-only span of UTF-8 bytes.</returns>
		private ReadOnlySpan<Byte> GetUtf8Bytes() => MemoryMarshal.AsBytes<Char>(this._utf8String);

		/// <summary>
		/// Creates a UTF-16 text which contains the binary information of <paramref name="str"/>
		/// encoded to UTF-8 text.
		/// </summary>
		/// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
		/// <param name="length">Output. Length of UTF-8 text.</param>
		/// <returns>UTF-16 text containing the UTF-8 text binary information.</returns>
		private static String GetUtf8String(String str, out Int32 length)
		{
			//Encodes String to UTF8 bytes.
			Byte[] bytes = Utf16ConversionHelper.GetUtf8Bytes(str);
			//Calculates the final UTF8 String length.
			Int32 stringLength = Utf16ConversionHelper.GetUtf8StringLength(bytes);
			//Creates final String.
			String result = String.Create(stringLength, bytes, CString.CopyBytes);

			//Try to fetch internal String
			length = bytes.Length;
			return String.IsInterned(result) ?? result;
		}

		/// <summary>
		/// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with
		/// UTF-8 text.
		/// </summary>
		/// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
		/// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
		private static Byte[] GetUtf8Bytes(String str)
			=> !String.IsNullOrEmpty(str) ? Encoding.UTF8.GetBytes(str) : Array.Empty<Byte>();

		/// <summary>
		/// Retrieves the UTF8 String length which contains <paramref name="bytes"/>
		/// information.
		/// </summary>
		/// <param name="bytes">UTF8 bytes.</param>
		/// <returns>UTF8 String length contains <paramref name="bytes"/> information.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Int32 GetUtf8StringLength(ReadOnlySpan<Byte> bytes)
		{
			Int32 result = (bytes.Length + 1) / sizeof(Char);
			if (bytes.Length % sizeof(Char) == 0)
				result++;
			return result;
		}
	}
}