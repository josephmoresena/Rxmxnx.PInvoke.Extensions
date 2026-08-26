namespace Rxmxnx.PInvoke.Tests.Internal;

[TestFixture]
[ExcludeFromCodeCoverage]
public class FixedValueHandleTests
{
	[Fact]
	public void Test()
	{
		IWrapper<Boolean>? result = FixedValueHandle.EmptyDisposable as IWrapper<Boolean>;
		PInvokeAssert.NotNull(result!);
		PInvokeAssert.True(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.True(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.True(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.True(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.True(result.Value);
		FixedValueHandle.EmptyDisposable.Dispose();
		PInvokeAssert.True(result.Value);
	}
}