namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class OperatorsTest
{
    [Theory]
    [InlineData(false)]
    [InlineData(default)]
    [InlineData(true)]
    internal void AsUIntPtrTest(Boolean? input)
    {
        IntPtr inputValue = input.HasValue ? input.Value ? IntPtr.MaxValue : IntPtr.MinValue : IntPtr.Zero;
        if (!input.HasValue)
            Assert.Equal(UIntPtr.Zero, inputValue.ToUIntPtr());
        else if (Environment.Is64BitProcess)
        {
            if (input.Value)
                Assert.Equal(Int64.MaxValue, (Int64)inputValue.ToUIntPtr().ToUInt64());
            else
                Assert.Equal(Int64.MinValue, (Int64)inputValue.ToUIntPtr().ToUInt64());
        }
        else
        {
            if (input.Value)
                Assert.Equal(Int32.MaxValue, (Int32)inputValue.ToUIntPtr().ToUInt32());
            else
                Assert.Equal(Int32.MinValue, (Int32)inputValue.ToUIntPtr().ToUInt32());
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    internal void AsIntPtrTest(Boolean input)
    {
        UIntPtr inputValue = input ? UIntPtr.MaxValue : UIntPtr.MinValue;
        if (!input)
            Assert.Equal(IntPtr.Zero, inputValue.ToIntPtr());
        else if (Environment.Is64BitProcess)
            Assert.Equal(UInt64.MaxValue, (UInt64)inputValue.ToIntPtr().ToInt64());
        else
            Assert.Equal(UInt32.MaxValue, (UInt32)inputValue.ToIntPtr().ToInt32());
    }
}

