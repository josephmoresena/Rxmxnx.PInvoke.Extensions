namespace Rxmxnx.PInvoke.Tests.Internal;

[TestFixture]
[ExcludeFromCodeCoverage]
public class FixedValueHandleTests
{
	[Fact]
	public void Test()
	{
		IWrapper<Boolean> result = PInvokeAssert.IsType<IWrapper<Boolean>>(FixedValueHandle.EmptyDisposable);
		PInvokeAssert.False(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.False(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.False(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.False(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.False(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.False(result.Value);
	}
}