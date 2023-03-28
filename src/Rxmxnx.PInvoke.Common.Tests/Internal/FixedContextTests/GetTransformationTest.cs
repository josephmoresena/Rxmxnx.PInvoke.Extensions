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
        Boolean isReadOnly = ctx.IsReadOnly;

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
        FixedContext<T2> result = ctx.GetTransformation<T2>(out FixedOffset offset, true);
        Assert.NotNull(result);
        unsafe
        {
            ContextTest(ctx, offset, result, isReadOnly);
        }
        if (!isReadOnly)
        {
            FixedContext<T2> result2 = ctx.GetTransformation<T2>(out FixedOffset offset2, false);
            Assert.NotNull(result2);
            Assert.Equal(offset, offset2);
            Assert.Equal(result, result2);
        }
        else
        {
            Exception readOnly = Assert.Throws<InvalidOperationException>(() => ctx.GetTransformation<T2>(out FixedOffset offset2, false));
            Assert.Equal(ReadOnlyError, readOnly.Message);
        }
    }
    private static unsafe void ContextTest<T, T2>(FixedContext<T> ctx, FixedOffset offset, FixedContext<T2> result, Boolean isReadOnly)
        where T : unmanaged
        where T2 : unmanaged
    {
        Int32 countT2 = ctx.BinaryLength / sizeof(T2);
        Int32 offsetT2 = countT2 * sizeof(T2);
        HashCode hashResidual = new();
        hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref Unsafe.AsRef(ctx.CreateReadOnlyReference<Byte>()))));
        hashResidual.Add(offsetT2);
        hashResidual.Add(ctx.BinaryLength);
        hashResidual.Add(isReadOnly);

        Assert.Equal(countT2, result.Count);
        Assert.Equal(0, ctx.BinaryOffset);
        Assert.Equal(offsetT2, offset.BinaryOffset);
        Assert.Equal(ctx.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(), offset.CreateReadOnlyBinarySpan().ToArray());
        Assert.Equal(ctx.BinaryLength, result.BinaryLength);
        Assert.Equal(ctx.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
        Assert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());

        if (!isReadOnly)
            Assert.Equal(ctx.CreateBinarySpan()[offset.BinaryOffset..].ToArray(), offset.CreateBinarySpan().ToArray());
        else
            Assert.Equal(ReadOnlyError, Assert.Throws<InvalidOperationException>(() => offset.CreateBinarySpan()).Message);

        FixedOffset offset2;
        _ = ctx.GetTransformation<Boolean>(out offset2, true);
        OffsetTest<T2, Boolean>(offset, offset2);
        _ = ctx.GetTransformation<Byte>(out offset2, true);
        OffsetTest<T2, Byte>(offset, offset2);
        _ = ctx.GetTransformation<Int16>(out offset2, true);
        OffsetTest<T2, Int16>(offset, offset2);
        _ = ctx.GetTransformation<Char>(out offset2, true);
        OffsetTest<T2, Char>(offset, offset2);
        _ = ctx.GetTransformation<Int32>(out offset2, true);
        OffsetTest<T2, Int32>(offset, offset2);
        _ = ctx.GetTransformation<Int64>(out offset2, true);
        OffsetTest<T2, Int64>(offset, offset2);
        _ = ctx.GetTransformation<Int128>(out offset2, true);
        OffsetTest<T2, Int128>(offset, offset2);
        _ = ctx.GetTransformation<Single>(out offset2, true);
        OffsetTest<T2, Single>(offset, offset2);
        _ = ctx.GetTransformation<Half>(out offset2, true);
        OffsetTest<T2, Half>(offset, offset2);
        _ = ctx.GetTransformation<Double>(out offset2, true);
        OffsetTest<T2, Double>(offset, offset2);
        _ = ctx.GetTransformation<Decimal>(out offset2, true);
        OffsetTest<T2, Decimal>(offset, offset2);
        _ = ctx.GetTransformation<DateTime>(out offset2, true);
        OffsetTest<T2, DateTime>(offset, offset2);
        _ = ctx.GetTransformation<TimeOnly>(out offset2, true);
        OffsetTest<T2, TimeOnly>(offset, offset2);
        _ = ctx.GetTransformation<TimeSpan>(out offset2, true);
        OffsetTest<T2, TimeSpan>(offset, offset2);
    }

    private static unsafe void OffsetTest<T2, T3>(FixedOffset offset1, FixedOffset offset2)
        where T2 : unmanaged
        where T3 : unmanaged
    {
        Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
        Assert.Equal(equal, offset1.Equals(offset2));
        Assert.Equal(equal, offset1.Equals((Object)offset2));
        if (equal)
            Assert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(), offset2.CreateReadOnlyBinarySpan().ToArray());
    }
}

