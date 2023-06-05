namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafeSpanTest
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

        fixed (void* p = &MemoryMarshal.GetReference(input.AsSpan()))
        {
            IntPtr intPtr = (IntPtr)p;
            UIntPtr uintPtr = (UIntPtr)p;

            Assert.Equal(input, intPtr.GetUnsafeSpan<T>(input.Length).ToArray());
            Assert.Equal(input, intPtr.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());
            Assert.Equal(input, uintPtr.GetUnsafeSpan<T>(input.Length).ToArray());
            Assert.Equal(input, uintPtr.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());

            Assert.Equal(input, intPtr.GetUnsafeArray<T>(input.Length));
            Assert.Equal(input, uintPtr.GetUnsafeArray<T>(input.Length));
        }
    }
    private static void EmptyTest<T>(Int32 length) where T : unmanaged
    {
        if (length >= 0)
        {
            Span<T> result = IntPtr.Zero.GetUnsafeSpan<T>(length);
            ReadOnlySpan<T> result2 = IntPtr.Zero.GetUnsafeReadOnlySpan<T>(length);
            Span<T> result3 = UIntPtr.Zero.GetUnsafeSpan<T>(length);
            ReadOnlySpan<T> result4 = UIntPtr.Zero.GetUnsafeReadOnlySpan<T>(length);
            Assert.True(result.IsEmpty);
            Assert.True(result2.IsEmpty);
            Assert.True(result3.IsEmpty);
            Assert.True(result4.IsEmpty);

            Assert.Null(IntPtr.Zero.GetUnsafeArray<T>(length));
            Assert.Null(UIntPtr.Zero.GetUnsafeArray<T>(length));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeSpan<T>(length));
            Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeReadOnlySpan<T>(length));
            Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeSpan<T>(length));
            Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeReadOnlySpan<T>(length));
        }
    }
}