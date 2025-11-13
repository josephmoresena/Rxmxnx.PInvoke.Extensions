#if NET5_0_OR_GREATER
using Skip = Xunit.Skip;

#elif NETCOREAPP
using SkippableTheoryAttribute = Xunit.TheoryAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
public sealed class GetNativeMethodTest
{
	internal delegate Int32 GetInt32();
	internal delegate T GetT<out T>();
#if NETCOREAPP
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
#if NET5_0_OR_GREATER
		Skip.If(SystemInfo.CompilationFramework.Contains(".NET Standard"),
		        ".NETStandard does not support NativeLibrary class.");
#else
		if (SystemInfo.CompilationFramework.Contains(".NET Standard")) return;
#endif

		String prefix = GetNativeMethodTest.fixture.Create<String>();
		String sufix = GetNativeMethodTest.fixture.Create<String>();
		IntPtr handle = !zeroPtr ?
			!useRealHandle ?
#if NET7_0_OR_GREATER
				GetNativeMethodTest.fixture.Create<Int32>() :
#else
				(IntPtr)GetNativeMethodTest.fixture.Create<Int32>() :
#endif
				NativeLibrary.Load(LoadNativeLibTest.LibraryName) :
			IntPtr.Zero;
		String? name = !emptyName ? prefix + LoadNativeLibTest.MethodName + sufix : default;
		Delegate? result;
#if NET5_0_OR_GREATER
		Skip.If(!zeroPtr && !useRealHandle,
		        "Calling this method with an invalid handle parameter other than IntPtr.Zero is not supported and will result in undefined behaviour.");
#else
		if (!zeroPtr && !useRealHandle) return;
#endif
		if (!generic)
			result = NativeUtilities.GetNativeMethod<GetInt32>(handle, name);
		else
			result = NativeUtilities.GetNativeMethod<GetT<Int32>>(handle, name);
		Assert.Null(result);
		Assert.Equal(default, NativeUtilities.GetNativeMethodPtr<GetInt32>(handle, name));
		Assert.Equal(default, NativeUtilities.GetNativeMethodPtr<GetT<Int32>>(handle, name));
		if (!zeroPtr && useRealHandle)
			NativeLibrary.Free(handle);
	}

	[SkippableTheory]
	[InlineData(true)]
	[InlineData(false)]
	internal void NormalTest(Boolean generic)
	{
#if NET5_0_OR_GREATER
		Skip.If(SystemInfo.CompilationFramework.Contains(".NET Standard"),
		        ".NETStandard does not support NativeLibrary class.");
#else
		if (SystemInfo.CompilationFramework.Contains(".NETStandard")) return;
#endif

		IntPtr handle = NativeLibrary.Load(LoadNativeLibTest.LibraryName);
		IntPtr addressMethod = NativeLibrary.GetExport(handle, LoadNativeLibTest.MethodName);
		if (!generic)
		{
			GetInt32? result = NativeUtilities.GetNativeMethod<GetInt32>(handle, LoadNativeLibTest.MethodName);
			FuncPtr<GetInt32> funcPtr =
				NativeUtilities.GetNativeMethodPtr<GetInt32>(handle, LoadNativeLibTest.MethodName);
			Assert.NotNull(result);
			Assert.Equal(addressMethod, funcPtr.Pointer);
#if NET5_0_OR_GREATER
			Assert.Equal(Environment.ProcessId, result());
			Assert.Equal(Environment.ProcessId, funcPtr.Invoke());
#else
			Assert.Equal(Process.GetCurrentProcess().Id, result());
			Assert.Equal(Process.GetCurrentProcess().Id, funcPtr.Invoke());
#endif
		}
		else
		{
			FuncPtr<GetT<Int32>> funcPtr =
				NativeUtilities.GetNativeMethodPtr<GetT<Int32>>(handle, LoadNativeLibTest.MethodName);
			Assert.Throws<ArgumentException>(() => NativeUtilities.GetNativeMethod<GetT<Int32>>(
				                                 handle, LoadNativeLibTest.MethodName));
			Assert.Equal(addressMethod, funcPtr.Pointer);
			Assert.Throws<ArgumentException>(() => funcPtr.Invoke);
		}
		NativeLibrary.Free(handle);
	}
#endif
}