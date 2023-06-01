namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
public sealed class GetNativeMethodTest
{
    internal delegate Int32 GetInt32();
    internal delegate T GetT<T>();

    private static readonly IFixture fixture = new Fixture();

    [SkippableTheory]
    [InlineData(true, false)]
    [InlineData(false, true)] //https://github.com/dotnet/dotnet-api-docs/pull/7342
    [InlineData(true, true)]
    [InlineData(false, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)] //https://github.com/dotnet/dotnet-api-docs/pull/7342
    [InlineData(true, true, true)]
    [InlineData(false, false, true)]
    [InlineData(false, true, true, true)]
    [InlineData(false, false, true, true)]
    [InlineData(false, true, false, true)]
    [InlineData(false, false, false, true)]
    internal void EmptyTest(Boolean zeroPtr, Boolean generic, Boolean emptyName = false, Boolean useRealHandle = false)
    {
        String prefix = fixture.Create<String>();
        String sufix = fixture.Create<String>();
        IntPtr handle = !zeroPtr ?
            !useRealHandle ? (IntPtr)fixture.Create<Int32>() : NativeLibrary.Load(LoadNativeLibTest.LibraryName)
            : IntPtr.Zero;
        String? name = !emptyName ? prefix + LoadNativeLibTest.MethodName + sufix : default;
        Delegate? result;
        Skip.If(!zeroPtr && !useRealHandle, "Calling this method with an invalid handle parameter other than IntPtr.Zero is not supported and will result in undefined behaviour.");
        if (!generic)
            result = NativeUtilities.GetNativeMethod<GetInt32>(handle, name);
        else
            result = NativeUtilities.GetNativeMethod<GetT<Int32>>(handle, name);
        Assert.Null(result);
        if (!zeroPtr && useRealHandle)
            NativeLibrary.Free(handle);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NormalTest(Boolean generic)
    {
        IntPtr handle = NativeLibrary.Load(LoadNativeLibTest.LibraryName);
        if (!generic)
        {
            GetInt32? result = NativeUtilities.GetNativeMethod<GetInt32>(handle, LoadNativeLibTest.MethodName);
            Assert.NotNull(result);
            Assert.Equal(Environment.ProcessId, result());
        }
        else
        {
            Assert.Throws<ArgumentException>(() => NativeUtilities.GetNativeMethod<GetT<Int32>>(handle, LoadNativeLibTest.MethodName));
        }
        NativeLibrary.Free(handle);
    }
}