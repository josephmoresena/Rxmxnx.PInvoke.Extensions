#if NETCOREAPP
#if NET5_0_OR_GREATER
using Skip = Xunit.Skip;

#else
using SkippableTheoryAttribute = Xunit.TheoryAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
public sealed class LoadNativeLibTest
{
	private const String LIBRARYNAME_WINDOWS = "kernel32.dll";
	private const String LIBRARYNAME_OSX = "libSystem.B.dylib";
	private const String LIBRARYNAME_LINUX = "libc.so.6";
	private const String LIBRARYNAME_FREEBSD = "libc.so.7";

	private const String METHODNAME_WINDOWS = "GetCurrentProcessId";
	private const String METHODNAME_UNIX = "getpid";

	private static readonly IFixture fixture = new Fixture();
	public static readonly String LibraryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
		LoadNativeLibTest.LIBRARYNAME_WINDOWS :
		RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? LoadNativeLibTest.LIBRARYNAME_OSX :
			RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ? LoadNativeLibTest.LIBRARYNAME_FREEBSD :
				LoadNativeLibTest.LIBRARYNAME_LINUX;
	public static readonly String MethodName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
		LoadNativeLibTest.METHODNAME_WINDOWS :
		LoadNativeLibTest.METHODNAME_UNIX;

	[SkippableTheory]
	[InlineData(true)]
	[InlineData(false)]
	[InlineData(true, true)]
	[InlineData(false, true)]
	internal void EmptyTest(Boolean unloadEvent, Boolean emptyName = false)
	{
#if NET5_0_OR_GREATER
		Skip.If(SystemInfo.CompilationFramework.Contains(".NET Standard"),
		        ".NETStandard does not support NativeLibrary class.");
#else
		if (SystemInfo.CompilationFramework.Contains(".NETStandard")) return;
#endif

		String prefix = LoadNativeLibTest.fixture.Create<String>();
		String sufix = LoadNativeLibTest.fixture.Create<String>();
		IntPtr? result = default;
		EventHandler? eventHandler = default;
		String? libraryName = !emptyName ? prefix + LoadNativeLibTest.LibraryName + sufix : default;
		if (unloadEvent)
		{
			result = NativeUtilities.LoadNativeLib(libraryName, ref eventHandler);
			Assert.Null(eventHandler);
		}
		else
		{
			result = NativeUtilities.LoadNativeLib(libraryName);
		}
		Assert.Null(result);
	}

	[SkippableTheory]
	[InlineData(true)]
	[InlineData(false)]
	internal void NormalTest(Boolean unloadEvent)
	{
#if NET5_0_OR_GREATER
		Skip.If(SystemInfo.CompilationFramework.Contains(".NET Standard"),
		        ".NETStandard does not support NativeLibrary class.");
#else
		if (SystemInfo.CompilationFramework.Contains(".NETStandard")) return;
#endif

		EventHandler? eventHandler = default;
		IntPtr? result = unloadEvent ?
			NativeUtilities.LoadNativeLib(LoadNativeLibTest.LibraryName, ref eventHandler) :
			NativeUtilities.LoadNativeLib(LoadNativeLibTest.LibraryName);
		Assert.NotNull(result);
		if (unloadEvent)
		{
			Assert.NotNull(eventHandler);
			eventHandler(null, null!);
		}
		else
		{
			NativeLibrary.Free(result.Value);
		}
	}
}
#endif
