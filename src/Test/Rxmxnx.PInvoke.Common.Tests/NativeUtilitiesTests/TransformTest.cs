namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
public sealed class TransformTest
{
    private static readonly IFixture fixture = new Fixture();

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

        BinaryTest(value);
        Test<T, Byte>(value);
        Test<T, Char>(value);
        Test<T, DateTime>(value);
        Test<T, Decimal>(value);
        Test<T, Double>(value);
        Test<T, Guid>(value);
        Test<T, Half>(value);
        Test<T, Int16>(value);
        Test<T, Int32>(value);
        Test<T, Int64>(value);
        Test<T, SByte>(value);
        Test<T, Single>(value);
        Test<T, UInt16>(value);
        Test<T, UInt32>(value);
        Test<T, UInt64>(value);
    }

    private static unsafe void Test<T, T2>(in T refValue)
        where T : unmanaged
        where T2 : unmanaged
    {
        try
        {
            ref readonly T2 refValue2 = ref NativeUtilities.Transform<T, T2>(refValue);
            Assert.Equal(sizeof(T), sizeof(T2));
            fixed (void* ptr1 = &refValue)
            fixed (void* ptr2 = &refValue2)
                Assert.Equal(new IntPtr(ptr1), new IntPtr(ptr2));

            if (typeof(T) == typeof(T2))
                Assert.Equal((Object)refValue, refValue2);
            else
            {
                ReadOnlySpan<Byte> bytes1 = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(refValue), 1));
                ReadOnlySpan<Byte> bytes2 = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(refValue2), 1));
                Assert.Equal(bytes1.ToArray(), bytes2.ToArray());
            }
        }
        catch (Exception ex)
        {
            Assert.IsType<InvalidOperationException>(ex);
            Assert.NotEqual(sizeof(T), sizeof(T2));
        }
    }

    private static unsafe void BinaryTest<T>(in T refValue) where T : unmanaged
    {
        Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(refValue), 1)).ToArray();
        ReadOnlySpan<Byte> span = NativeUtilities.AsBytes(refValue);
        ReadOnlySpan<T> spanT = MemoryMarshal.Cast<Byte, T>(span);

        Assert.Equal(bytes, NativeUtilities.ToBytes(refValue));
        Assert.Equal(bytes, span.ToArray());
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(refValue), ref Unsafe.AsRef(spanT[0])));
    }
}

