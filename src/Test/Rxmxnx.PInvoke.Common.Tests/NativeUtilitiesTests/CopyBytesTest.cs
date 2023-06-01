namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
public sealed class CopyBytesTest
{
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal void ExceptionTest()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Span<Byte> bytes = stackalloc Byte[2];
            NativeUtilities.CopyBytes(fixture.Create<Decimal>(), bytes);
        });
    }

    [Fact]
    internal void NormalTest()
    {
        CopyTest<Boolean>();
        CopyTest<Byte>();
        CopyTest<Int16>();
        CopyTest<Int32>();
        CopyTest<Int64>();
        CopyTest<Single>();
        CopyTest<Double>();
        CopyTest<Decimal>();
    }

    private static unsafe void CopyTest<T>() where T : unmanaged
    {
        T[] values = fixture.CreateMany<T>().ToArray();
        Span<Byte> span = stackalloc Byte[values.Length * sizeof(T)];
        for (Int32 i = 0; i < values.Length; i++)
            NativeUtilities.CopyBytes(values[i], span, i * sizeof(T));
        Span<T> spanValues = MemoryMarshal.Cast<Byte, T>(span);
        Assert.Equal(values, spanValues.ToArray());
    }
}