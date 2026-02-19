#if NETCOREAPP
using CStringJsonConverter = Rxmxnx.PInvoke.CString.JsonConverter;
#else
using CStringJsonConverter = Rxmxnx.PInvoke.Json.CStringJsonConverter;
#endif
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class SerializationTest
{
	private static readonly JsonSerializerOptions jsonOptions = new() { Converters = { new CStringJsonConverter(), }, };

	[Fact]
	public void UnicodePrefixTest() => PInvokeAssert.True(TextUnescape.UnicodePrefix.SequenceEqual("\\u"u8));
	[Fact]
	public void EscapedSpanTest()
	{
		PInvokeAssert.StrictEqual(CString.Empty, CString.Unescape([]));

		SerializationTest.AssertUnescaped(new(() => TextUnescape.UnicodePrefix));
		SerializationTest.AssertUnescaped(TextContainer.Slash);
		SerializationTest.AssertUnescaped(TextContainer.NewLine);
		SerializationTest.AssertUnescaped(TextContainer.Tab);
		SerializationTest.AssertUnescaped(TextContainer.Quotes);

		foreach (ReadOnlySpanFunc<Byte> text in TestSet.Utf8Text)
			SerializationTest.AssertUnescaped(new(text));
	}
	[Fact]
	public void EscapedSequenceTest()
	{
		for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
		{
			ReadOnlySpanFunc<Byte> text = TestSet.Utf8Text[i];
			Byte[] systemEncoded = JsonEncodedText.Encode(text()).EncodedUtf8Bytes.ToArray();
			Byte[] newtonsoftEncoded = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(TestSet.Utf16Text[i])[1..^1]);
			SerializationTest.EscapedSequenceAssert(text, systemEncoded);
			SerializationTest.EscapedSequenceAssert(text, newtonsoftEncoded);
		}
	}

	[Theory]
	[InlineData(JsonIgnoreCondition.WhenWritingNull)]
	[InlineData(JsonIgnoreCondition.WhenWritingDefault)]
	[InlineData(JsonIgnoreCondition.Never)]
	public void EmptyTest(JsonIgnoreCondition condition)
	{
		JsonSerializerOptions options = new()
		{
			DefaultIgnoreCondition = condition, Converters = { SerializationTest.jsonOptions.Converters.First(), },
		};
		TextContainer<String> valueS = new();
		TextContainer<CString> valueC = new();

		String vsSerialized = JsonSerializer.Serialize(valueS, options);
		String vcSerialized = JsonSerializer.Serialize(valueC, options);

		PInvokeAssert.Equal(vsSerialized, vcSerialized);

		Assert.Null(JsonSerializer.Deserialize<TextContainer<String>>(vcSerialized, options)?.Value);
		Assert.Null(JsonSerializer.Deserialize<TextContainer<CString>>(vsSerialized, options)?.Value);

		valueC.Value = CString.Zero;

		vcSerialized = JsonSerializer.Serialize(valueC, options);

		if (condition is JsonIgnoreCondition.WhenWritingDefault or JsonIgnoreCondition.WhenWritingNull)
		{
			// Zero is serialized as empty string when ignore condition is set to WhenWritingDefault or WhenWritingNull.
			// In .NET Core 3.1 and .NET Standard 2.1 Zero is serialized as non-ignorable null.
			Boolean isNetStandard = SystemInfo.CompilationFramework.EndsWith(" 3.0") ||
				SystemInfo.CompilationFramework.EndsWith(" 2.1");
			PInvokeAssert.NotEqual(vsSerialized, vcSerialized);
			PInvokeAssert.Equal(!isNetStandard ? String.Empty : null,
			                    JsonSerializer.Deserialize<TextContainer<String>>(vcSerialized)?.Value);
		}
		else
		{
			PInvokeAssert.Equal(vsSerialized, vcSerialized);
			Assert.Null(JsonSerializer.Deserialize<TextContainer<String>>(vcSerialized)?.Value);
		}
		Assert.Null(JsonSerializer.Deserialize<TextContainer<CString>>(vsSerialized)?.Value);

		valueS.Value = String.Empty;
		valueC.Value = CString.Empty;

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		PInvokeAssert.Equal(vsSerialized, vcSerialized);

		PInvokeAssert.Equal(0, JsonSerializer.Deserialize<TextContainer<String>>(vcSerialized, options)?.Value?.Length);
		PInvokeAssert.Equal(0, JsonSerializer.Deserialize<TextContainer<CString>>(vsSerialized, options)?.Value
		                                     ?.Length);
	}
	[Fact]
	public void SpanTest()
	{
		List<Int32> indices = TestSet.GetIndices();
		using TestMemoryHandle handle = new();
		StringBuilder builder = new();
		foreach (Int32 index in indices)
		{
			TextContainer<String> valueS = new() { Value = TestSet.GetString(index, true), };
			TextContainer<CString> valueC = new() { Value = TestSet.GetCString(index, handle), };
			SerializationTest.AssertSerialization(valueS, valueC);
			builder.Append(valueS.Value);
		}
		SerializationTest.AssertStringSerialization(builder.ToString());
	}
	[Fact]
	public void SlashTest()
		=> SerializationTest.AssertSerialization(TextContainer.Slash.Utf16, TextContainer.Slash.Utf8);
	[Fact]
	public void NewLineTest()
		=> SerializationTest.AssertSerialization(TextContainer.NewLine.Utf16, TextContainer.NewLine.Utf8);
	[Fact]
	public void TabTest() => SerializationTest.AssertSerialization(TextContainer.Tab.Utf16, TextContainer.Tab.Utf8);
	[Fact]
	public void QuoteTest()
		=> SerializationTest.AssertSerialization(TextContainer.Quotes.Utf16, TextContainer.Quotes.Utf8);

	private static void EscapedSequenceAssert(ReadOnlySpanFunc<Byte> text, Byte[] encoded)
	{
		Byte[] encodedStandard = JsonEncoderStandard.EncodeToUtf8Bytes(text());

		CString valueStandard = CString.Unescape(new ReadOnlySequence<Byte>(encodedStandard));
		CString value = CString.Unescape(new ReadOnlySequence<Byte>(encoded));

		SerializationTest.AssertUnescaped(text(), value);
		SerializationTest.AssertUnescaped(text(), valueStandard);
	}
	private static void AssertUnescaped(TextContainer unescaped)
	{
		ReadOnlySpan<Byte> systemEncoded = JsonEncodedText.Encode(unescaped.Utf8.Value).EncodedUtf8Bytes;
		ReadOnlySpan<Byte> newtonsoftEncoded =
			Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(unescaped.Utf16.Value)[1..^1]);
		SerializationTest.AssertUnescaped(unescaped, CString.Unescape(systemEncoded));
		SerializationTest.AssertUnescaped(unescaped, CString.Unescape(newtonsoftEncoded));
	}
	private static void AssertUnescaped(TextContainer unescaped, CString value)
	{
		Byte[] encodedStandard = JsonEncoderStandard.EncodeToUtf8Bytes(unescaped.Utf8.Value);
		CString valueStandard = CString.Unescape(encodedStandard);

		SerializationTest.AssertUnescaped(unescaped.Utf8.Value, valueStandard);
		SerializationTest.AssertUnescaped(unescaped.Utf8.Value, value);
	}
	private static void AssertUnescaped(ReadOnlySpan<Byte> unescaped, CString value)
	{
		PInvokeAssert.False(value.IsFunction);
		PInvokeAssert.False(value.IsReference);
		PInvokeAssert.False(value.IsZero);
		PInvokeAssert.False(value.IsSegmented);
		PInvokeAssert.True(value.IsNullTerminated);

		PInvokeAssert.True(unescaped.SequenceEqual(value.AsSpan()));
	}
	private static void AssertSerialization(TextContainer<String> valueS, TextContainer<CString> valueC)
	{
		String vsSerialized = JsonSerializer.Serialize(valueS, SerializationTest.jsonOptions);
		String vcSerialized = JsonSerializer.Serialize(valueC, SerializationTest.jsonOptions);
		CString? value = JsonSerializer.Deserialize<TextContainer<CString>>(vsSerialized, SerializationTest.jsonOptions)
		                               ?.Value;
		Int32? textLength = value?.Length;
		Byte[]? bytes = !CString.IsNullOrEmpty(value) ? CString.GetBytes(value) : default;
		Int32? byteCount = bytes?.Length;

		PInvokeAssert.Equal(vsSerialized, vcSerialized);
		PInvokeAssert.Equal(
			JsonSerializer.Deserialize<TextContainer<String>>(vcSerialized, SerializationTest.jsonOptions)?.Value,
			valueS.Value);

		if (valueC.Value is null || valueC.Value.IsZero)
		{
			Assert.Null(value);
			return;
		}

		Assert.NotNull(value);
		PInvokeAssert.Equal(value, valueC.Value);
		Assert.True(value!.IsNullTerminated);
		Assert.False(value.IsSegmented);
		Assert.False(value.IsZero);

		if (value.Equals(CString.Empty))
		{
			Assert.True(value.IsFunction);
			return;
		}

		Assert.False(value.IsFunction);
		Assert.NotNull(bytes);
		Assert.True(byteCount >= textLength + 1);
	}
	private static void AssertStringSerialization(String? value)
	{
		TextContainer<String> valueS = new() { Value = value, };
		TextContainer<CString> valueC = new() { Value = (CString?)value, };
		SerializationTest.AssertSerialization(valueS, valueC);
	}
}