namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class MayBeNonLiteralTest
{
	[Fact]
	public void Test()
	{
		if (!MemoryInspector.IsSupported) return;

		Byte[] heap = [1, 2, 3,];
		ReadOnlySpan<Byte> heapSpan = heap;
		ReadOnlySpan<Char> literal = "CONST_STRING_VALUE".AsSpan();

		PInvokeAssert.NotEqual(heapSpan.IsLiteral(), heapSpan.MayBeNonLiteral());
		PInvokeAssert.NotEqual(literal.IsLiteral(), literal.MayBeNonLiteral());
	}
}
