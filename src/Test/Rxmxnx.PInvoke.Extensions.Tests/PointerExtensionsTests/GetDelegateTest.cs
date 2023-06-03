namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class GetDelegateTest
{
    private delegate T GetValue<T>(T value);
    private delegate Byte GetByteValue(Byte value);

    private static readonly IFixture fixture = new Fixture();

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    internal void EmptyTest(Boolean useGeneric)
    {
        if (useGeneric)
        {
            Assert.Null(IntPtr.Zero.GetDelegate<GetValue<Byte>>());
            Assert.Null(UIntPtr.Zero.GetDelegate<GetValue<Byte>>());
        }
        else
        {
            Assert.Null(IntPtr.Zero.GetDelegate<GetByteValue>());
            Assert.Null(UIntPtr.Zero.GetDelegate<GetByteValue>());
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    internal unsafe void NormalTest(Boolean useGeneric)
    {
        IntPtr intPtr = Marshal.GetFunctionPointerForDelegate<GetByteValue>(GetByte);
        UIntPtr uIntPtr = (UIntPtr)intPtr.ToPointer();
        Byte input = fixture.Create<Byte>();
        if (!useGeneric)
        {
            Assert.Equal(GetByte(input), intPtr.GetDelegate<GetByteValue>()!(input));
            Assert.Equal(GetByte(input), uIntPtr.GetDelegate<GetByteValue>()!(input));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => intPtr.GetDelegate<GetValue<Byte>>()!(input));
            Assert.Throws<ArgumentException>(() => uIntPtr.GetDelegate<GetValue<Byte>>()!(input));
        }
    }

    private static Byte GetByte(Byte value) => value;
}
