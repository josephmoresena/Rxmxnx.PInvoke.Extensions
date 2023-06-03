namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
public sealed class GetPointerTest
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

    private static unsafe void Test<T>() where T : unmanaged
    {
        T[] values = fixture.CreateMany<T>(10).ToArray();
        Span<T> span = values;
        ReadOnlySpan<T> readOnlySpan = span;

        fixed(void* ptr = &MemoryMarshal.GetReference(readOnlySpan))
        {
            IntPtr intPtr = (IntPtr)ptr;
            UIntPtr uintPtr = (UIntPtr)ptr;

            Assert.Equal(intPtr, span.GetIntPtr());
            Assert.Equal(uintPtr, span.GetUIntPtr());
            Assert.Equal(intPtr, readOnlySpan.GetIntPtr());
            Assert.Equal(uintPtr, readOnlySpan.GetUIntPtr());
        }
    }
}

