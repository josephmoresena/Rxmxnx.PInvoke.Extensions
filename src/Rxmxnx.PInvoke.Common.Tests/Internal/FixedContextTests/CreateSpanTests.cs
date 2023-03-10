using System.Runtime.CompilerServices;
using Rxmxnx.PInvoke.Internal;

namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

public sealed class CreateSpanTests : FixedContextTestsBase
{
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

    private void Test<T>() where T : unmanaged
    {
        T[] values = fixture.CreateMany<T>(10).ToArray();
        base.WithFixed(values, false, Test);
        Exception readOnly = Assert.Throws<InvalidOperationException>(() => base.WithFixed(values, true, Test));
        Assert.Equal(ReadOnlyError, readOnly.Message);
    }

    private static void Test<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
    {
        Span<T> span = ctx.CreateSpan<T>(values.Length);
        Assert.Equal(values.Length, span.Length);
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(values[0]), ref span[0]));

        ctx.Unload();
        Exception invalid = Assert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
        Assert.Equal(InvalidError, invalid.Message);
    }
}

