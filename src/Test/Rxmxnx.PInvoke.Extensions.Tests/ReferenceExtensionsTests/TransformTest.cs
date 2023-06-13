namespace Rxmxnx.PInvoke.Tests.ReferenceExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class TransformTest
{
    private static IFixture fixture = new Fixture();

    [Fact]
    internal void BooleanTest() => Test<Boolean>();
    [Fact]
    internal void ByteTest() => Test<Byte>();
    [Fact]
    internal void CharTest() => Test<Char>();
    [Fact]
    internal void DateTimeTest() => Test<DateTime>();
    [Fact]
    internal void DecimalTest() => Test<Decimal>();
    [Fact]
    internal void DoubleTest() => Test<Double>();
    [Fact]
    internal void GuidTest() => Test<Guid>();
    [Fact]
    internal void HalfTest() => Test<Half>();
    [Fact]
    internal void Int16Test() => Test<Int16>();
    [Fact]
    internal void Int32Test() => Test<Int32>();
    [Fact]
    internal void Int64Test() => Test<Int64>();
    [Fact]
    internal void SByteTest() => Test<SByte>();
    [Fact]
    internal void SingleTest() => Test<Single>();
    [Fact]
    internal void UInt16Test() => Test<UInt16>();
    [Fact]
    internal void UInt32Test() => Test<UInt32>();
    [Fact]
    internal void UInt64Test() => Test<UInt64>();

    private static void Test<T>() where T : unmanaged
    {
        T value = fixture.Create<T>();

        BinaryTest(ref value);
        Test<T, Boolean>(ref value);
        Test<T, Byte>(ref value);
        Test<T, Char>(ref value);
        Test<T, DateTime>(ref value);
        Test<T, Decimal>(ref value);
        Test<T, Double>(ref value);
        Test<T, Guid>(ref value);
        Test<T, Half>(ref value);
        Test<T, Int16>(ref value);
        Test<T, Int32>(ref value);
        Test<T, Int64>(ref value);
        Test<T, SByte>(ref value);
        Test<T, Single>(ref value);
        Test<T, UInt16>(ref value);
        Test<T, UInt32>(ref value);
        Test<T, UInt64>(ref value);
    }

    private static unsafe void Test<T, T2>(ref T refValue)
        where T : unmanaged
        where T2 : unmanaged
    {
        try
        {
            ref T2 refValue2 = ref refValue.Transform<T, T2>();
            Assert.Equal(sizeof(T), sizeof(T2));
            fixed (void* ptr1 = &refValue)
            fixed (void* ptr2 = &refValue2)
                Assert.Equal(new IntPtr(ptr1), new IntPtr(ptr2));

            if (typeof(T) == typeof(T2))
                Assert.Equal((Object)refValue, refValue2);
            else
            {
                Span<Byte> bytes1 = refValue.AsBytes();
                Span<Byte> bytes2 = refValue2.AsBytes();
                Assert.Equal(bytes1.ToArray(), bytes2.ToArray());
            }
        }
        catch (Exception ex)
        {
            Assert.IsType<InvalidOperationException>(ex);
            Assert.NotEqual(sizeof(T), sizeof(T2));
        }
    }

    private static unsafe void BinaryTest<T>(ref T refValue) where T : unmanaged
    {
        Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(refValue), 1)).ToArray();
        Span<Byte> span = refValue.AsBytes();
        Span<T> spanT = MemoryMarshal.Cast<Byte, T>(span);

        Assert.Equal(bytes, span.ToArray());
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(refValue), ref Unsafe.AsRef(spanT[0])));
    }
}

