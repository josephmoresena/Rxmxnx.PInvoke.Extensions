#if NETCOREAPP
using CStringSequenceJsonConverter = Rxmxnx.PInvoke.CStringSequence.JsonConverter;

#else
using CStringSequenceJsonConverter = Rxmxnx.PInvoke.Json.CStringSequenceJsonConverter;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class SerializationTest
{
	private static readonly JsonSerializerOptions jsonOptions =
		new() { Converters = { new CStringSequenceJsonConverter(), }, };

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
		Serializable<String?[], String> valueS = new();
		Serializable<CStringSequence, CString> valueC = new();

		String vsSerialized = JsonSerializer.Serialize(valueS, options);
		String vcSerialized = JsonSerializer.Serialize(valueC, options);

		PInvokeAssert.Equal(vsSerialized, vcSerialized);

		PInvokeAssert.Null(JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized, options)?.Values);
		PInvokeAssert.Null(JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized, options)
		                                 ?.Values);

		valueS.Values = [default,];
		valueC.Values = new(CString.Zero);

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		PInvokeAssert.Equal(vsSerialized, vcSerialized);
		PInvokeAssert.Null(
			JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized, options)?.Values?[0]);
		PInvokeAssert.True(JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized, options)
		                                 ?.Values?[0].IsZero);

		valueC.Values = new(default(CString));

		PInvokeAssert.Equal(vsSerialized, vcSerialized);
		PInvokeAssert.Null(
			JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized, options)?.Values?[0]);

		valueS.Values = [];
		valueC.Values = CStringSequence.Empty;

		vsSerialized = JsonSerializer.Serialize(valueS, options);
		vcSerialized = JsonSerializer.Serialize(valueC, options);

		PInvokeAssert.Equal(vsSerialized, vcSerialized);

		PInvokeAssert.Equal(0, JsonSerializer.Deserialize<Serializable<String[], String>>(vcSerialized, options)?.Values
		                                     ?.Length);
		PInvokeAssert.Equal(0, JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(vsSerialized, options)
		                                     ?.Values?.Count);
	}
	[Fact]
	public void Test()
	{
		Serializable<String?[], String> valueS = new()
		{
			Values = TestSet.GetIndices().Select(i => TestSet.GetString(i, true)).ToArray(),
		};
		Serializable<CStringSequence, CString> valueC = new() { Values = new(valueS.Values), };

		String vsSerialized = JsonSerializer.Serialize(valueS, SerializationTest.jsonOptions);
		String vcSerialized = JsonSerializer.Serialize(valueC, SerializationTest.jsonOptions);

		PInvokeAssert.Equal(vsSerialized, vcSerialized);

		PInvokeAssert.Equal(valueS.Values,
		                    JsonSerializer
			                    .Deserialize<Serializable<String[], String>>(
				                    vcSerialized, SerializationTest.jsonOptions)?.Values);
		PInvokeAssert.Equal(valueC.Values,
		                    JsonSerializer.Deserialize<Serializable<CStringSequence, CString>>(
			                    vsSerialized, SerializationTest.jsonOptions)!.Values);
	}

	public sealed class Serializable<TEnumerable, TString> where TString : IEquatable<TString>, IEquatable<String>
		where TEnumerable : IReadOnlyList<TString?>
	{
		public TEnumerable? Values { get; set; }
	}
}