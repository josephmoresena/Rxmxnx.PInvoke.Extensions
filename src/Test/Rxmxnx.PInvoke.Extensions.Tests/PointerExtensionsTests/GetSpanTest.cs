namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class GetSpanTest
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
        EmptyTest<T>(-1);
        EmptyTest<T>(0);
        EmptyTest<T>(1);

        T[] input = fixture.CreateMany<T>(10).ToArray();

        fixed(void* p = &MemoryMarshal.GetReference(input.AsSpan()))
        {
            IntPtr intPtr = (IntPtr)p;
            UIntPtr uintPtr = (UIntPtr)p;

            Assert.Equal(input, intPtr.GetSpan<T>(input.Length).ToArray());
            Assert.Equal(input, intPtr.GetReadOnlySpan<T>(input.Length).ToArray());
            Assert.Equal(input, uintPtr.GetSpan<T>(input.Length).ToArray());
            Assert.Equal(input, uintPtr.GetReadOnlySpan<T>(input.Length).ToArray());
        }
    }
    private static void EmptyTest<T>(Int32 length) where T : unmanaged
    {
        if (length >= 0)
        {
            Span<Byte> result = IntPtr.Zero.GetSpan<Byte>(length);
            ReadOnlySpan<Byte> result2 = IntPtr.Zero.GetReadOnlySpan<Byte>(length);
            Span<Byte> result3 = UIntPtr.Zero.GetSpan<Byte>(length);
            ReadOnlySpan<Byte> result4 = UIntPtr.Zero.GetReadOnlySpan<Byte>(length);
            Assert.True(result.IsEmpty);
            Assert.True(result2.IsEmpty);
            Assert.True(result3.IsEmpty);
            Assert.True(result4.IsEmpty);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetSpan<Byte>(length));
            Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetReadOnlySpan<Byte>(length));
            Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetSpan<Byte>(length));
            Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetReadOnlySpan<Byte>(length));
        }
    }
}