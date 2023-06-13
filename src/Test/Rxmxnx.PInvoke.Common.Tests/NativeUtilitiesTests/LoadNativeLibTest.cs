namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
public sealed class LoadNativeLibTest
{
    private const String LIBRARYNAME_WINDOWS = "kernel32.dll";
    private const String LIBRARYNAME_OSX = "libSystem.B.dylib";
    private const String LIBRARYNAME_LINUX = "libc.so.6";

    private const String METHODNAME_WINDOWS = "GetCurrentProcessId";
    private const String METHODNAME_UNIX = "getpid";

    private static readonly IFixture fixture = new Fixture();
    public static readonly String LibraryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? LIBRARYNAME_WINDOWS :
           RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? LIBRARYNAME_OSX : LIBRARYNAME_LINUX;
    public static readonly String MethodName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? METHODNAME_WINDOWS : METHODNAME_UNIX;

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    internal void EmptyTest(Boolean unloadEvent, Boolean emptyName = false)
    {
        String prefix = fixture.Create<String>();
        String sufix = fixture.Create<String>();
        IntPtr? result = default;
        EventHandler? eventHandler = default;
        String? libraryName = !emptyName ? prefix + LibraryName + sufix : default;
        if (unloadEvent)
        {
            result = NativeUtilities.LoadNativeLib(libraryName, ref eventHandler);
            Assert.Null(eventHandler);
        }
        else
            result = NativeUtilities.LoadNativeLib(libraryName);
        Assert.Null(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NormalTest(Boolean unloadEvent)
    {
        IntPtr? result = default;
        EventHandler? eventHandler = default;
        if (unloadEvent)
            result = NativeUtilities.LoadNativeLib(LibraryName, ref eventHandler);
        else
            result = NativeUtilities.LoadNativeLib(LibraryName);
        Assert.NotNull(result);
        if (unloadEvent)
        {
            Assert.NotNull(eventHandler);
            eventHandler(null, null!);
        }
        else
            NativeLibrary.Free(result.Value);
    }
}
