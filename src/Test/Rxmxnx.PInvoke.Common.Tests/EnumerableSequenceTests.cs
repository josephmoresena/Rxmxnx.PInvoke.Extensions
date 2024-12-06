namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EnumerableSequenceTests
{
	private const String NotStartedError = "Enumeration has not started. Call MoveNext.";
	private const String FinishedError = "Enumeration already finished.";
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => EnumerableSequenceTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => EnumerableSequenceTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => EnumerableSequenceTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => EnumerableSequenceTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => EnumerableSequenceTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => EnumerableSequenceTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => EnumerableSequenceTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => EnumerableSequenceTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => EnumerableSequenceTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => EnumerableSequenceTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => EnumerableSequenceTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => EnumerableSequenceTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => EnumerableSequenceTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => EnumerableSequenceTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => EnumerableSequenceTests.TestAsync<TimeSpan>();
	[Fact]
	internal Task StringTestAsync() => EnumerableSequenceTests.TestAsync<String>();
	[Fact]
	internal Task ObjectTestAsync() => EnumerableSequenceTests.TestAsync<Object>();
	[Fact]
	internal Task NullableOBooleanTestAsync() => EnumerableSequenceTests.TestAsync<Boolean?>();
	[Fact]
	internal Task NullableOByteTestAsync() => EnumerableSequenceTests.TestAsync<Byte?>();
	[Fact]
	internal Task NullableOInt16TestAsync() => EnumerableSequenceTests.TestAsync<Int16?>();
	[Fact]
	internal Task NullableOCharTestAsync() => EnumerableSequenceTests.TestAsync<Char?>();
	[Fact]
	internal Task NullableOInt32TestAsync() => EnumerableSequenceTests.TestAsync<Int32?>();
	[Fact]
	internal Task NullableOInt64TestAsync() => EnumerableSequenceTests.TestAsync<Int64?>();
	[Fact]
	internal Task NullableOInt128TestAsync() => EnumerableSequenceTests.TestAsync<Int128?>();
	[Fact]
	internal Task NullableOGuidTestAsync() => EnumerableSequenceTests.TestAsync<Guid?>();
	[Fact]
	internal Task NullableOSingleTestAsync() => EnumerableSequenceTests.TestAsync<Single?>();
	[Fact]
	internal Task NullableOHalfTestAsync() => EnumerableSequenceTests.TestAsync<Half?>();
	[Fact]
	internal Task NullableODoubleTestAsync() => EnumerableSequenceTests.TestAsync<Double?>();
	[Fact]
	internal Task NullableODecimalTestAsync() => EnumerableSequenceTests.TestAsync<Decimal?>();
	[Fact]
	internal Task NullableODateTimeTestAsync() => EnumerableSequenceTests.TestAsync<DateTime?>();
	[Fact]
	internal Task NullableOTimeOnlyTestAsync() => EnumerableSequenceTests.TestAsync<TimeOnly?>();
	[Fact]
	internal Task NullableOTimeSpanTestAsync() => EnumerableSequenceTests.TestAsync<TimeSpan?>();
	[Fact]
	internal Task NullableOStringTestAsync() => EnumerableSequenceTests.TestAsync<String?>();
	[Fact]
	internal Task NullableObjectTestAsync() => EnumerableSequenceTests.TestAsync<Object?>();

	private static async Task TestAsync<T>()
	{
		T[] values = EnumerableSequenceTests.fixture.CreateMany<T>().ToArray();
		IEnumerableSequence<T> sequence = new EnumerableSequence<T>(() => values);
		Task enumerableTestTask = Task.Run(() => EnumerableSequenceTests.EnumerableTest(values, sequence));
		Task enumerationTestTask = Task.Run(() => EnumerableSequenceTests.EnumeratorTest(values, sequence));
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
		EnumerableSequenceTests.TestEnumerator(values, enumerator);
		enumerator.Reset();
		EnumerableSequenceTests.TestEnumerator(values, enumerator);
	}

	private static void TestEnumerator<T>(T[] values, IEnumerator<T> enumerator)
	{
		Assert.Equal(EnumerableSequenceTests.NotStartedError,
		             Assert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
		Int32 index = -1;
		while (enumerator.MoveNext())
		{
			index++;
			Assert.Equal(values[index], enumerator.Current);
		}
		Assert.Equal(values.Length, index + 1);
		Assert.Equal(EnumerableSequenceTests.FinishedError,
		             Assert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
	}
}