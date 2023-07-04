namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IEnumerableSequenceTests
{
	private const String notStartedError = "Enumeration has not started. Call MoveNext.";
	private const String finishedError = "Enumeration already finished.";
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => IEnumerableSequenceTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => IEnumerableSequenceTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => IEnumerableSequenceTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => IEnumerableSequenceTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => IEnumerableSequenceTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => IEnumerableSequenceTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => IEnumerableSequenceTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => IEnumerableSequenceTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => IEnumerableSequenceTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => IEnumerableSequenceTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => IEnumerableSequenceTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => IEnumerableSequenceTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => IEnumerableSequenceTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => IEnumerableSequenceTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => IEnumerableSequenceTests.TestAsync<TimeSpan>();
	[Fact]
	internal Task StringTestAsync() => IEnumerableSequenceTests.TestAsync<String>();
	[Fact]
	internal Task ObjectTestAsync() => IEnumerableSequenceTests.TestAsync<Object>();
	[Fact]
	internal Task NullableOBooleanTestAsync() => IEnumerableSequenceTests.TestAsync<Boolean?>();
	[Fact]
	internal Task NullableOByteTestAsync() => IEnumerableSequenceTests.TestAsync<Byte?>();
	[Fact]
	internal Task NullableOInt16TestAsync() => IEnumerableSequenceTests.TestAsync<Int16?>();
	[Fact]
	internal Task NullableOCharTestAsync() => IEnumerableSequenceTests.TestAsync<Char?>();
	[Fact]
	internal Task NullableOInt32TestAsync() => IEnumerableSequenceTests.TestAsync<Int32?>();
	[Fact]
	internal Task NullableOInt64TestAsync() => IEnumerableSequenceTests.TestAsync<Int64?>();
	[Fact]
	internal Task NullableOInt128TestAsync() => IEnumerableSequenceTests.TestAsync<Int128?>();
	[Fact]
	internal Task NullableOGuidTestAsync() => IEnumerableSequenceTests.TestAsync<Guid?>();
	[Fact]
	internal Task NullableOSingleTestAsync() => IEnumerableSequenceTests.TestAsync<Single?>();
	[Fact]
	internal Task NullableOHalfTestAsync() => IEnumerableSequenceTests.TestAsync<Half?>();
	[Fact]
	internal Task NullableODoubleTestAsync() => IEnumerableSequenceTests.TestAsync<Double?>();
	[Fact]
	internal Task NullableODecimalTestAsync() => IEnumerableSequenceTests.TestAsync<Decimal?>();
	[Fact]
	internal Task NullableODateTimeTestAsync() => IEnumerableSequenceTests.TestAsync<DateTime?>();
	[Fact]
	internal Task NullableOTimeOnlyTestAsync() => IEnumerableSequenceTests.TestAsync<TimeOnly?>();
	[Fact]
	internal Task NullableOTimeSpanTestAsync() => IEnumerableSequenceTests.TestAsync<TimeSpan?>();
	[Fact]
	internal Task NullableOStringTestAsync() => IEnumerableSequenceTests.TestAsync<String?>();
	[Fact]
	internal Task NullableObjectTestAsync() => IEnumerableSequenceTests.TestAsync<Object?>();

	private static async Task TestAsync<T>()
	{
		T[] values = IEnumerableSequenceTests.fixture.CreateMany<T>().ToArray();
		IEnumerableSequence<T> sequence = new EnumerableSequence<T>(() => values);
		Task enumerableTestTask = Task.Run(() => IEnumerableSequenceTests.EnumerableTest(values, sequence));
		Task enumerationTestTask = Task.Run(() => IEnumerableSequenceTests.EnumeratorTest(values, sequence));
		await Task.WhenAll(enumerableTestTask, enumerationTestTask);
	}

	private static void EnumerableTest<T>(T[] values, IEnumerableSequence<T> sequence)
	{
		Assert.Equal(values.Length, sequence.GetSize());
		for (Int32 i = 0; i < values.Length; i++)
			Assert.Equal(values[i], sequence.GetItem(i));
		Assert.Equal(values, sequence);
	}

	private static void EnumeratorTest<T>(T[] values, IEnumerableSequence<T> sequence)
	{
		IEnumerator<T> enumerator = sequence.GetEnumerator();
		IEnumerableSequenceTests.TestEnumerator(values, enumerator);
		enumerator.Reset();
		IEnumerableSequenceTests.TestEnumerator(values, enumerator);
	}

	private static void TestEnumerator<T>(T[] values, IEnumerator<T> enumerator)
	{
		Assert.Equal(IEnumerableSequenceTests.notStartedError,
		             Assert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
		Int32 index = -1;
		while (enumerator.MoveNext())
		{
			index++;
			Assert.Equal(values[index], enumerator.Current);
		}
		Assert.Equal(values.Length, index + 1);
		Assert.Equal(IEnumerableSequenceTests.finishedError,
		             Assert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
	}
}