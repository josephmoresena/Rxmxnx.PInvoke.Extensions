using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rxmxnx.PInvoke.Internal;

namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

public class GetHashCodeTest : FixedContextTestsBase
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

    private void Test<T>() where T: unmanaged
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
            Int32 hash = HashCode.Combine(new IntPtr(ptrValue), binaryLength, false, typeof(T));
            Int32 hashReadOnly = HashCode.Combine(new IntPtr(ptrValue), binaryLength, true, typeof(T));

            Assert.Equal(!isReadOnly, hash.Equals(ctx.GetHashCode()));
            Assert.Equal(isReadOnly, hashReadOnly.Equals(ctx.GetHashCode()));

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
    private static unsafe void TransformationTest<T, TInt>(FixedContext<T> ctx, Boolean readOnly, Int32 length)
        where T: unmanaged
        where TInt : unmanaged
    {
        ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
        void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
        ReadOnlySpan<TInt> transformedSpan = new(ptr, length * sizeof(T) / sizeof(TInt));
        WithFixed(transformedSpan, readOnly, ctx, Test);
    }
    private static void Test<T, TInt>(FixedContext<TInt> ctx2, FixedContext<T> ctx)
        where T : unmanaged
        where TInt : unmanaged
        => Assert.Equal(typeof(TInt) == typeof(T), ctx2.GetHashCode().Equals(ctx.GetHashCode()));
}

