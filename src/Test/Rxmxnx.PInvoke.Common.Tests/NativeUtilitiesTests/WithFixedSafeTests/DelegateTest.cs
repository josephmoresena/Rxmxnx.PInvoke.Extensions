namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests.WithFixedSafeTests;

[ExcludeFromCodeCoverage]
public sealed class DelegateTest
{
	[Fact]
	internal void Test()
	{
		IntPtr ptr = DelegateTest.GetMethodPtr();

		NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId, DelegateTest.ActionTest);

		Assert.Equal(Environment.CurrentManagedThreadId,
		             NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32, Int32>(
			             DelegateTest.GetThreadId, DelegateTest.FuncTest));

		NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32, GetNativeMethodTest.GetInt32>(
			DelegateTest.GetThreadId, DelegateTest.GetMethodDelegate(ptr), DelegateTest.ActionTest);

		Assert.Equal(Environment.CurrentManagedThreadId,
		             NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32, GetNativeMethodTest.GetInt32, Int32>(
			             DelegateTest.GetThreadId, DelegateTest.GetMethodDelegate(ptr), DelegateTest.FuncTest));
	}

	private static IntPtr GetMethodPtr()
		=> Marshal.GetFunctionPointerForDelegate<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId);
	private static GetNativeMethodTest.GetInt32 GetMethodDelegate(IntPtr ptr)
		=> Marshal.GetDelegateForFunctionPointer<GetNativeMethodTest.GetInt32>(ptr);
	private static void ActionTest(in IFixedMethod<GetNativeMethodTest.GetInt32> fMethod)
	{
		Assert.Equal(Environment.CurrentManagedThreadId, fMethod.Method());
		Assert.Equal(Marshal.GetFunctionPointerForDelegate<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId),
		             fMethod.Pointer);
	}
	private static void ActionTest(in IFixedMethod<GetNativeMethodTest.GetInt32> fMethod,
		GetNativeMethodTest.GetInt32 instance)
	{
		DelegateTest.ActionTest(fMethod);
		Assert.Equal(fMethod.Method, instance);
		Assert.Equal(fMethod.Method(), instance());
	}
	private static Int32 FuncTest(in IFixedMethod<GetNativeMethodTest.GetInt32> fMethod)
	{
		DelegateTest.ActionTest(fMethod);
		return fMethod.Method();
	}
	private static Int32 FuncTest(in IFixedMethod<GetNativeMethodTest.GetInt32> fMethod,
		GetNativeMethodTest.GetInt32 instance)
	{
		DelegateTest.ActionTest(fMethod, instance);
		return fMethod.Method();
	}

	private static Int32 GetThreadId() => Environment.CurrentManagedThreadId;
}