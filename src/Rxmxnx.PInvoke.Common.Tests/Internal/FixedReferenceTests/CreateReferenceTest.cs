namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

public sealed class CreateReferenceTest : FixedReferenceTestsBase
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
        T value = fixture.Create<T>();
        base.WithFixed(ref Unsafe.AsRef(value), false, Test);
        Exception readOnly = Assert.Throws<InvalidOperationException>(() => base.WithFixed(ref Unsafe.AsRef(value), true, Test));
        Assert.Equal(ReadOnlyError, readOnly.Message);
    }

    private unsafe static void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
    {
        ref T refValue = ref fref.CreateReference<T>();
        IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
        Assert.Equal(ptr2, ptr);
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
        Assert.Equal(sizeof(T), fref.BinaryLength);
        Assert.Equal(0, fref.BinaryOffset);
        Assert.Equal(typeof(T), fref.Type);

        TestSize<T, Boolean>(fref);
        TestSize<T, Byte>(fref);
        TestSize<T, Int16>(fref);
        TestSize<T, Char>(fref);
        TestSize<T, Int32>(fref);
        TestSize<T, Int64>(fref);
        TestSize<T, Int128>(fref);
        TestSize<T, Guid>(fref);
        TestSize<T, Single>(fref);
        TestSize<T, Half>(fref);
        TestSize<T, Double>(fref);
        TestSize<T, Decimal>(fref);
        TestSize<T, DateTime>(fref);
        TestSize<T, TimeOnly>(fref);
        TestSize<T, TimeSpan>(fref);

        fref.Unload();
        Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
        Assert.Equal(InvalidError, invalid.Message);
    }

    private unsafe static void TestSize<T, T2>(FixedReference<T> fref) where T : unmanaged where T2 : unmanaged
    {
        Int32 size = sizeof(T);
        Int32 size2 = sizeof(T2);

        if(size < size2)
        {
            Exception invalidSize = Assert.Throws<InsufficientMemoryException>(() => fref.CreateReference<T2>());
            Assert.Equal(String.Format(InvalidSizeFormat, typeof(T2)), invalidSize.Message);
        }
        else
        {
            ref T value = ref fref.CreateReference<T>();
            ref T2 value2 = ref fref.CreateReference<T2>();
            Byte[] bytes1 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size).ToArray();
            Byte[] bytes2 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size2).ToArray();

            Assert.Equal(bytes1[..size2], bytes2);

            if (typeof(T) == typeof(T2))
                Assert.Equal((Object)value, (Object)value2);
        }
    }
}

