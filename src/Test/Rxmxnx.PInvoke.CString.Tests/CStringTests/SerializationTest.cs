namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class SerializationTest
{
	[Theory]
	[InlineData(JsonIgnoreCondition.WhenWritingNull)]
	[InlineData(JsonIgnoreCondition.WhenWritingDefault)]
	[InlineData(JsonIgnoreCondition.Never)]
	public void SimpleTest(JsonIgnoreCondition condition)
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

		Int32 index = TestSet.GetIndices(1)[0];

		valueS.Value = TestSet.GetString(index, true);
		valueC.Value = (CString?)valueS.Value;

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		Assert.Equal(vsSerialized, vcSerialized);

		Assert.Equal(JsonSerializer.Deserialize<Serializable<String>>(vcSerialized)?.Value, valueS.Value);
		Assert.Equal(JsonSerializer.Deserialize<Serializable<CString>>(vsSerialized)?.Value, valueC.Value);
	}

	public sealed class Serializable<TString> where TString : class, IEquatable<TString>, IEquatable<String>
	{
		public TString? Value { get; set; }
	}
}