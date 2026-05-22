#if NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class FormattableTests
{
	[Fact]
	public void SpanFormattableTest()
	{
		Span<Char> destination = stackalloc Char[512];

		Assert.True(((ISpanFormattable)CString.Empty).TryFormat(destination, out Int32 charsWritten, default, null));

		Assert.Equal(0, charsWritten);

		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		{
			CString cstr = new(TestSet.Utf8Text[i]);
			String text = TestSet.Utf16Text[i];

			Boolean formatted = ((ISpanFormattable)cstr).TryFormat(destination, out charsWritten, default, null);

			Assert.True(formatted);
			Assert.Equal(text.Length, charsWritten);
			Assert.Equal(text, new(destination[..charsWritten]));
		}
	}

	[Fact]
	public void SpanFormattableInsufficientBufferTest()
	{
		Span<Char> destination = stackalloc Char[1];

		foreach (String text in TestSet.Utf16Text.Where(s => !String.IsNullOrEmpty(s)))
		{
			CString cstr = (CString)text;
			Boolean formatted = ((ISpanFormattable)cstr).TryFormat(destination, out Int32 charsWritten, default, null);
			Assert.False(formatted);
			Assert.True(charsWritten <= destination.Length);
		}
	}

#if NET7_0_OR_GREATER
	[Fact]
	public void ParsableTest()
	{
		foreach (String text in TestSet.Utf16Text)
		{
			CString parsed = FormattableTests.Parse<CString>(text);

			Assert.Equal(text, parsed.ToString());
			Assert.True(parsed.Equals(text));

			Boolean success = FormattableTests.TryParse(text, out CString? tryParsed);
			Assert.True(success);
			Assert.Equal(parsed, tryParsed);
		}
	}

	[Fact]
	public void ParsableNullTest()
	{
		Assert.Throws<ArgumentNullException>(() => FormattableTests.Parse<CString>(default(String)!));
		Boolean success = FormattableTests.TryParse(default(String)!, out CString? result);

		Assert.False(success);
		Assert.Null(result);
	}

	[Fact]
	public void SpanParsableTest()
	{
		foreach (String text in TestSet.Utf16Text)
		{
			CString parsed = FormattableTests.Parse<CString>(text.AsSpan());

			Assert.Equal(text, parsed.ToString());

			Boolean success = FormattableTests.TryParse(text.AsSpan(), out CString? tryParsed);

			Assert.True(success);
			Assert.Equal(parsed, tryParsed);
		}
	}

	[Fact]
	public void SpanParsableEmptyTest()
	{
		Boolean success = FormattableTests.TryParse(ReadOnlySpan<Char>.Empty, out CString? result);
		Assert.True(success);
		Assert.Equal(CString.Empty, result);
	}

	private static T Parse<T>(String txt) where T : IParsable<T> => T.Parse(txt, default);
	private static Boolean TryParse<T>(String txt, out T? result) where T : IParsable<T>
		=> T.TryParse(txt, default, out result);
	private static T Parse<T>(ReadOnlySpan<Char> chars) where T : ISpanParsable<T> => T.Parse(chars, default);
	private static Boolean TryParse<T>(ReadOnlySpan<Char> chars, out T? result) where T : ISpanParsable<T>
		=> T.TryParse(chars, default, out result);
#endif

#if NET8_0_OR_GREATER
	[Fact]
	public void Utf8SpanFormattableTest()
	{
		Span<Byte> destination = stackalloc Byte[512];

		Assert.True(
			((IUtf8SpanFormattable)CString.Empty).TryFormat(destination, out Int32 bytesWritten, default, null));

		Assert.Equal(0, bytesWritten);

		for (Int32 i = 0; i < TestSet.Utf8Bytes.Count; i++)
		{
			CString cstr = new(TestSet.Utf8Text[i]);
			Byte[] utf8Bytes = TestSet.Utf8Bytes[i];

			Boolean formatted = ((IUtf8SpanFormattable)cstr).TryFormat(destination, out bytesWritten, default, null);

			Assert.True(formatted);
			Assert.Equal(utf8Bytes.Length, bytesWritten);
			Assert.True(destination[..bytesWritten].SequenceEqual(utf8Bytes));
		}
	}

	[Fact]
	public void Utf8SpanFormattableInsufficientBufferTest()
	{
		Span<Byte> destination = stackalloc Byte[1];

		foreach (String text in TestSet.Utf16Text.Where(s => !String.IsNullOrEmpty(s)))
		{
			CString cstr = (CString)text;

			Boolean formatted =
				((IUtf8SpanFormattable)cstr).TryFormat(destination, out Int32 bytesWritten, default, null);

			Assert.False(formatted);
			Assert.Equal(0, bytesWritten);
		}
	}

	[Fact]
	public void Utf8SpanParsableTest()
	{
		for (Int32 i = 0; i < TestSet.Utf8Bytes.Count; i++)
		{
			Byte[] utf8Bytes = TestSet.Utf8Bytes[i];
			String text = TestSet.Utf16Text[i];

			CString parsed = FormattableTests.Parse<CString>(utf8Bytes);
			Assert.Equal(text, parsed.ToString());

			Boolean success = FormattableTests.TryParse(utf8Bytes, out CString? tryParsed);

			Assert.True(success);
			Assert.Equal(parsed, tryParsed);
		}
	}

	[Fact]
	public void Utf8SpanParsableNullTerminatedTest()
	{
		for (Int32 i = 0; i < TestSet.Utf8NullTerminatedBytes.Count; i++)
		{
			Byte[] utf8Bytes = TestSet.Utf8NullTerminatedBytes[i];

			String text = TestSet.Utf16Text[i];

			Boolean success = FormattableTests.TryParse(utf8Bytes, out CString? result);
			Assert.True(success);
			Assert.Equal(text, result?.ToString());
		}
	}

	[Fact]
	public void Utf8SpanParsableEmptyTest()
	{
		Boolean success = FormattableTests.TryParse(ReadOnlySpan<Byte>.Empty, out CString? result);
		Assert.True(success);
		Assert.Equal(CString.Empty, result);
	}
	private static T Parse<T>(ReadOnlySpan<Byte> utf8Text) where T : IUtf8SpanParsable<T> => T.Parse(utf8Text, default);
	private static Boolean TryParse<T>(ReadOnlySpan<Byte> utf8Text, out T? result) where T : IUtf8SpanParsable<T>
		=> T.TryParse(utf8Text, default, out result);
#endif
}
#endif