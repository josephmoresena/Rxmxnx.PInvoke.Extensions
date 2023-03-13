using Rxmxnx.PInvoke.Tests.Internal;

namespace Rxmxnx.PInvoke.Tests;

public sealed class IEnumerableSequenceTests
{
    private const String notStartedError = "Enumeration has not started. Call MoveNext.";
    private const String finishedError = "Enumeration already finished.";
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal void BooleanTest() => Test<Boolean>();
    [Fact]
    internal void ByteTest() => Test<Byte>();
    [Fact]
    internal void Int16Test() => Test<Int16>();
    [Fact]
    internal void CharTest() => Test<Char>();
    [Fact]
    internal void Int32Test() => Test<Int32>();
    [Fact]
    internal void Int64Test() => Test<Int64>();
    [Fact]
    internal void Int128Test() => Test<Int128>();
    [Fact]
    internal void GuidTest() => Test<Guid>();
    [Fact]
    internal void SingleTest() => Test<Single>();
    [Fact]
    internal void HalfTest() => Test<Half>();
    [Fact]
    internal void DoubleTest() => Test<Double>();
    [Fact]
    internal void DecimalTest() => Test<Decimal>();
    [Fact]
    internal void DateTimeTest() => Test<DateTime>();
    [Fact]
    internal void TimeOnlyTest() => Test<TimeOnly>();
    [Fact]
    internal void TimeSpanTest() => Test<TimeSpan>();

    private static void Test<T>()
    {
        T[] values = fixture.CreateMany<T>().ToArray();
        IEnumerableSequence<T> sequence = new EnumerableSequence<T>(() => values);
        IEnumerator<T> enumerator = sequence.GetEnumerator();

        Assert.Equal(values.Length, sequence.GetSize());
        for (Int32 i = 0; i < values.Length; i++)
            Assert.Equal(values[i], sequence.GetItem(i));
        Assert.Equal(values, sequence);
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

