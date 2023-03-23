namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

public sealed class GetHashCodeTest : FixedContextTestsBase
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
        unsafe
        {
            T[] values = fixture.CreateMany<T>(sizeof(Int128) * 3 / sizeof(T)).ToArray();
            base.WithFixed(values, true, Test);
            base.WithFixed(values, false, Test);
        }
    }

    private static unsafe void Test<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
    {
        Boolean isReadOnly = IsReadOnly(ctx);

        fixed (T* ptrValue = values)
        {
            Int32 binaryLength = values.Length * sizeof(T);
            HashCode hash = new();
            HashCode hashReadOnly = new();

            hash.Add(new IntPtr(ptrValue));
            hash.Add(0);
            hash.Add(binaryLength);
            hash.Add(false);
            hash.Add(typeof(T));
            hashReadOnly.Add(new IntPtr(ptrValue));
            hashReadOnly.Add(0);
            hashReadOnly.Add(binaryLength);
            hashReadOnly.Add(true);
            hashReadOnly.Add(typeof(T));

            Assert.Equal(!isReadOnly, hash.ToHashCode().Equals(ctx.GetHashCode()));
            Assert.Equal(isReadOnly, hashReadOnly.ToHashCode().Equals(ctx.GetHashCode()));

            TransformationTest<T, Boolean>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Byte>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Int16>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Char>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Int32>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Int64>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Int128>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Single>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Half>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Double>(ctx, isReadOnly, values.Length);
            TransformationTest<T, Decimal>(ctx, isReadOnly, values.Length);
            TransformationTest<T, DateTime>(ctx, isReadOnly, values.Length);
            TransformationTest<T, TimeOnly>(ctx, isReadOnly, values.Length);
            TransformationTest<T, TimeSpan>(ctx, isReadOnly, values.Length);
        }
    }
    private static unsafe void TransformationTest<T, T2>(FixedContext<T> ctx, Boolean readOnly, Int32 length)
        where T : unmanaged
        where T2 : unmanaged
    {
        ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
        void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
        ReadOnlySpan<T2> transformedSpan = new(ptr, length * sizeof(T) / sizeof(T2));
        WithFixed(transformedSpan, readOnly, ctx, Test);
    }
    private static void Test<T, TInt>(FixedContext<TInt> ctx2, FixedContext<T> ctx)
        where T : unmanaged
        where TInt : unmanaged
        => Assert.Equal(typeof(TInt) == typeof(T), ctx2.GetHashCode().Equals(ctx.GetHashCode()));
}

