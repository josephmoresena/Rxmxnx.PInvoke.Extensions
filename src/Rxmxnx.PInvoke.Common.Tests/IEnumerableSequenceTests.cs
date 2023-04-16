namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
public sealed class IEnumerableSequenceTests
{
    private const String notStartedError = "Enumeration has not started. Call MoveNext.";
    private const String finishedError = "Enumeration already finished.";
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal Task BooleanTestAsync() => TestAsync<Boolean>();
    [Fact]
    internal Task ByteTestAsync() => TestAsync<Byte>();
    [Fact]
    internal Task Int16TestAsync() => TestAsync<Int16>();
    [Fact]
    internal Task CharTestAsync() => TestAsync<Char>();
    [Fact]
    internal Task Int32TestAsync() => TestAsync<Int32>();
    [Fact]
    internal Task Int64TestAsync() => TestAsync<Int64>();
    [Fact]
    internal Task Int128TestAsync() => TestAsync<Int128>();
    [Fact]
    internal Task GuidTestAsync() => TestAsync<Guid>();
    [Fact]
    internal Task SingleTestAsync() => TestAsync<Single>();
    [Fact]
    internal Task HalfTestAsync() => TestAsync<Half>();
    [Fact]
    internal Task DoubleTestAsync() => TestAsync<Double>();
    [Fact]
    internal Task DecimalTestAsync() => TestAsync<Decimal>();
    [Fact]
    internal Task DateTimeTestAsync() => TestAsync<DateTime>();
    [Fact]
    internal Task TimeOnlyTestAsync() => TestAsync<TimeOnly>();
    [Fact]
    internal Task TimeSpanTestAsync() => TestAsync<TimeSpan>();
    [Fact]
    internal Task StringTestAsync() => TestAsync<String>();
    [Fact]
    internal Task ObjectTestAsync() => TestAsync<Object>();
    [Fact]
    internal Task NullableOBooleanTestAsync() => TestAsync<Boolean?>();
    [Fact]
    internal Task NullableOByteTestAsync() => TestAsync<Byte?>();
    [Fact]
    internal Task NullableOInt16TestAsync() => TestAsync<Int16?>();
    [Fact]
    internal Task NullableOCharTestAsync() => TestAsync<Char?>();
    [Fact]
    internal Task NullableOInt32TestAsync() => TestAsync<Int32?>();
    [Fact]
    internal Task NullableOInt64TestAsync() => TestAsync<Int64?>();
    [Fact]
    internal Task NullableOInt128TestAsync() => TestAsync<Int128?>();
    [Fact]
    internal Task NullableOGuidTestAsync() => TestAsync<Guid?>();
    [Fact]
    internal Task NullableOSingleTestAsync() => TestAsync<Single?>();
    [Fact]
    internal Task NullableOHalfTestAsync() => TestAsync<Half?>();
    [Fact]
    internal Task NullableODoubleTestAsync() => TestAsync<Double?>();
    [Fact]
    internal Task NullableODecimalTestAsync() => TestAsync<Decimal?>();
    [Fact]
    internal Task NullableODateTimeTestAsync() => TestAsync<DateTime?>();
    [Fact]
    internal Task NullableOTimeOnlyTestAsync() => TestAsync<TimeOnly?>();
    [Fact]
    internal Task NullableOTimeSpanTestAsync() => TestAsync<TimeSpan?>();
    [Fact]
    internal Task NullableOStringTestAsync() => TestAsync<String?>();
    [Fact]
    internal Task NullableObjectTestAsync() => TestAsync<Object?>();

    private static async Task TestAsync<T>()
    {
        T[] values = fixture.CreateMany<T>().ToArray();
        IEnumerableSequence<T> sequence = new EnumerableSequence<T>(() => values);
        Task enumerableTestTask = Task.Run(() => EnumerableTest(values, sequence));
        Task enumerationTestTask = Task.Run(() => EnumeratorTest(values, sequence));
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
        TestEnumerator(values, enumerator);
        enumerator.Reset();
        TestEnumerator(values, enumerator);
    }

    private static void TestEnumerator<T>(T[] values, IEnumerator<T> enumerator)
    {
        Assert.Equal(notStartedError, Assert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
        Int32 index = -1;
        while (enumerator.MoveNext())
        {
            index++;
            Assert.Equal(values[index], enumerator.Current);
        }
        Assert.Equal(values.Length, index + 1);
        Assert.Equal(finishedError, Assert.Throws<InvalidOperationException>(() => enumerator.Current).Message);
    }
}

