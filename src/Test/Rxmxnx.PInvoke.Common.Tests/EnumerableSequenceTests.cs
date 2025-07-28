#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EnumerableSequenceTests
{
	private static readonly String notStartedError = IMessageResource.GetInstance().NotStartedEnumerable;
	private static readonly String finishedError = IMessageResource.GetInstance().FinishedEnumerable;
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public Task BooleanTestAsync() => EnumerableSequenceTests.TestAsync<Boolean>();
	[Fact]
	public Task ByteTestAsync() => EnumerableSequenceTests.TestAsync<Byte>();
	[Fact]
	public Task Int16TestAsync() => EnumerableSequenceTests.TestAsync<Int16>();
	[Fact]
	public Task CharTestAsync() => EnumerableSequenceTests.TestAsync<Char>();
	[Fact]
	public Task Int32TestAsync() => EnumerableSequenceTests.TestAsync<Int32>();
	[Fact]
	public Task Int64TestAsync() => EnumerableSequenceTests.TestAsync<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task Int128TestAsync() => EnumerableSequenceTests.TestAsync<Int128>();
#endif
	[Fact]
	public Task GuidTestAsync() => EnumerableSequenceTests.TestAsync<Guid>();
	[Fact]
	public Task SingleTestAsync() => EnumerableSequenceTests.TestAsync<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task HalfTestAsync() => EnumerableSequenceTests.TestAsync<Half>();
#endif
	[Fact]
	public Task DoubleTestAsync() => EnumerableSequenceTests.TestAsync<Double>();
	[Fact]
	public Task DecimalTestAsync() => EnumerableSequenceTests.TestAsync<Decimal>();
	[Fact]
	public Task DateTimeTestAsync() => EnumerableSequenceTests.TestAsync<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task TimeOnlyTestAsync() => EnumerableSequenceTests.TestAsync<TimeOnly>();
#endif
	[Fact]
	public Task TimeSpanTestAsync() => EnumerableSequenceTests.TestAsync<TimeSpan>();
	[Fact]
	public Task StringTestAsync() => EnumerableSequenceTests.TestAsync<String>();
	[Fact]
	public Task ObjectTestAsync() => EnumerableSequenceTests.TestAsync<Object>();
	[Fact]
	public Task NullableOBooleanTestAsync() => EnumerableSequenceTests.TestAsync<Boolean?>();
	[Fact]
	public Task NullableOByteTestAsync() => EnumerableSequenceTests.TestAsync<Byte?>();
	[Fact]
	public Task NullableOInt16TestAsync() => EnumerableSequenceTests.TestAsync<Int16?>();
	[Fact]
	public Task NullableOCharTestAsync() => EnumerableSequenceTests.TestAsync<Char?>();
	[Fact]
	public Task NullableOInt32TestAsync() => EnumerableSequenceTests.TestAsync<Int32?>();
	[Fact]
	public Task NullableOInt64TestAsync() => EnumerableSequenceTests.TestAsync<Int64?>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task NullableOInt128TestAsync() => EnumerableSequenceTests.TestAsync<Int128?>();
#endif
	[Fact]
	public Task NullableOGuidTestAsync() => EnumerableSequenceTests.TestAsync<Guid?>();
	[Fact]
	public Task NullableOSingleTestAsync() => EnumerableSequenceTests.TestAsync<Single?>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task NullableOHalfTestAsync() => EnumerableSequenceTests.TestAsync<Half?>();
#endif
	[Fact]
	public Task NullableODoubleTestAsync() => EnumerableSequenceTests.TestAsync<Double?>();
	[Fact]
	public Task NullableODecimalTestAsync() => EnumerableSequenceTests.TestAsync<Decimal?>();
	[Fact]
	public Task NullableODateTimeTestAsync() => EnumerableSequenceTests.TestAsync<DateTime?>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task NullableOTimeOnlyTestAsync() => EnumerableSequenceTests.TestAsync<TimeOnly?>();
#endif
	[Fact]
	public Task NullableOTimeSpanTestAsync() => EnumerableSequenceTests.TestAsync<TimeSpan?>();
	[Fact]
	public Task NullableOStringTestAsync() => EnumerableSequenceTests.TestAsync<String?>();
	[Fact]
	public Task NullableObjectTestAsync() => EnumerableSequenceTests.TestAsync<Object?>();

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
		PInvokeAssert.Equal(values.Length, sequence.GetSize());
		for (Int32 i = 0; i < values.Length; i++)
			PInvokeAssert.Equal(values[i], sequence.GetItem(i));
		PInvokeAssert.Equal(values, sequence);
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
		PInvokeAssert.Equal(EnumerableSequenceTests.notStartedError,
		                    PInvokeAssert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
		Int32 index = -1;
		while (enumerator.MoveNext())
		{
			index++;
			PInvokeAssert.Equal(values[index], enumerator.Current);
		}
		PInvokeAssert.Equal(values.Length, index + 1);
		PInvokeAssert.Equal(EnumerableSequenceTests.finishedError,
		                    PInvokeAssert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
	}
}