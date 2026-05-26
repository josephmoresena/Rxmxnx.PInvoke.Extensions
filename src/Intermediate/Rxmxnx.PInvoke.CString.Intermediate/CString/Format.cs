#if NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public partial class CString : ISpanFormattable
#if NET7_0_OR_GREATER
	, ISpanParsable<CString>
#endif
#if NET8_0_OR_GREATER
	, IUtf8SpanFormattable, IUtf8SpanParsable<CString>
#endif
{
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	String IFormattable.ToString(String? format, IFormatProvider? formatProvider) => this.ToString();
	Boolean ISpanFormattable.TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format,
		IFormatProvider? provider)
	{
		ReadOnlySpan<Byte> utf8Bytes = this.AsSpan();
		if (utf8Bytes.IsEmpty)
		{
			charsWritten = 0;
			return true;
		}
		OperationStatus status = Utf8.ToUtf16(utf8Bytes, destination, out _, out charsWritten);
		return status == OperationStatus.Done;
	}
#if NET7_0_OR_GREATER
	static CString IParsable<CString>.Parse(String s, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		return CString.TryParse(s, out CString? result) ? result : throw new FormatException();
	}
	static Boolean IParsable<CString>.TryParse([NotNullWhen(true)] String? s, IFormatProvider? provider,
		[MaybeNullWhen(false)] out CString result)
	{
		if (s is not null) return CString.TryParse(s, out result);
		result = null;
		return false;
	}
	static CString ISpanParsable<CString>.Parse(ReadOnlySpan<Char> s, IFormatProvider? provider)
		=> CString.TryParse(s, out CString? result) ? result : throw new FormatException();
	static Boolean ISpanParsable<CString>.TryParse(ReadOnlySpan<Char> s, IFormatProvider? provider,
		[MaybeNullWhen(false)] out CString result)
		=> CString.TryParse(s, out result);
#if NET8_0_OR_GREATER
	Boolean IUtf8SpanFormattable.TryFormat(Span<Byte> utf8Destination, out Int32 bytesWritten,
		ReadOnlySpan<Char> format, IFormatProvider? provider)
	{
		ReadOnlySpan<Byte> source = this.AsSpan();
		if (source.TryCopyTo(utf8Destination))
		{
			bytesWritten = source.Length;
			return true;
		}
		bytesWritten = 0;
		return false;
	}
	static CString IUtf8SpanParsable<CString>.Parse(ReadOnlySpan<Byte> utf8Text, IFormatProvider? provider)
		=> CString.TryParse(utf8Text, out CString result) ? result : throw new FormatException();
	static Boolean IUtf8SpanParsable<CString>.TryParse(ReadOnlySpan<Byte> utf8Text, IFormatProvider? provider,
		[MaybeNullWhen(false)] out CString result)
		=> CString.TryParse(utf8Text, out result);

	/// <inheritdoc cref="IUtf8SpanParsable{CString}.TryParse(ReadOnlySpan{Byte}, IFormatProvider, out CString)"/>
	private static Boolean TryParse(ReadOnlySpan<Byte> utf8Text, out CString result)
	{
		if (utf8Text.IsEmpty)
		{
			result = CString.Empty;
			return true;
		}
		_ = CString.IsNullTerminatedSpan(utf8Text, out Int32 length);
		Byte[] utf8Buffer = CString.CreateByteArray(length + 1);
		utf8Text[..length].CopyTo(utf8Buffer);
#if NET5_0_OR_GREATER
		utf8Buffer[^1] = default;
#endif
		result = new(utf8Buffer, true);
		return true;
	}
#endif
	/// <inheritdoc cref="ISpanParsable{CString}.TryParse(ReadOnlySpan{Char}, IFormatProvider, out CString)"/>
	private static Boolean TryParse(ReadOnlySpan<Char> s, [MaybeNullWhen(false)] out CString result)
	{
		if (s.IsEmpty)
		{
			result = CString.Empty;
			return true;
		}
		Int32 maxBytes = Encoding.UTF8.GetByteCount(s);
		Byte[] utf8Buffer = CString.CreateByteArray(maxBytes + 1);
		if (Utf8.FromUtf16(s, utf8Buffer, out Int32 _, out Int32 _) != OperationStatus.Done)
		{
			result = default;
			return false;
		}
#if NET5_0_OR_GREATER
		utf8Buffer[^1] = default;
#endif
		result = new(utf8Buffer, true);
		return true;
	}
#endif
}
#endif