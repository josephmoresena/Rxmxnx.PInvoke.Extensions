namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class GetUnsafePointerTest
{
	private static readonly IFixture fixture = ManagedStruct.Register(new Fixture());
	[Fact]
	public void DelegateTest()
	{
		FuncPtr<GetProcessDelegate> getCurrentProcessPtr =
			NativeUtilities.GetUnsafeFuncPtr<GetProcessDelegate>(Process.GetCurrentProcess);
		FuncPtr<GetNativeMethodTest.GetInt32> getProcessIdPtr =
			NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdFunc);
		FuncPtr<GetNativeMethodTest.GetInt32> getProcessStaticIdPtr =
			NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdStaticFunc);

		PInvokeAssert.Equal(getProcessStaticIdPtr.Invoke(), getCurrentProcessPtr.Invoke().Id);
		PInvokeAssert.Equal(getCurrentProcessPtr,
		                    NativeUtilities.GetUnsafeFuncPtr<GetProcessDelegate>(Process.GetCurrentProcess));
#if NETCOREAPP
		Assert.NotEqual(getProcessIdPtr,
		                NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdFunc));
#endif
		PInvokeAssert.Equal(getProcessStaticIdPtr,
		                    NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetInt32>(GetProcessIdStaticFunc));

#if NETCOREAPP
		Assert.Throws<ArgumentException>(() => NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetT<Int32>>(
			                                 Thread.GetCurrentProcessorId));
		Assert.Throws<ArgumentException>(() => NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetT<Int32>>(
			                                 GetProcessIdStaticFunc));
		Assert.Throws<ArgumentException>(() => NativeUtilities.GetUnsafeFuncPtr<GetNativeMethodTest.GetT<Int32>>(
			                                 GetProcessIdFunc));
#endif

		PInvokeAssert.Equal(getCurrentProcessPtr.Invoke, Process.GetCurrentProcess);
		PInvokeAssert.Equal(getProcessIdPtr.Invoke, GetProcessIdFunc);
		PInvokeAssert.Equal(getProcessStaticIdPtr.Invoke, GetProcessIdStaticFunc);

		return;

#if NET5_0_OR_GREATER
		Int32 GetProcessIdFunc() => Environment.ProcessId;
		static Int32 GetProcessIdStaticFunc() => Environment.ProcessId;
#else
		Int32 GetProcessIdFunc() => Process.GetCurrentProcess().Id;
		static Int32 GetProcessIdStaticFunc() => Process.GetCurrentProcess().Id;
#endif
	}
	[Fact]
	public void BooleanTest() => GetUnsafePointerTest.UnmanagedTest<Boolean>();
	[Fact]
	public void ByteTest() => GetUnsafePointerTest.UnmanagedTest<Byte>();
	[Fact]
	public void CharTest() => GetUnsafePointerTest.UnmanagedTest<Char>();
	[Fact]
	public void DateTimeTest() => GetUnsafePointerTest.UnmanagedTest<DateTime>();
	[Fact]
	public void DecimalTest() => GetUnsafePointerTest.UnmanagedTest<Decimal>();
	[Fact]
	public void DoubleTest() => GetUnsafePointerTest.UnmanagedTest<Double>();
	[Fact]
	public void GuidTest() => GetUnsafePointerTest.UnmanagedTest<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetUnsafePointerTest.UnmanagedTest<Half>();
#endif
	[Fact]
	public void Int16Test() => GetUnsafePointerTest.UnmanagedTest<Int16>();
	[Fact]
	public void Int32Test() => GetUnsafePointerTest.UnmanagedTest<Int32>();
	[Fact]
	public void Int64Test() => GetUnsafePointerTest.UnmanagedTest<Int64>();
	[Fact]
	public void SByteTest() => GetUnsafePointerTest.UnmanagedTest<SByte>();
	[Fact]
	public void SingleTest() => GetUnsafePointerTest.UnmanagedTest<Single>();
	[Fact]
	public void UInt16Test() => GetUnsafePointerTest.UnmanagedTest<UInt16>();
	[Fact]
	public void UInt32Test() => GetUnsafePointerTest.UnmanagedTest<UInt32>();
	[Fact]
	public void UInt64Test() => GetUnsafePointerTest.UnmanagedTest<UInt64>();
	[Fact]
	public void ManagedStructTest() => GetUnsafePointerTest.ManagedTest<ManagedStruct>();
	[Fact]
	public void StringTest() => GetUnsafePointerTest.ManagedTest<String>();
	private static unsafe void UnmanagedTest<T>() where T : unmanaged
	{
		T value = GetUnsafePointerTest.fixture.Create<T>();
		ref readonly T refValue = ref value;
		fixed (void* ptr = &refValue)
		{
			PInvokeAssert.Equal((IntPtr)ptr, NativeUtilities.GetUnsafeIntPtr(value));
			PInvokeAssert.Equal((UIntPtr)ptr, NativeUtilities.GetUnsafeUIntPtr(value));
			PInvokeAssert.Equal((ReadOnlyValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtr(value));
			PInvokeAssert.Equal((ValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtrFromRef(ref value));
		}
	}
	private static unsafe void ManagedTest<T>()
	{
		T value = GetUnsafePointerTest.fixture.Create<T>();
		ref T refValue = ref value;
		fixed (void* ptr = &refValue)
		{
			PInvokeAssert.Equal((ReadOnlyValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtr(value));
			PInvokeAssert.Equal((ValPtr<T>)(IntPtr)ptr, NativeUtilities.GetUnsafeValPtrFromRef(ref value));
		}
	}
	private delegate Process GetProcessDelegate();
}
#pragma warning restore CS8500