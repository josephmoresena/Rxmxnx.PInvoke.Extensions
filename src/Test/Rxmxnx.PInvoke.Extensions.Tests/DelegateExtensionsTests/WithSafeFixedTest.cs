namespace Rxmxnx.PInvoke.Tests.DelegateExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
	[Fact]
	internal void Test()
	{
		GetInt32 getInt = WithSafeFixedTest.GetThreadId;
		IntPtr ptr = WithSafeFixedTest.GetMethodPtr();

		getInt.WithSafeFixed(WithSafeFixedTest.ActionTest);
		Assert.Equal(Environment.CurrentManagedThreadId, getInt.WithSafeFixed(WithSafeFixedTest.FuncTest));
		getInt.WithSafeFixed(WithSafeFixedTest.GetMethodDelegate(ptr), WithSafeFixedTest.ActionTest);
		Assert.Equal(Environment.CurrentManagedThreadId,
		             getInt.WithSafeFixed(WithSafeFixedTest.GetMethodDelegate(ptr), WithSafeFixedTest.FuncTest));
	}

	private static IntPtr GetMethodPtr()
		=> Marshal.GetFunctionPointerForDelegate<GetInt32>(WithSafeFixedTest.GetThreadId);
	private static GetInt32 GetMethodDelegate(IntPtr ptr) => Marshal.GetDelegateForFunctionPointer<GetInt32>(ptr);
	private static void ActionTest(in IFixedMethod<GetInt32> fMethod)
	{
		Assert.Equal(Environment.CurrentManagedThreadId, fMethod.Method());
		Assert.Equal(Marshal.GetFunctionPointerForDelegate<GetInt32>(WithSafeFixedTest.GetThreadId), fMethod.Pointer);
	}
	private static void ActionTest(in IFixedMethod<GetInt32> fMethod, GetInt32 instance)
	{
		WithSafeFixedTest.ActionTest(fMethod);
		Assert.Equal(fMethod.Method, instance);
		Assert.Equal(fMethod.Method(), instance());
	}
	private static Int32 FuncTest(in IFixedMethod<GetInt32> fMethod)
	{
		WithSafeFixedTest.ActionTest(fMethod);
		return fMethod.Method();
	}
	private static Int32 FuncTest(in IFixedMethod<GetInt32> fMethod, GetInt32 instance)
	{
		WithSafeFixedTest.ActionTest(fMethod, instance);
		return fMethod.Method();
	}
	private static Int32 GetThreadId() => Environment.CurrentManagedThreadId;
	private delegate Int32 GetInt32();
}