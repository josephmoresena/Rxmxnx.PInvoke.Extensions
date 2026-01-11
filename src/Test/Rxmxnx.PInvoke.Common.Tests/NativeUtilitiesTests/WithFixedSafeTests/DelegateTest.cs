namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests.WithFixedSafeTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class DelegateTest
{
	[Fact]
	public void Test()
	{
		IntPtr ptr = DelegateTest.GetMethodPtr();

		NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId, DelegateTest.ActionTest);

		PInvokeAssert.Equal(Environment.CurrentManagedThreadId,
		                    NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32, Int32>(
			                    DelegateTest.GetThreadId, DelegateTest.FuncTest));

		NativeUtilities.WithSafeFixed<GetNativeMethodTest.GetInt32, GetNativeMethodTest.GetInt32>(
			DelegateTest.GetThreadId, DelegateTest.GetMethodDelegate(ptr), DelegateTest.ActionTest);

		PInvokeAssert.Equal(Environment.CurrentManagedThreadId,
		                    NativeUtilities
			                    .WithSafeFixed<GetNativeMethodTest.GetInt32, GetNativeMethodTest.GetInt32, Int32>(
				                    DelegateTest.GetThreadId, DelegateTest.GetMethodDelegate(ptr),
				                    DelegateTest.FuncTest));
	}

	private static IntPtr GetMethodPtr()
		=> Marshal.GetFunctionPointerForDelegate<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId);
	private static GetNativeMethodTest.GetInt32 GetMethodDelegate(IntPtr ptr)
		=> Marshal.GetDelegateForFunctionPointer<GetNativeMethodTest.GetInt32>(ptr);
	private static void ActionTest(in IFixedMethod<GetNativeMethodTest.GetInt32> fMethod)
	{
		PInvokeAssert.Equal(Environment.CurrentManagedThreadId, fMethod.Method());
		PInvokeAssert.Equal(Environment.CurrentManagedThreadId, fMethod.FunctionPointer.Invoke());
		PInvokeAssert.Equal(
			Marshal.GetFunctionPointerForDelegate<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId),
			fMethod.Pointer);
		PInvokeAssert.Equal(
			Marshal.GetFunctionPointerForDelegate<GetNativeMethodTest.GetInt32>(DelegateTest.GetThreadId),
			fMethod.FunctionPointer.Pointer);
	}
	private static void ActionTest(in IFixedMethod<GetNativeMethodTest.GetInt32> fMethod,
		GetNativeMethodTest.GetInt32 instance)
	{
		DelegateTest.ActionTest(fMethod);
		PInvokeAssert.Equal(fMethod.Method, instance);
		PInvokeAssert.Equal(fMethod.FunctionPointer.Invoke, instance);
		PInvokeAssert.Equal(fMethod.Method(), instance());
		PInvokeAssert.Equal(fMethod.FunctionPointer.Invoke(), instance());
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