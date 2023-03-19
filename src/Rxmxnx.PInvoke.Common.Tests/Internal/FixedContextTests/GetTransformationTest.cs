namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

public sealed class GetTransformationTest : FixedContextTestsBase
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
        Boolean isReadOnly = IsReadOnly(ctx);

        Test<T, Boolean>(ctx, isReadOnly);
        Test<T, Byte>(ctx, isReadOnly);
        Test<T, Int16>(ctx, isReadOnly);
        Test<T, Char>(ctx, isReadOnly);
        Test<T, Int32>(ctx, isReadOnly);
        Test<T, Int64>(ctx, isReadOnly);
        Test<T, Int128>(ctx, isReadOnly);
        Test<T, Single>(ctx, isReadOnly);
        Test<T, Half>(ctx, isReadOnly);
        Test<T, Double>(ctx, isReadOnly);
        Test<T, Decimal>(ctx, isReadOnly);
        Test<T, DateTime>(ctx, isReadOnly);
        Test<T, TimeOnly>(ctx, isReadOnly);
        Test<T, TimeSpan>(ctx, isReadOnly);

        ctx.Unload();
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Boolean>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Byte>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int16>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Char>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int32>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int64>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int128>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Single>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Half>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Double>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Decimal>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, DateTime>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, TimeOnly>(ctx, isReadOnly)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, TimeSpan>(ctx, isReadOnly)).Message);
    }
    private static void Test<T, T2>(FixedContext<T> ctx, Boolean isReadOnly)
        where T : unmanaged
        where T2 : unmanaged
    {
        TransformationContext<T, T2> result = ctx.GetTransformation<T2>(true);
        Assert.NotNull(result);
        unsafe
        {
            ContextTest(ctx, result);
        }
        if (!isReadOnly)
        {
            Object? result2 = ctx.GetTransformation<T2>(false);
            Assert.IsType<TransformationContext<T, T2>>(result2);
            Assert.Equal(result, result2);
        }
        else
        {
            Exception readOnly = Assert.Throws<InvalidOperationException>(() => ctx.GetTransformation<T2>(false));
            Assert.Equal(ReadOnlyError, readOnly.Message);
        }
    }
    private static unsafe void ContextTest<T, T2>(FixedContext<T> ctx, TransformationContext<T, T2> result)
        where T : unmanaged
        where T2 : unmanaged
    {
        Int32 countT2 = ctx.BinaryLength / sizeof(T2);
        Int32 offset = countT2 * sizeof(T2);
        FixedContext<T> ctx0 = result;
        FixedContext<T2> ctx1 = result;
        Assert.Equal(offset, result.Offset);
        Assert.Equal(ctx, ctx0);
        Assert.Equal(countT2, ctx1.Count);
        Assert.Equal(ctx.BinaryLength, ctx1.BinaryLength);
    }
}

