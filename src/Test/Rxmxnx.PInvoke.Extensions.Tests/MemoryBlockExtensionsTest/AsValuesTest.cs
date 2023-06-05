namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsValuesTest
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
        Test<T, Byte>(values);
        Test<T, Char>(values);
        Test<T, DateTime>(values);
        Test<T, Decimal>(values);
        Test<T, Double>(values);
        Test<T, Guid>(values);
        Test<T, Half>(values);
        Test<T, Int16>(values);
        Test<T, Int32>(values);
        Test<T, Int64>(values);
        Test<T, SByte>(values);
        Test<T, Single>(values);
        Test<T, UInt16>(values);
        Test<T, UInt32>(values);
        Test<T, UInt64>(values);
    }
    private static unsafe void Test<T, T2>(T[] values)
        where T : unmanaged where T2 : unmanaged
    {
        Span<T> span = values;
        ReadOnlySpan<T> readOnlySpan = values;
        Span<Byte> byteSpan = MemoryMarshal.AsBytes(span);
        Span<T2> spanT2 = MemoryMarshal.Cast<Byte, T2>(byteSpan);
        Span<Byte> spanResidual = byteSpan[(spanT2.Length * sizeof(T2))..];

        T2[] valuesT2 = spanT2.ToArray();
        Byte[] resiudalBytes = spanResidual.ToArray();

        Assert.Equal(valuesT2, span.AsValues<T, T2>().ToArray());
        Assert.Equal(valuesT2, readOnlySpan.AsValues<T, T2>().ToArray());
        Assert.Equal(valuesT2, span.AsValues<T, T2>(out Span<Byte> residual).ToArray());
        Assert.Equal(resiudalBytes, residual.ToArray());
        Assert.Equal(valuesT2, span.AsValues<T, T2>(out ReadOnlySpan<Byte> residualRO).ToArray());
        Assert.Equal(resiudalBytes, residualRO.ToArray());
        Assert.Equal(valuesT2, readOnlySpan.AsValues<T, T2>(out ReadOnlySpan<Byte> residualRO2).ToArray());
        Assert.Equal(resiudalBytes, residualRO2.ToArray());

        Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanResidual), ref MemoryMarshal.GetReference(residual)));
        Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanResidual), ref MemoryMarshal.GetReference(residualRO)));
        Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanResidual), ref MemoryMarshal.GetReference(residualRO2)));

        Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanT2), ref MemoryMarshal.GetReference(span.AsValues<T, T2>())));
        Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanT2), ref MemoryMarshal.GetReference(readOnlySpan.AsValues<T, T2>())));
    }
}