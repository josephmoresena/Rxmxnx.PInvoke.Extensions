namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsBytesTest
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
        T[] values = fixture.CreateMany<T>(10).ToArray();
        Span<T> span = values;
        ReadOnlySpan<T> readOnlySpan = span;

        Span<Byte> spanBytes = MemoryMarshal.AsBytes(span);
        Byte[] bytes = spanBytes.ToArray();

        Assert.Equal(bytes, span.AsBytes().ToArray());
        Assert.Equal(bytes, readOnlySpan.AsBytes().ToArray());
        Assert.True(Unsafe.AreSame(ref spanBytes[0], ref MemoryMarshal.GetReference(span.AsBytes())));
        Assert.True(Unsafe.AreSame(ref spanBytes[0], ref MemoryMarshal.GetReference(readOnlySpan.AsBytes())));
    }
}