namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class SerializationTest
{
	private static readonly JsonSerializerOptions
		jsonOptions = new() { Converters = { new CString.JsonConverter(), }, };

	[Theory]
	[InlineData(JsonIgnoreCondition.WhenWritingNull)]
	[InlineData(JsonIgnoreCondition.WhenWritingDefault)]
	[InlineData(JsonIgnoreCondition.Never)]
	internal void EmptyTest(JsonIgnoreCondition condition)
	{
		JsonSerializerOptions options = new()
		{
			DefaultIgnoreCondition = condition, Converters = { new CString.JsonConverter(), },
		};
		Serializable<String> valueS = new();
		Serializable<CString> valueC = new();

		String vsSerialized = JsonSerializer.Serialize(valueS, options);
		String vcSerialized = JsonSerializer.Serialize(valueC, options);

		Assert.Equal(vsSerialized, vcSerialized);

		Assert.Null(JsonSerializer.Deserialize<Serializable<String>>(vcSerialized)?.Value);
		Assert.Null(JsonSerializer.Deserialize<Serializable<CString>>(vsSerialized)?.Value);

		valueC.Value = CString.Zero;

		vcSerialized = JsonSerializer.Serialize(valueC, options);

		if (condition is JsonIgnoreCondition.WhenWritingDefault or JsonIgnoreCondition.WhenWritingNull)
		{
			// Zero is serialized as empty string when ignore condition is set to WhenWritingDefault or WhenWritingNull.
			Assert.NotEqual(vsSerialized, vcSerialized);
			Assert.Equal(String.Empty, JsonSerializer.Deserialize<Serializable<String>>(vcSerialized)?.Value);
		}
		else
		{
			Assert.Equal(vsSerialized, vcSerialized);
			Assert.Null(JsonSerializer.Deserialize<Serializable<String>>(vcSerialized)?.Value);
		}
		Assert.Null(JsonSerializer.Deserialize<Serializable<CString>>(vsSerialized)?.Value);

		valueS.Value = String.Empty;
		valueC.Value = CString.Empty;

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		Assert.Equal(vsSerialized, vcSerialized);

		Assert.Equal(0, JsonSerializer.Deserialize<Serializable<String>>(vcSerialized)?.Value?.Length);
		Assert.Equal(0, JsonSerializer.Deserialize<Serializable<CString>>(vsSerialized)?.Value?.Length);
	}
	[Fact]
	internal void SpanTest()
	{
		List<Int32> indices = TestSet.GetIndices();
		using TestMemoryHandle handle = new();
		StringBuilder builder = new();
		foreach (Int32 index in indices)
		{
			Serializable<String> valueS = new() { Value = TestSet.GetString(index, true), };
			Serializable<CString> valueC = new() { Value = TestSet.GetCString(index, handle), };
			SerializationTest.AssertSerialization(valueS, valueC);
			builder.Append(valueS.Value);
		}
		SerializationTest.AssertStringSerialization(builder.ToString());
	}
	[Fact]
	internal void SlashTest()
	{
		Serializable<String> valueS = new()
		{
			Value = "Windows uses backslashes (\\) to separate directories in file paths",
		};
		Serializable<CString> valueC = new()
		{
			Value = new(() => "Windows uses backslashes (\\) to separate directories in file paths"u8),
		};

		SerializationTest.AssertSerialization(valueS, valueC);
	}
	[Fact]
	internal void NewLineTest()
	{
		Serializable<String> valueS = new()
		{
			Value =
				"In Windows new lines are represented by the combination of a carriage return and a line feed (\r\n) characters.",
		};
		Serializable<CString> valueC = new()
		{
			Value =
				new(()
					    => "In Windows new lines are represented by the combination of a carriage return and a line feed (\r\n) characters."u8),
		};

		SerializationTest.AssertSerialization(valueS, valueC);
	}
	[Fact]
	internal void TabTest()
	{
		Serializable<String> valueS = new() { Value = "Tabs are represented by the \t character in strings.", };
		Serializable<CString> valueC = new()
		{
			Value = new(() => "Tabs are represented by the \t character in strings."u8),
		};

		SerializationTest.AssertSerialization(valueS, valueC);
	}
	[Fact]
	internal void QuoteTest()
	{
		Serializable<String> valueS = new() { Value = "Quotes are represented by the \" character in strings.", };
		Serializable<CString> valueC = new()
		{
			Value = new(() => "Quotes are represented by the \" character in strings."u8),
		};

		SerializationTest.AssertSerialization(valueS, valueC);
	}

	private static void AssertSerialization(Serializable<String> valueS, Serializable<CString> valueC)
	{
		String vsSerialized = JsonSerializer.Serialize(valueS, SerializationTest.jsonOptions);
		String vcSerialized = JsonSerializer.Serialize(valueC, SerializationTest.jsonOptions);

		Assert.Equal(vsSerialized, vcSerialized);
		Assert.Equal(JsonSerializer.Deserialize<Serializable<String>>(vcSerialized)?.Value, valueS.Value);
		if (valueC.Value is not null && !valueC.Value.IsZero)
			Assert.Equal(JsonSerializer.Deserialize<Serializable<CString>>(vsSerialized)?.Value, valueC.Value);
		else
			Assert.Null(JsonSerializer.Deserialize<Serializable<CString>>(vsSerialized)?.Value);
	}
	private static void AssertStringSerialization(String? value)
	{
		Serializable<String> valueS = new() { Value = value, };
		Serializable<CString> valueC = new() { Value = (CString?)value, };
		SerializationTest.AssertSerialization(valueS, valueC);
	}

	public sealed class Serializable<TString> where TString : IEquatable<TString>, IEquatable<String>
	{
		public TString? Value { get; set; }
	}
}