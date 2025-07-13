#if NETCOREAPP
namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class SerializationTest
{
	private static readonly JsonSerializerOptions jsonOptions =
		new() { Converters = { new CStringSequence.JsonConverter(), }, };

	[Theory]
	[InlineData(JsonIgnoreCondition.WhenWritingNull)]
	[InlineData(JsonIgnoreCondition.WhenWritingDefault)]
	[InlineData(JsonIgnoreCondition.Never)]
	internal void EmptyTest(JsonIgnoreCondition condition)
	{
		JsonSerializerOptions options = new()
		{
			DefaultIgnoreCondition = condition, Converters = { new CStringSequence.JsonConverter(), },
		};
		Serializable<String?[], String> valueS = new();
		Serializable<CStringSequence, CString> valueC = new();

		String vsSerialized = JsonSerializer.Serialize(valueS, options);
		String vcSerialized = JsonSerializer.Serialize(valueC, options);

		Assert.Equal(vsSerialized, vcSerialized);

		Assert.Null(JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized)?.Values);
		Assert.Null(JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized)?.Values);

		valueS.Values = [default,];
		valueC.Values = new(CString.Zero);

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		Assert.Equal(vsSerialized, vcSerialized);
		Assert.Null(JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized)?.Values?[0]);
		Assert.True(JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized)?.Values?[0]
		                          .IsZero);

		valueC.Values = new(default(CString));

		Assert.Equal(vsSerialized, vcSerialized);
		Assert.Null(JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized)?.Values?[0]);

		valueS.Values = [];
		valueC.Values = CStringSequence.Empty;

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		Assert.Equal(vsSerialized, vcSerialized);

		Assert.Equal(0, JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized)?.Values?.Length);
		Assert.Equal(
			0, JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized)?.Values?.Count);
	}
	[Fact]
	internal void Test()
	{
		Serializable<String?[], String> valueS = new()
		{
			Values = TestSet.GetIndices().Select(i => TestSet.GetString(i, true)).ToArray(),
		};
		Serializable<CStringSequence, CString> valueC = new() { Values = new(valueS.Values), };

		String vsSerialized = JsonSerializer.Serialize(valueS, SerializationTest.jsonOptions);
		String vcSerialized = JsonSerializer.Serialize(valueC, SerializationTest.jsonOptions);

		Assert.Equal(vsSerialized, vcSerialized);

		Assert.Equal(valueS.Values, JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized)?.Values);
		Assert.Equal(valueC.Values,
		             JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized)!.Values);
	}

	public sealed class Serializable<TEnumerable, TString> where TString : IEquatable<TString>, IEquatable<String>
		where TEnumerable : IReadOnlyList<TString?>
	{
		public TEnumerable? Values { get; set; }
	}
}
#endif