namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
public sealed class CreateReadOnlySpanTest : FixedContextTestsBase
{
    [Fact]
    internal void BooleanTest() => this.Test<Boolean>();
    [Fact]
    internal void ByteTest() => this.Test<Byte>();
    [Fact]
    internal void Int16Test() => this.Test<Int16>();
    [Fact]
    internal void CharTest() => this.Test<Char>();
    [Fact]
    internal void Int32Test() => this.Test<Int32>();
    [Fact]
    internal void Int64Test() => this.Test<Int64>();
    [Fact]
    internal void Int128Test() => this.Test<Int128>();
    [Fact]
    internal void GuidTest() => this.Test<Guid>();
    [Fact]
    internal void SingleTest() => this.Test<Single>();
    [Fact]
    internal void HalfTest() => this.Test<Half>();
    [Fact]
    internal void DoubleTest() => this.Test<Double>();
    [Fact]
    internal void DecimalTest() => this.Test<Decimal>();
    [Fact]
    internal void DateTimeTest() => this.Test<DateTime>();
    [Fact]
    internal void TimeOnlyTest() => this.Test<TimeOnly>();
    [Fact]
    internal void TimeSpanTest() => this.Test<TimeSpan>();

    private void Test<T>() where T : unmanaged
    {
        T[] values = fixture.CreateMany<T>().ToArray();
        base.WithFixed(values, false, Test);
        base.WithFixed(values, true, Test);
    }

    private static void Test<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
    {
        ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(values.Length);
        Assert.Equal(values.Length, span.Length);
        Assert.Equal(values.Length, ctx.Count);
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(values[0]), ref Unsafe.AsRef(span[0])));

        ctx.Unload();
        Exception invalid = Assert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
        Assert.Equal(InvalidError, invalid.Message);
    }
}

