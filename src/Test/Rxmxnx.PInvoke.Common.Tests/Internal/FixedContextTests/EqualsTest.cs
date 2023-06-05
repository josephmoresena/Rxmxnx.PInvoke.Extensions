namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EqualsTest : FixedContextTestsBase
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

    private unsafe void Test<T>() where T : unmanaged
    {
        T[] values = fixture.CreateMany<T>(sizeof(Int128) * 3 / sizeof(T)).ToArray();
        base.WithFixed(values, true, Test);
        base.WithFixed(values, false, Test);
    }

    private static unsafe void Test<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
    {
        fixed (T* ptrValue = values)
        {
            Test(ctx, false, values.Length);
            Test(ctx, true, values.Length);
            FalseTest(ctx, values, ptrValue);
        }
    }

    private static unsafe void Test<T>(FixedContext<T> ctx, Boolean isReadOnly, Int32 length) where T : unmanaged
    {
        TransformationTest<T, Boolean>(ctx, isReadOnly, length);
        TransformationTest<T, Byte>(ctx, isReadOnly, length);
        TransformationTest<T, Int16>(ctx, isReadOnly, length);
        TransformationTest<T, Char>(ctx, isReadOnly, length);
        TransformationTest<T, Int32>(ctx, isReadOnly, length);
        TransformationTest<T, Int64>(ctx, isReadOnly, length);
        TransformationTest<T, Int128>(ctx, isReadOnly, length);
        TransformationTest<T, Single>(ctx, isReadOnly, length);
        TransformationTest<T, Half>(ctx, isReadOnly, length);
        TransformationTest<T, Double>(ctx, isReadOnly, length);
        TransformationTest<T, Decimal>(ctx, isReadOnly, length);
        TransformationTest<T, DateTime>(ctx, isReadOnly, length);
        TransformationTest<T, TimeOnly>(ctx, isReadOnly, length);
        TransformationTest<T, TimeSpan>(ctx, isReadOnly, length);
    }
    private static unsafe void TransformationTest<T, T2>(FixedContext<T> ctx, Boolean readOnly, Int32 length)
        where T : unmanaged
        where T2 : unmanaged
    {
        ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
        void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
        Int32 binaryLength = length * sizeof(T);
        ReadOnlySpan<T2> transformedSpan = new(ptr, binaryLength / sizeof(T2));

        Assert.Equal(binaryLength, ctx.BinaryLength);
        WithFixed(transformedSpan, readOnly, ctx, Test);
    }
    private static void Test<T, TInt>(FixedContext<TInt> ctx2, FixedContext<T> ctx)
        where T : unmanaged
        where TInt : unmanaged
    {
        Boolean equal = ctx.IsReadOnly == ctx2.IsReadOnly && typeof(TInt) == typeof(T);

        Assert.Equal(equal, ctx2.Equals(ctx));
        Assert.Equal(equal, ctx2.Equals((Object)ctx));
        Assert.Equal(equal, ctx2.Equals(ctx as FixedContext<TInt>));
        Assert.False(ctx2.Equals(null));
        Assert.False(ctx2.Equals(new Object()));
    }
    private static unsafe void FalseTest<T>(FixedContext<T> ctx, T[] values, T* ptrValue) where T : unmanaged
    {
        FixedContext<T> ctx1 = new(ptrValue, values.Length - 1, true);
        FixedContext<T> ctx2 = new(ptrValue, values.Length - 1, false);
        FixedContext<T> ctx3 = new(ptrValue + 1, values.Length - 1, true);
        FixedContext<T> ctx4 = new(ptrValue + 1, values.Length - 1, false);

        Assert.False(ctx.Equals(ctx1));
        Assert.False(ctx.Equals(ctx2));
        Assert.False(ctx.Equals(ctx3));
        Assert.False(ctx.Equals(ctx4));
        Assert.False(ctx1.Equals(ctx2));
        Assert.False(ctx1.Equals(ctx3));
        Assert.False(ctx1.Equals(ctx4));
        Assert.False(ctx2.Equals(ctx3));
        Assert.False(ctx2.Equals(ctx4));
        Assert.False(ctx3.Equals(ctx4));
    }
}

