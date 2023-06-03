namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class ReadStringTest
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
            Assert.Null(IntPtr.Zero.ReadString(length));
            Assert.Null(UIntPtr.Zero.ReadString(length));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => IntPtr.Zero.ReadString(length));
            Assert.Throws<ArgumentException>(() => UIntPtr.Zero.ReadString(length));
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

            Assert.Equal(input, intPtr.ReadString(fixedLength ? input.Length : default));
            Assert.Equal(input, uintPtr.ReadString(fixedLength ? input.Length : default));
        }
    }
}

