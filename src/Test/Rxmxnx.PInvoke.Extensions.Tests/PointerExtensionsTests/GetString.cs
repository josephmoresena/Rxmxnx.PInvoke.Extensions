namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class GetString
{
    private static readonly IFixture fixture = new Fixture();

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    internal void IntPtrZeroTest(Int32 length)
    {
        if (length >= 0)
        {
            Assert.Null(IntPtr.Zero.GetString(length));
            Assert.Null(UIntPtr.Zero.GetString(length));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetString(length));
            Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetString(length));
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    internal unsafe void IntPtrTest(Boolean fixedLength)
    {
        String input = fixture.Create<String>();
        fixed (void* p = input)
        {
            IntPtr intPtr = (IntPtr)p;
            UIntPtr uintPtr = (UIntPtr)p;

            Assert.Equal(input, intPtr.GetString(fixedLength ? input.Length : default));
            Assert.Equal(input, uintPtr.GetString(fixedLength ? input.Length : default));
        }
    }
}

