namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsValueTest
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
        Span<Byte> bytes = fixture.CreateMany<Byte>(sizeof(T)).ToArray();
        ReadOnlySpan<Byte> readOnlyBytes = bytes;
        T expected = GetValue<T>(bytes);

        ref T refValue = ref bytes.AsValue<T>();
        ref readonly T refReadOnlyValue = ref readOnlyBytes.AsValue<T>();

        Assert.True(Unsafe.AreSame(ref refValue, ref Unsafe.AsRef(refReadOnlyValue)));
        Assert.Equal(expected, refValue);

        Assert.Throws<InsufficientMemoryException>(() => InvalidTest<T>(true));
        Assert.Throws<InvalidCastException>(() => InvalidTest<T>(false));
        Assert.Throws<InsufficientMemoryException>(() => InvalidReadOnlyTest<T>(true));
        Assert.Throws<InvalidCastException>(() => InvalidReadOnlyTest<T>(false));
    }
    private static unsafe T GetValue<T>(ReadOnlySpan<Byte> bytes) where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(bytes))
            return Unsafe.Read<T>(ptr);
    }
    private static unsafe void InvalidTest<T>(Boolean empty) where T : unmanaged
    {
        Span<Byte> bytes = empty ? Array.Empty<Byte>() : fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
        _ = ref bytes.AsValue<T>();
    }
    private static unsafe void InvalidReadOnlyTest<T>(Boolean empty) where T : unmanaged
    {
        ReadOnlySpan<Byte> bytes = empty ? Array.Empty<Byte>() : fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
        _ = ref bytes.AsValue<T>();
    }
}

