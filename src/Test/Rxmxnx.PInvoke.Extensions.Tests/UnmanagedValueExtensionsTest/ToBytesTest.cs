namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[ExcludeFromCodeCoverage]
public sealed class ToBytesTest
{
    private static readonly IFixture fixture = new Fixture();

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

    private static unsafe void Test<T>() where T : unmanaged
    {
        T value = fixture.Create<T>();
        T[] values = fixture.CreateMany<T>(10).ToArray();
        Byte[] bytes = GetBytes<T>(ref value);
        Byte[] bytesValues = MemoryMarshal.AsBytes(values.AsSpan()).ToArray();

        Assert.Equal(bytes, value.ToBytes());
        Assert.Equal(bytesValues, values.ToBytes());
    }
    private static unsafe Byte[] GetBytes<T>(ref T refValue) where T : unmanaged
    {
        fixed (void* ptr = &refValue)
            return new ReadOnlySpan<Byte>(ptr, sizeof(T)).ToArray();
    }
}

