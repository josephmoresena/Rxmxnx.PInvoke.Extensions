namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class IsZeroTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(false, true)]
    internal void IntPtrTest(Boolean isZero, Boolean minValue = false)
    {
        IntPtr input = isZero ? IntPtr.Zero : !minValue ? IntPtr.MaxValue : IntPtr.MinValue;
        Assert.Equal(isZero, input.IsZero());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(false, true)]
    internal void UIntPtrTest(Boolean isZero, Boolean minValue = false)
    {
        UIntPtr input = isZero ? UIntPtr.Zero : !minValue ? UIntPtr.MaxValue : UIntPtr.MinValue;
        Assert.Equal(isZero || minValue, input.IsZero());
    }
}