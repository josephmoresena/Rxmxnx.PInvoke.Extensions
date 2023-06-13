namespace Rxmxnx.PInvoke.Tests.DelegateExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
    private delegate Int32 GetInt32();

    [Fact]
    internal void Test()
    {
        GetInt32 getInt = GetThreadId;
        IntPtr ptr = GetMethodPtr();

        getInt.WithSafeFixed(ActionTest);
        Assert.Equal(Environment.CurrentManagedThreadId, getInt.WithSafeFixed(FuncTest));
        getInt.WithSafeFixed(GetMethodDelegate(ptr), ActionTest);
        Assert.Equal(Environment.CurrentManagedThreadId,
            getInt.WithSafeFixed(GetMethodDelegate(ptr), FuncTest));
    }

    private static IntPtr GetMethodPtr()
        => Marshal.GetFunctionPointerForDelegate<GetInt32>(GetThreadId);
    private static GetInt32 GetMethodDelegate(IntPtr ptr)
        => Marshal.GetDelegateForFunctionPointer<GetInt32>(ptr);
    private static void ActionTest(in IFixedMethod<GetInt32> fMethod)
    {
        Assert.Equal(Environment.CurrentManagedThreadId, fMethod.Method());
        Assert.Equal(Marshal.GetFunctionPointerForDelegate<GetInt32>(GetThreadId), fMethod.Pointer);
    }
    private static void ActionTest(in IFixedMethod<GetInt32> fMethod, GetInt32 instance)
    {
        ActionTest(fMethod);
        Assert.Equal(fMethod.Method, instance);
        Assert.Equal(fMethod.Method(), instance());
    }
    private static Int32 FuncTest(in IFixedMethod<GetInt32> fMethod)
    {
        ActionTest(fMethod);
        return fMethod.Method();
    }
    private static Int32 FuncTest(in IFixedMethod<GetInt32> fMethod, GetInt32 instance)
    {
        ActionTest(fMethod, instance);
        return fMethod.Method();
    }
    private static Int32 GetThreadId() => Environment.CurrentManagedThreadId;
}
