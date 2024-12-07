namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class GetUnsafePointerTest
{
	private static readonly IFixture fixture = ManagedStruct.Register(new Fixture());
	[Fact]
	internal void DelegateTest()
	{
		FuncPtr<GetProcessDelegate> getCurrentProcessPtr =
			NativeUtilities.GetUnsafeFuncPtr<GetProcessDelegate>(Process.GetCurrentProcess);
		FuncPtr<GetNativeMethodTest.GetInt32> getProcessIdPtr =
			NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdFunc);
		FuncPtr<GetNativeMethodTest.GetInt32> getProcessStaticIdPtr =
			NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdStaticFunc);

		Assert.Equal(getProcessStaticIdPtr.Invoke(), getCurrentProcessPtr.Invoke().Id);
		Assert.Equal(getCurrentProcessPtr,
		             NativeUtilities.GetUnsafeFuncPtr<GetProcessDelegate>(Process.GetCurrentProcess));
		Assert.NotEqual(getProcessIdPtr,
		                NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdFunc));
		Assert.Equal(getProcessStaticIdPtr,
		             NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdStaticFunc));

		Assert.Throws<ArgumentException>(
			() => NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetT<Int32>>(Thread.GetCurrentProcessorId));
		Assert.Throws<ArgumentException>(
			() => NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetT<Int32>>(GetProcessIdStaticFunc));
		Assert.Throws<ArgumentException>(
			() => NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetT<Int32>>(GetProcessIdFunc));

		Assert.Equal(getCurrentProcessPtr.Invoke, Process.GetCurrentProcess);
		Assert.Equal(getProcessIdPtr.Invoke, GetProcessIdFunc);
		Assert.Equal(getProcessStaticIdPtr.Invoke, GetProcessIdStaticFunc);

		return;

		Int32 GetProcessIdFunc() => Environment.ProcessId;
		static Int32 GetProcessIdStaticFunc() => Environment.ProcessId;
	}
	[Fact]
	internal void BooleanTest() => GetUnsafePointerTest.UnmanagedTest<Boolean>();
	[Fact]
	internal void ByteTest() => GetUnsafePointerTest.UnmanagedTest<Byte>();
	[Fact]
	internal void CharTest() => GetUnsafePointerTest.UnmanagedTest<Char>();
	[Fact]
	internal void DateTimeTest() => GetUnsafePointerTest.UnmanagedTest<DateTime>();
	[Fact]
	internal void DecimalTest() => GetUnsafePointerTest.UnmanagedTest<Decimal>();
	[Fact]
	internal void DoubleTest() => GetUnsafePointerTest.UnmanagedTest<Double>();
	[Fact]
	internal void GuidTest() => GetUnsafePointerTest.UnmanagedTest<Guid>();
	[Fact]
	internal void HalfTest() => GetUnsafePointerTest.UnmanagedTest<Half>();
	[Fact]
	internal void Int16Test() => GetUnsafePointerTest.UnmanagedTest<Int16>();
	[Fact]
	internal void Int32Test() => GetUnsafePointerTest.UnmanagedTest<Int32>();
	[Fact]
	internal void Int64Test() => GetUnsafePointerTest.UnmanagedTest<Int64>();
	[Fact]
	internal void SByteTest() => GetUnsafePointerTest.UnmanagedTest<SByte>();
	[Fact]
	internal void SingleTest() => GetUnsafePointerTest.UnmanagedTest<Single>();
	[Fact]
	internal void UInt16Test() => GetUnsafePointerTest.UnmanagedTest<UInt16>();
	[Fact]
	internal void UInt32Test() => GetUnsafePointerTest.UnmanagedTest<UInt32>();
	[Fact]
	internal void UInt64Test() => GetUnsafePointerTest.UnmanagedTest<UInt64>();
	[Fact]
	internal void ManagedStructTest() => GetUnsafePointerTest.ManagedTest<ManagedStruct>();
	[Fact]
	internal void StringTest() => GetUnsafePointerTest.ManagedTest<String>();
	private static unsafe void UnmanagedTest<T>() where T : unmanaged
	{
		T value = GetUnsafePointerTest.fixture.Create<T>();
		ref readonly T refValue = ref value;
		fixed (void* ptr = &refValue)
		{
			Assert.Equal((IntPtr)ptr, NativeUtilities.GetUnsafeIntPtr(value));
			Assert.Equal((UIntPtr)ptr, NativeUtilities.GetUnsafeUIntPtr(value));
			Assert.Equal((ReadOnlyValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtr(value));
			Assert.Equal((ValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtrFromRef(ref value));
		}
	}
	private static unsafe void ManagedTest<T>()
	{
		T value = GetUnsafePointerTest.fixture.Create<T>();
		ref T refValue = ref value;
		fixed (void* ptr = &refValue)
		{
			Assert.Equal((ReadOnlyValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtr(value));
			Assert.Equal((ValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtrFromRef(ref value));
		}
	}
	private delegate Process GetProcessDelegate();
}
#pragma warning restore CS8500