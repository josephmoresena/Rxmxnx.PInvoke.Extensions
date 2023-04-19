namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
public sealed class EqualsTest : FixedReferenceTestsBase
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
        T value = fixture.Create<T>();
        base.WithFixed(ref Unsafe.AsRef(value), true, Test);
        base.WithFixed(ref Unsafe.AsRef(value), false, Test);
    }

    private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
    {
        Test(fref, false);
        Test(fref, true);
    }

    private static unsafe void Test<T>(FixedReference<T> fref, Boolean isReadOnly) where T : unmanaged
    {
        TransformationTest<T, Boolean>(fref, isReadOnly);
        TransformationTest<T, Byte>(fref, isReadOnly);
        TransformationTest<T, Int16>(fref, isReadOnly);
        TransformationTest<T, Char>(fref, isReadOnly);
        TransformationTest<T, Int32>(fref, isReadOnly);
        TransformationTest<T, Int64>(fref, isReadOnly);
        TransformationTest<T, Int128>(fref, isReadOnly);
        TransformationTest<T, Single>(fref, isReadOnly);
        TransformationTest<T, Half>(fref, isReadOnly);
        TransformationTest<T, Double>(fref, isReadOnly);
        TransformationTest<T, Decimal>(fref, isReadOnly);
        TransformationTest<T, DateTime>(fref, isReadOnly);
        TransformationTest<T, TimeOnly>(fref, isReadOnly);
        TransformationTest<T, TimeSpan>(fref, isReadOnly);
    }
    private static unsafe void TransformationTest<T, T2>(FixedReference<T> fref, Boolean readOnly)
        where T : unmanaged
        where T2 : unmanaged
    {
        ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
        void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(valueRef));
        Int32 binaryLength = sizeof(T);

        if (binaryLength >= sizeof(T2))
        {
            ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

            Assert.Equal(binaryLength, fref.BinaryLength);
            WithFixed(transformedRef, readOnly, fref, Test);
        }
    }
    private static void Test<T, TInt>(FixedReference<TInt> fref2, FixedReference<T> fref)
        where T : unmanaged
        where TInt : unmanaged
    {
        Boolean equal = fref.IsReadOnly == fref2.IsReadOnly && typeof(TInt) == typeof(T);

        Assert.Equal(equal, fref2.Equals(fref));
        Assert.Equal(equal, fref2.Equals((Object)fref));
        Assert.Equal(equal, fref2.Equals(fref as FixedReference<TInt>));
        Assert.False(fref2.Equals(null));
        Assert.False(fref2.Equals(new Object()));
    }
}

