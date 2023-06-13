namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToValuesTest
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

    private static void Test<T>() where T : unmanaged
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
        ReadOnlySpan<T> span = values;
        ReadOnlySpan<Byte> byteSpan = MemoryMarshal.AsBytes(span);
        ReadOnlySpan<T2> spanT2 = MemoryMarshal.Cast<Byte, T2>(byteSpan);
        ReadOnlySpan<Byte> spanResidual = byteSpan[(spanT2.Length * sizeof(T2))..];

        T2[] valuesT2 = spanT2.ToArray();
        Byte[] resiudalBytes = spanResidual.ToArray();

        Assert.Equal(valuesT2, values.ToValues<T, T2>());
        Assert.Equal(valuesT2, values.ToValues<T, T2>(out Byte[] residual));
        Assert.Equal(resiudalBytes, residual);

        Assert.Equal(Array.Empty<T2>(), Array.Empty<T>().ToValues<T, T2>());
        Assert.Equal(Array.Empty<T2>(), Array.Empty<T>().ToValues<T, T2>(out Byte[] residualEmpty));
        Assert.Equal(Array.Empty<Byte>(), residualEmpty);

        T[]? nullValues = default;
        Assert.Null(nullValues.ToValues<T, T2>());
        Assert.Null(nullValues.ToValues<T, T2>(out Byte[]? residualNull));
        Assert.Null(residualNull);
    }
}

