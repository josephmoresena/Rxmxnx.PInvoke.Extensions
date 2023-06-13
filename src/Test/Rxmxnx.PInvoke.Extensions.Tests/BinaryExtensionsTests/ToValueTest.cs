namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToValueTest
{
    private static IFixture fixture = new Fixture();

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
        Byte[] bytes = fixture.CreateMany<Byte>(sizeof(T)).ToArray();
        Span<Byte> byteSpan = bytes;
        ReadOnlySpan<Byte> readOnlyBytes = bytes;
        T expected = GetValue<T>(bytes);

        Assert.Equal(expected, bytes.ToValue<T>());
        Assert.Equal(expected, byteSpan.ToValue<T>());
        Assert.Equal(expected, readOnlyBytes.ToValue<T>());

        bytes = fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
        byteSpan = bytes;
        readOnlyBytes = bytes;
        expected = GetValue<T>(bytes);

        Assert.Equal(expected, bytes.ToValue<T>());
        Assert.Equal(expected, byteSpan.ToValue<T>());
        Assert.Equal(expected, readOnlyBytes.ToValue<T>());

        bytes = fixture.CreateMany<Byte>(sizeof(T) - 1).ToArray();
        byteSpan = bytes;
        readOnlyBytes = bytes;
        expected = GetValue<T>(bytes);

        Assert.Equal(expected, bytes.ToValue<T>());
        Assert.Equal(expected, byteSpan.ToValue<T>());
        Assert.Equal(expected, readOnlyBytes.ToValue<T>());

        bytes = Array.Empty<Byte>();
        byteSpan = bytes;
        readOnlyBytes = bytes;

        Assert.Equal(default, bytes.ToValue<T>());
        Assert.Equal(default, byteSpan.ToValue<T>());
        Assert.Equal(default, readOnlyBytes.ToValue<T>());
    }
    private static unsafe T GetValue<T>(ReadOnlySpan<Byte> bytes) where T : unmanaged
    {
        if (bytes.Length >= sizeof(T))
            fixed (void* ptr = &MemoryMarshal.GetReference(bytes))
                return Unsafe.Read<T>(ptr);
        else
        {
            Span<Byte> result = stackalloc Byte[sizeof(T)];
            bytes.CopyTo(result);
            fixed (void* ptr = &MemoryMarshal.GetReference(result))
                return Unsafe.Read<T>(ptr);
        }
    }
}

