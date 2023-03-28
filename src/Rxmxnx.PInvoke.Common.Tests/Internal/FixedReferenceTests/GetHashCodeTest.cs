namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

public sealed class GetHashCodeTest : FixedReferenceTestsBase
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
            T value = fixture.Create<T>();
            base.WithFixed(ref Unsafe.AsRef(value), true, Test);
            base.WithFixed(ref Unsafe.AsRef(value), false, Test);
        }
    }

    private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
    {
        Boolean isReadOnly = fref.IsReadOnly;

        Int32 binaryLength = sizeof(T);
        HashCode hash = new();
        HashCode hashReadOnly = new();

        hash.Add(ptr);
        hash.Add(0);
        hash.Add(binaryLength);
        hash.Add(false);
        hash.Add(typeof(T));
        hashReadOnly.Add(ptr);
        hashReadOnly.Add(0);
        hashReadOnly.Add(binaryLength);
        hashReadOnly.Add(true);
        hashReadOnly.Add(typeof(T));

        Assert.Equal(!isReadOnly, hash.ToHashCode().Equals(fref.GetHashCode()));
        Assert.Equal(isReadOnly, hashReadOnly.ToHashCode().Equals(fref.GetHashCode()));

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
        if (sizeof(T2) > fref.BinaryLength)
            return;

        ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
        void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(valueRef));
        ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);
        WithFixed(transformedRef, readOnly, fref, Test);
    }
    private static void Test<T, TInt>(FixedReference<TInt> fref2, FixedReference<T> fref)
        where T : unmanaged
        where TInt : unmanaged
        => Assert.Equal(typeof(TInt) == typeof(T), fref2.GetHashCode().Equals(fref.GetHashCode()));
}

